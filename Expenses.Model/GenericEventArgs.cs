using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Expenses.Model
{
    public class EventArgs<T> : EventArgs
    {
        public T Data { get; set; }

        public EventArgs(T data)
        {
            this.Data = data;
        }

    }
}
