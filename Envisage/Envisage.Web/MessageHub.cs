using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Envisage.Web
{
    [HubName("messageHub")]
    public class MessageHub : Hub
    {
        public void Send(String methodName, String name, String colour, String state)
        {
            Clients.All.broadcastMessage(methodName, name, colour, state);
        }
    }
}