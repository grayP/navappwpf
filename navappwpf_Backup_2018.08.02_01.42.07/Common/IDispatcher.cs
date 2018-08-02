using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace navappwpf.Common
{
    public interface IDispatcher
    {
        void Invoke(Action action);
        void BeginInvoke(Action action);
    }

    public class DispatcherWrapper : IDispatcher
    {
        Dispatcher dispatcher;

        public DispatcherWrapper()
        {
            dispatcher = Application.Current.Dispatcher;

        }
        public void Invoke(Action action)
        {
            dispatcher.Invoke(action);
        }
        public void BeginInvoke(Action action)

        {
            dispatcher.BeginInvoke(action);
        }
    }
}
