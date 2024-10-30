
using ClientSWH.Application.Common;
using ClientSWH.Application.Interfaces;
using ClientSWH.Core.Abstraction.Repositories;

using ClientSWH.Core.Models;
using ClientSWH.DocsRecordCore.Abstraction;
using ClientSWH.DocsRecordCore.Models;
using System.Data;

using System.Xml.Linq;



namespace ClienSWH.XMLParser
{
    public class LoadFromFile(IPackagesRepository pkgRepository,
        IDocumentsRepository docRepository, IDocRecordRepository docRecordRepository) : ILoadFromFile
    {
        private readonly IPackagesRepository _pkgRepository = pkgRepository;
        private readonly IDocumentsRepository _docRepository = docRepository;
        private readonly IDocRecordRepository _docRecordRepository = docRecordRepository;


        public async Task<string> LoadFileXml(Guid userId, string inFile)
        {
            int Pid = _pkgRepository.GetLastPkgId().Result + 1;
            int resDoc = 0;
            int cXmlDoc = 0;
            try
            {
                XDocument xFile = XDocument.Parse(inFile.Trim());

                var xPkg = xFile.Element("Package")?
                    .Elements().Where(p => p.Attributes().Where(a => a.Name.LocalName.Contains("CfgName")).FirstOrDefault()?.Value is not null);
                
                if (xPkg is not null)
                {
                    //create package
                    var Pkg = Package.Create(Pid, userId, 0, Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow);
                    Pkg = await _pkgRepository.Add(Pkg);
                    Pid = Pkg.Id;
                    cXmlDoc = xPkg.Count();
                    // var resProc = new TasksHandlerDoc(_docRecordContext, _docRepository,xPkg,Pid);
                    // var resDoc= resProc.ProcessQueueDoc();
                    foreach (var xDoc in xPkg)
                    {
                        var LastDocId = _docRepository.GetLastDocId().Result + 1;

                        var tdoc = xDoc.Name.LocalName;
                        var num = xDoc.Elements().Elements("RegNum").FirstOrDefault()?.Value.ToString();
                        var dat = xDoc.Elements().Elements("RegDate").FirstOrDefault()?.Value.ToString();
                        var doctext = xDoc.ToString();
                        string docCode = tdoc.Contains("CONOSAMENT") ? "02011" : "09999";
                        DateTime DocDate = DateTime.Now.Date;
                        if (dat is not null)
                            _ = DateTime.TryParse(dat, out DocDate);

                        var Doc = Document.Create(LastDocId, Guid.NewGuid(), num is not null ? num : string.Empty, DocDate.Date, docCode,
                                      tdoc, doctext.Length, DopFunction.GetHashMd5(doctext),
                                      DopFunction.GetSha256(doctext),
                                      Pid, DateTime.UtcNow, DateTime.UtcNow);


                        Doc = await _docRepository.Add(Doc);
                        if (Doc is not null)
                        {
                            
                            var dRecordId = await _docRecordRepository.AddRecord(Doc.DocId.ToString(), doctext);
                            if (dRecordId.Trim()!=string.Empty) resDoc++;
                        }
                    }
                    var hPkg = HistoryPkg.Create(Pid, 0, 0, "Load Package", DateTime.UtcNow);
                    
                    
                }
            }
            catch (Exception ex)
            {
                string mess = ex.Message;

            }

            return $"Создан пакет {Pid}, В исходном xml документов{cXmlDoc}, загружено {resDoc}";
        }

    }
}
