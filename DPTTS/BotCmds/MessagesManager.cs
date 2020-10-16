using System;
using VkNet;
using VkNet.Model.RequestParams;

namespace DPTTS.BotCmds
{
    class MessagesManager
    {
        public static VkApi api = new VkApi();

        public static void SendMessage(string message, long? peerID)
        {
            Random rnd = new Random();
            api.Messages.Send(new MessagesSendParams
            {
                RandomId = rnd.Next(),
                PeerId = peerID,
                Message = message
            });

        }
    }
}
