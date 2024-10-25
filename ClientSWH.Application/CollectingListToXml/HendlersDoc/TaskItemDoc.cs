using ClientSWH.Core.Models;
using ClientSWH.DocsRecordCore.Abstraction;
using System.Xml.Linq;

namespace ClientSWH.Application.CollectingListToXml.Hendlers
{
    public class TaskItemDoc : TaskItemBase
    {

        public TaskItemDoc(XElement inDocx)
        {
            TaskElem = inDocx;
        }
    }
}
