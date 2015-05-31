using System;

namespace Envisage.Model
{
    public class Notification<T>
    {
        public String MessageName { get; set; }
        public T Data { get; set; }
        public String Callback { get; set; }
    }
}
