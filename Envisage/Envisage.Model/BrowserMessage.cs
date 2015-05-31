using System;

namespace Envisage.Model
{
    public class BrowserMessage : Notification<String>
    {
        public String Name { get; set; }
        public String Colour { get; set; }
        public String State { get; set; }
    }
}