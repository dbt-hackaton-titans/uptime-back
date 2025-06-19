using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titans.Uptime.Domain
{
    public enum CheckType
    {
        Ping,
        Https
    }

    public enum CheckStatus
    {
        Unknown,
        Up,
        Down
    }

    public enum ResponseStringType
    {
        Contains,
        NotContains
    }

    public enum EventType
    {
        Down,
        Up
    }

    public enum EventCategory
    {
        Internal,
        External
    }

    public enum MaintenanceType
    {
        Planned,
        Emergency
    }
}
