using System;
using MediatR;

namespace NSE.Core.Messages
{
    public class Event : Message, INotification
    {
        public Event()
        {
            Timestamp = DateTime.Now;
            ;
        }

        public DateTime Timestamp { get; }
    }
}