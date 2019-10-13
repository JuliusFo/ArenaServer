using TwitchLib.Client;

namespace ArenaServer.Services
{
    public class ChatOutputService
    {
        #region Fields

        private readonly TwitchClient twitchClient;
        private readonly string twitchDestinationChannel;

        #endregion

        #region Constructor

        public ChatOutputService(TwitchClient twitchClient, string twitchDestinationChannel)
        {
            this.twitchClient = twitchClient;
            this.twitchDestinationChannel = twitchDestinationChannel;
        }

        #endregion

        #region Properties



        #endregion

        #region Methods

        public void SendMessage(string message)
        {
            twitchClient.SendMessage(twitchDestinationChannel, message);
        }

        #endregion
    }
}