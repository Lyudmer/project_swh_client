﻿

using ClientSWH.Application.Common;
using ClientSWH.Core.Abstraction.Repositories;

using ClientSWH.Core.Models;
using ClientSWH.DocsRecordCore.Abstraction;

using System.Xml.Linq;


namespace ClientSWH.Application.CollectingListToXml.Hendlers
{
    public class TasksHandlerDoc(IDocRecordRepository docRecordRepository, IDocumentsRepository docRepository,
                                 IEnumerable<XElement> inDocs,int Pid) : IHandlerDoc
    {
        private readonly List<TaskItemDoc> _queueTasksDoc = [];
        private readonly IDocRecordRepository _docRecordRepository = docRecordRepository;
        private readonly IDocumentsRepository _docRepository = docRepository;
        public int ProcessQueueDoc()
        {
            
            foreach (var item in inDocs)
            {

                var taskitem = new TaskItemDoc(item);
                _queueTasksDoc.Add(taskitem);
            }
            _queueTasksDoc.ForEach(RunTask);

            return WaitAllTasks();

        }

        private int WaitAllTasks()
        {
            var tasksForWait = GetTasksForWait();
            Task.WaitAll(tasksForWait);
            int recDocCount = 0;
            foreach (var result in _queueTasksDoc)
            {
               if( result is not null) recDocCount++;
            }
            return recDocCount;
        }

        private Task[] GetTasksForWait()
        {
            return _queueTasksDoc
               .Select(t => t.Task)
               .ToArray();
        }

        private void RunTask(TaskItemDoc item)
        {
            item.Task = Task.Run(async () => item.TaskDocs = await CreateElemXmlFromDoc(item.TaskElem));
        }

        

        public async Task<Document> CreateElemXmlFromDoc(XElement xDoc)
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
                
            var Doc = Document.Create(LastDocId, Guid.NewGuid(), (num is not null) ? num : string.Empty, DocDate.Date, docCode,
                          tdoc, doctext.Length, DopFunction.GetHashMd5(doctext),
                          DopFunction.GetSha256(doctext),
                          Pid, DateTime.UtcNow, DateTime.UtcNow);


            Doc = await _docRepository.Add(Doc);
            if (Doc is not null)
            {
                var dRecordId = await _docRecordRepository.AddRecord(Doc.DocId.ToString(), doctext);
                if (dRecordId is not null) return Doc;
            }

            return null;
        }


    }


}