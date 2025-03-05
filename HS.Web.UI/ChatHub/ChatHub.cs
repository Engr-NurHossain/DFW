using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace HS.Web.UI.ChatHub
{ 
    public class ChatHub : Hub
    {
        public void Send(string another,string name, string message)
        {
            if(HttpContext.Current != null && HttpContext.Current.User != null)
            {
                name = HttpContext.Current.User.Identity.Name;
            }
            // Call the broadcastMessage method to update clients.
            
            Clients.All.broadcastMessage(name, message);
        }
        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;
            Groups.Add(Context.ConnectionId, name);

            return base.OnConnected();
        }
    }

}