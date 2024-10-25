using ClientSWH.Core.Models;


namespace ClientSWH.Application.CollectingListToXml.HendlersElem
{
    public class TaskItemElem : TaskItemBase
    {
        public TaskItemElem(Document inDocx)
        {
            TaskDocs = inDocx;
        }
    }
}