using ISPCore.Models.RequestsFilter.Monitoring;
using System;
using System.Runtime.CompilerServices;

namespace ISPCore.Models.Triggers.Events.core
{
    public class CheckRequest
    {
        public static Action<(string IP, TypeRequest type, ulong CountRequest, string host, int DomainID)> OnRequestToMinute => (s) => RequestToMinute?.Invoke(null, s);
        public static event EventHandler<ITuple> RequestToMinute;
    }
}
