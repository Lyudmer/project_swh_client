﻿
using ClientSWH.Application.CollectingListToXml.HendlersElem;
using ClientSWH.Application.Common;
using ClientSWH.Application.Interfaces;

using ClientSWH.Core.Abstraction.Repositories;

using ClientSWH.Core.Models;
using ClientSWH.DocsRecordCore.Abstraction;
using ClientSWH.DocsRecordCore.Models;
using ClientSWH.DocsRecordDataAccess;
using ClientSWH.SendReceivServer.Producer;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace ClientSWH.SendReceivServer
{
    public class SendToServer(IPackagesRepository pkgRepository,
       IDocumentsRepository docRepository, IDocRecordRepository docRecordRepository
        , IMessagePublisher messagePublisher, IHistoryPkgRepository historyPkgRepository
       ) : ISendToServer
    {
        private readonly IMessagePublisher _messagePublisher = messagePublisher;
        private readonly IDocumentsRepository _docRepository = docRepository;
        private readonly IDocRecordRepository _docRecordRepository = docRecordRepository;
        private readonly IPackagesRepository _pkgRepository = pkgRepository;
        private readonly IHistoryPkgRepository _historyPkgRepository = historyPkgRepository;

        readonly DirectoryInfo CurDirINfo = new(Directory.GetCurrentDirectory());

        async Task<int> ISendToServer.SendPaskageToServer(int Pid)
        {

            int stPkg = _pkgRepository.GetById(Pid).Result.StatusId;

            try
            {
                // собрать xml
                var xPkg = await CreatePaskageXml(Pid, stPkg);
                // отправить на сервер 
                if (xPkg is not null)
                {
                    //проверить перед отправкой 
                    stPkg = PkgFLK(Pid).Result;

                    if (stPkg != 4)
                    {
                        var resStatus = _messagePublisher.SendMessage(xPkg.ToString(), "sendpkg", stPkg);
                        if (resStatus != stPkg)
                        {

                            await _pkgRepository.UpdateStatus(Pid, resStatus);
                            var hPkg = HistoryPkg.Create(Pid, stPkg, resStatus, "SendPkgToServer", DateTime.UtcNow);
                            await _historyPkgRepository.Add(hPkg);
                            stPkg = resStatus;
                        }
                    }
                }
                else stPkg = -1;
            }
            catch (Exception)
            {
                //string mess = ex.Message;

            }
            return stPkg;
        }
        async Task<bool> ISendToServer.SendDelPkgToServer(int Pid)
        {
            try
            {
                int stPkg = _pkgRepository.GetById(Pid).Result.StatusId;
                var xPkg = new XDocument(new XDeclaration("1.0", "UTF-8", null));
                var elem = new XElement("Package");
                elem.SetAttributeValue("pid", Pid);
                var elem_props = new XElement("package-properties",
                    new XElement("props", new XAttribute("name", "uuid"), _pkgRepository.GetById(Pid).Result.UUID.ToString()));
                elem.Add(elem_props);
                var resDel = _messagePublisher.SendMessage(xPkg.ToString(), "delpkg", 0);
                if (resDel == -1)
                {
                    await _pkgRepository.UpdateStatus(Pid, 107);
                    var hPkg = HistoryPkg.Create(Pid, stPkg, 107, "SendPkgToServer", DateTime.UtcNow);
                    await _historyPkgRepository.Add(hPkg);

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }

        }
        private async Task<XDocument> CreatePaskageXml(int Pid, int stPkg)
        {
            var xPkg = new XDocument(new XDeclaration("1.0", "UTF-8", null));
            var elem = new XElement("Package");
            elem.SetAttributeValue("pid", Pid);
            var elem_props = new XElement("package-properties"
                , new XElement("props", new XAttribute("name", "Status"), stPkg.ToString())
                , new XElement("props", new XAttribute("name", "uuid"), _pkgRepository.GetById(Pid).Result.UUID.ToString())
                , new XElement("props", new XAttribute("name", "CreateDate"), _pkgRepository.GetById(Pid).Result.CreateDate.ToString())
                , new XElement("props", new XAttribute("name", "UserId"), _pkgRepository.GetById(Pid).Result.UserId.ToString()));
            elem.Add(elem_props);
            var docs = await _docRepository.GetByFilter(Pid);
            // var resProc = new TasksHandlerElem(_docRecordContext, docs);
            // resProc.ProcessQueue(ref elem);
            foreach (var inDoc in docs)
            {
                var docRecord = new DocRecordBase();
                docRecord = await _docRecordRepository.GetByDocId(inDoc.DocId.ToString().Trim());
                XElement elem_doc = XElement.Parse(docRecord.DocText);
                elem_doc.SetAttributeValue("docid", inDoc.DocId.ToString());
                elem_doc.SetAttributeValue("doctype", inDoc.DocType);
                elem.Add(elem_doc);
            }
            xPkg.Add(elem);
            return xPkg;
        }
        public async Task<int> PkgFLK(int Pid)
        {
            int stPkg = _pkgRepository.GetById(Pid).Result.StatusId;
            XDocument resXml = new();
            int resFLK = 0;
            try
            {
                // var xPkg = await CreatePaskageXml(Pid, stPkg);
                var strPkg = await PkgXml(Pid);
                var xPkg =  XDocument.Parse(strPkg);
                if (xPkg != null)
                {
                    resXml = ResultTransform(xPkg, CurDirINfo.Parent + "\\Workflow\\FLK\\DesNotif_PIResult.FLK.xsl");
                    if (resXml != null)
                    {
                        var nodeError = resXml.XPathSelectElements(@"//ResultInformation/RESULTINFORMATION_ITEM[contains(ResultCategory,'ERROR')]");
                        if (nodeError is not null && nodeError.Any())
                        {
                            await _pkgRepository.UpdateStatus(Pid, 4);
                            await _historyPkgRepository.Add(HistoryPkg.Create(Pid, stPkg, 4, "Error", DateTime.UtcNow));
                            resFLK = await AddResPackage(resXml, Pid);
                        }
                    }
                }
                else resFLK = -1;
            }
            finally
            {
                //
            }
            return resFLK;
        }
        public async Task<int> AddResPackage(XDocument resXml, int Pid)
        {
            int resFLK = 0;
            try
            {
                XDocument xDoc = resXml;
                if (xDoc != null)
                {
                    var LastDocId = _docRepository.GetLastDocId().Result + 1;

                    var Doc = Document.Create(LastDocId, Guid.NewGuid(), "", DateTime.UtcNow, "00000", "DesNotif_PIResult.cfg.xml",
                                              resXml.ToString().Length, DopFunction.GetHashMd5(resXml.ToString()),
                                              DopFunction.GetSha256(resXml.ToString()), Pid, DateTime.UtcNow, DateTime.UtcNow);


                    Doc = await _docRepository.Add(Doc);
                    if (Doc is not null)
                    {

                        var dRecordId = await _docRecordRepository.AddRecord(Doc.DocId.ToString(), resXml.ToString());
                        resFLK = Doc.Id;
                    }

                }
            }
            catch
            {
                return 0;
            };
            return resFLK;
        }
        public static XDocument ResultTransform(XDocument inMess, string filexslt)
        {
            XDocument resXml = new();
            if (!File.Exists(filexslt)) return null;
            try
            {
                var transform = new XslCompiledTransform();
                transform.Load(filexslt);
                StringWriter output = new();
                XsltArgumentList args = new();
                XPathDocument document = new(new StringReader(inMess.ToString()));
                transform.Transform(document, args, output);
                if (String.IsNullOrEmpty(output.ToString())) return null;
                resXml = XDocument.Parse(output.ToString());

            }
            catch
            {
                return null;
            }
            return resXml;
        }
        public async Task<string> PkgXml(int Pid)
        {
            try
            {
                var xPkg = new XDocument(new XDeclaration("1.0", "UTF-8", null));
                var elem = new XElement("Package");
                elem.SetAttributeValue("pid", Pid);
                var docs = await _docRepository.GetByFilter(Pid);
                foreach (var inDoc in docs)
                {
                    var docRecord = new DocRecordBase();
                    docRecord = await _docRecordRepository.GetByDocId(inDoc.DocId.ToString().Trim());
                    XElement elem_doc = XElement.Parse(docRecord.DocText);
                    elem.Add(elem_doc);
                }
                xPkg.Add(elem);
                if (xPkg == null) return string.Empty;
                else return xPkg.ToString();
            }
            catch
            {
                return string.Empty;
            }

        }
    }
}