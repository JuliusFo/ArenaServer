using ArenaServer.Services;
using System;
using System.Collections.Generic;
using System.Text;
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

        //Bot data
        private const string name = "ArenaBot";
        private const string version = "Version 0.1.2 Alpha";

        #endregion

        #region Constructor

        public TwitchBot(string _channelName)
        {
            //this.userService = new TwitchUserService();
            this.accessService = new AccessService();

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
            Console.WriteLine("Connected to channel '" + destinationChannelName + "'.");
        }

        private void onDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            try
            {
                Console.WriteLine("Disconnected from channel '" + destinationChannelName + "'.");

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
                        Console.WriteLine("Waiting " + reconnectWaitTime + " Milliseconds before the next reconnect try.");
                        System.Threading.Thread.Sleep(reconnectWaitTime);
                        reconnectTries = 1;
                        twitchclient.Reconnect();
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Error on automatic reconnect.");
            }
        }

        #endregion

        #region Message recieving

        private void onMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.Equals("!arena_version"))
            {
                twitchclient.SendMessage(destinationChannelName, version);
            }
        }

        #endregion


            #endregion

            #endregion
        }
}