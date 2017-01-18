using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearby.Interfaces
{
    public interface IReminderService
    {
        void AddEvent(DateTime startTime, DateTime endTime, string title, string location, string message, Action<bool> callback, string i);
    }
}
