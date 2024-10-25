using System;
using System.Xml.Linq;

namespace ClientSWH.Application.CollectingListToXml.HendlersElem
{
    public interface IHandlerElem
    {
        void ProcessQueue(ref XElement elem);

    }

}