using ArenaServer.Data;
using ArenaServer.Services;
using ArenaServer.Utils;
using System;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;

namespace ArenaServer.Bots
{
    public class TwitchBot
    {
        #region Fields

        //private readonly TwitchUserService userService;
        private readonly AccessService accessService;
        private static TwitchAPI api;
        private readonly AppDbContext db;

        //Additional clients
        private readonly TwitchClient twitchclient;
        //private readonly BossBot bossbot;
        //private readonly FightBot fightbot;

        //Reconnect settings
        private bool automaticreconnect = true;
        private int reconnectTries = 0;
        private int reconnectTriesMAX = 3;
        private int reconnectWaitTime = 120;

        private readonly string destinationChannelName = "Skei7";
        private readonly ChatService chatService;


        #endregion

        #region Constructor

        public TwitchBot(string _channelName)
        {
            this.db = new AppDbContextFactory().Create();
            //this.userService = new TwitchUserService();
            this.accessService = new AccessService();
            this.chatService = new ChatService(db);

            destinationChannelName = _channelName;

            //Init
            twitchclient = new TwitchClient();
            //bossbot = new BossBot(twitchclient, destinationChannelName, userService);
            //fightbot = new FightBot(twitchclient, destinationChannelName);

            //Api init
            api = new TwitchAPI();
            api.Settings.ClientId = accessService.GetTwitchClientID();
            api.Settings.AccessToken = accessService.GetTwitchAccessToken();

            Connect();
        }

        #endregion

        #region Properties



        #endregion

        #region Methods

        #region Twitch connection

        public void Connect()
        {
            //Connect
            twitchclient.Initialize(new ConnectionCredentials(destinationChannelName, accessService.GetTwitchAccessToken()), channel: destinationChannelName);
            twitchclient.Connect();

            //Events
            twitchclient.OnJoinedChannel += onJoinedChannel;
            twitchclient.OnDisconnected += onDisconnected;
            twitchclient.OnMessageReceived += onMessageReceived;
        }

        public void Disconnect()
        {
            automaticreconnect = false;
            twitchclient.Disconnect();
        }

        #endregion

        #region Twitch events

        #region Connect/Disconnect

        private void onJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            reconnectTries = 0;
            LogOutput.LogInformation("Connected to channel '" + destinationChannelName + "'.");
        }

        private void onDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            try
            {
                LogOutput.LogInformation("Disconnected from channel '" + destinationChannelName + "'.");

                //Try to reconnect
                if (automaticreconnect)
                {
                    if (reconnectTries < reconnectTriesMAX)
                    {
                        twitchclient.Reconnect();
                        reconnectTries += 1;
                    }
                    else
                    {
                        //Wait
                        LogOutput.LogWarning("Waiting " + reconnectWaitTime + " Milliseconds before the next reconnect try.");
                        System.Threading.Thread.Sleep(reconnectWaitTime);
                        reconnectTries = 1;
                        twitchclient.Reconnect();
                    }
                }
            }
            catch (Exception ex)
            {
                LogOutput.LogError("Error on automatic reconnect." + ex.Message);
            }
        }

        #endregion

        #region Message recieving

        private async void onMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var output = await chatService.HandleCommand(new Data.Common.Models.TwitchChatMessage()
            {
                Message = e.ChatMessage.Message,
                TwitchUsername = e.ChatMessage.Username,
                TwitchUserId = e.ChatMessage.UserId
            });

            if(null != output) twitchclient.SendMessage(destinationChannelName, output.ToReplyMessage());
        }

        #endregion


        #endregion

        #endregion
    }
}