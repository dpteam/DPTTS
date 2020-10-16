using System;
using VkNet;
using VkNet.Model.RequestParams;

namespace DPTTS
{
    class MessagesManager
    {
        public static VkApi api = new VkApi();

        public void SendMessage(string message, long? userID)
        {
            Random rnd = new Random();
            api.Messages.Send(new MessagesSendParams
            {
                RandomId = rnd.Next(),
                UserId = userID,
                Message = message
            });

        }
    }
}
