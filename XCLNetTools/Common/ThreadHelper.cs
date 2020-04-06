using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XCLNetTools.Common
{
    /// <summary>
    /// 线程帮助类
    /// </summary>
    public static class ThreadHelper
    {
        /// <summary>
        /// 以STA方式运行任务
        /// </summary>
        public static Task StartSTATask(Action action)
        {
            var source = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {
                try
                {
                    action();
                    source.SetResult(null);
                }
                catch (Exception ex)
                {
                    source.SetException(ex);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return source.Task;
        }
    }
}