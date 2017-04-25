using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BackChannelEventTest2
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Event &&
                string.Equals(activity.Name, "buttonClicked", StringComparison.InvariantCultureIgnoreCase))
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                // return our reply to the user
                Activity reply = activity.CreateReply("I see that you just pushed that button");
                await connector.Conversations.ReplyToActivityAsync(reply);
            }

            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                // return our reply to the user
                var reply = activity.CreateReply();
                reply.Type = ActivityTypes.Event;
                reply.Name = "changeBackground";
                reply.Value = activity.Text;
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                await HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                if (message.MembersAdded.Any(o => o.Id == message.Recipient.Id))
                {
                    ConnectorClient client = new ConnectorClient(new Uri(message.ServiceUrl));

                    var reply = message.CreateReply();

                    reply.Text = "Welcome to the bot!";

                    await client.Conversations.ReplyToActivityAsync(reply);
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }
        }
        ///// <summary>
        ///// POST: api/Messages
        ///// Receive a message from a user and reply to it
        ///// </summary>
        //public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        //{
        //    if (activity.Type == ActivityTypes.Message)
        //    {
        //        ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
        //        // calculate something for us to return
        //        int length = (activity.Text ?? string.Empty).Length;

        //        // return our reply to the user
        //        Activity reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
        //        await connector.Conversations.ReplyToActivityAsync(reply);
        //    }
        //    else
        //    {
        //        HandleSystemMessage(activity);
        //    }
        //    var response = Request.CreateResponse(HttpStatusCode.OK);
        //    return response;
        //}

        //private Activity HandleSystemMessage(Activity message)
        //{
        //    if (message.Type == ActivityTypes.DeleteUserData)
        //    {
        //        // Implement user deletion here
        //        // If we handle user deletion, return a real message
        //    }
        //    else if (message.Type == ActivityTypes.ConversationUpdate)
        //    {
        //        // Handle conversation state changes, like members being added and removed
        //        // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
        //        // Not available in all channels
        //    }
        //    else if (message.Type == ActivityTypes.ContactRelationUpdate)
        //    {
        //        // Handle add/remove from contact lists
        //        // Activity.From + Activity.Action represent what happened
        //    }
        //    else if (message.Type == ActivityTypes.Typing)
        //    {
        //        // Handle knowing tha the user is typing
        //    }
        //    else if (message.Type == ActivityTypes.Ping)
        //    {
        //    }

        //    return null;
        //}
    }
}