using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using D = Engine.BL.Delegates;

namespace Engine.Services
{
    public class ExceptionManager : IDisposable
    {
        public static D.CallbackExceptionMsg? CallbackException { get; } = Emit;
        private static List<ExceptionManager?> Subscribers { get; set; } = new ();

        private static void Emit(Exception ex, string msg)
        {
            foreach (var d in Subscribers)
                d?.Subscription?.Invoke(ex, msg);
        }

        public void Dispose()
        {
            lock(Subscribers)
            {
                int index = Subscribers.FindIndex(x => x?.KeyTime == KeyTime);

                if (index != -1)
                    Subscribers.RemoveAt(index);
            }
        }
        
        public D.CallbackExceptionMsg? Subscription { get; }
        private decimal KeyTime { get; }
        public ExceptionManager(D.CallbackExceptionMsg? subCallback) { 
            Subscription = subCallback;
            KeyTime = decimal.Parse(DateTime.Now.ToString("MddyyyymmHHmmss"));
            Subscribers.Add(this);
        }
        
    }
}
