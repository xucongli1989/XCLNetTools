using System;
using System.Collections.Generic;

namespace XCLNetTools.Generic
{
    /// <summary>
    /// 带有 Dispose 的 List
    /// </summary>
    public class DisposableList<T> : List<T>, IDisposable where T : IDisposable
    {
        /// <summary>
        /// 释放内存
        /// </summary>
        public void Dispose()
        {
            this.ForEach(x =>
            {
                try
                {
                    x.Dispose();
                }
                catch
                {
                    //
                }
            });
        }
    }
}