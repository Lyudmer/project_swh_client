﻿using ClientSWH.Core.Models;

using System.Xml.Linq;

namespace ClientSWH.Application.CollectingListToXml
{
    public class TaskItemBase
    {
        public Task Task { get; set; }
        public Document TaskDocs { get; set; }
        public XElement TaskElem { get; set; }
    }
}