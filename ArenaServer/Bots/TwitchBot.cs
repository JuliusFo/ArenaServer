﻿using ArenaServer.Data;
using ArenaServer.Data.Common.Models;
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

        private readonly UserService userService;
        private readonly AccessService accessService;
        private readonly BossService bossService;
        private readonly PokemonService pokemonService;
        private readonly UserfightService userfightService;
        private readonly AchievementService achievementService;
        private readonly SettingsService settingsService;
        private readonly ChatOutputService chatOutputService;
        private readonly ChatInputService chatService;

        private static TwitchAPI api;
        private readonly AppDbContext db;
        private readonly TwitchClient twitchclient;

        private readonly string destinationChannelName = "Skei7";

        #endregion

        #region Constructor

        public TwitchBot(string _channelName)
        {
            //Init Client
            twitchclient = new TwitchClient();

            //Init Database
            this.db = new AppDbContextFactory().Create();

            //Init access
            accessService = new AccessService();

            //Init Twitch API
            api = new TwitchAPI();
            api.Settings.ClientId = accessService.GetTwitchClientID();
            api.Settings.AccessToken = accessService.GetTwitchAccessToken();

            //Init services
            this.achievementService = new AchievementService(db);
            this.settingsService = new SettingsService(db);
            this.pokemonService = new PokemonService(db);

            userService = new UserService(db, api);
            
            chatOutputService = new ChatOutputService(twitchclient, _channelName);
            bossService = new BossService(userService, pokemonService, chatOutputService, settingsService);
            userfightService = new UserfightService(userService, chatOutputService);
            

            chatService = new ChatInputService(userService, chatOutputService, bossService, userfightService, achievementService);
            destinationChannelName = _channelName;

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
            twitchclient.OnJoinedChannel += OnJoinedChannel;
            twitchclient.OnDisconnected += OnDisconnected;
            twitchclient.OnMessageReceived += OnMessageReceived;
        }

        public void Disconnect()
        {
            twitchclient.Disconnect();
        }

        #endregion

        #region Twitch events

        #region Connect/Disconnect

        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            LogOutput.LogInformation("Connected to channel '" + destinationChannelName + "'.");
        }

        private void OnDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            Environment.Exit(-1);
        }

        #endregion

        #region Message recieving

        private async void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var output = await chatService.HandleCommand(new TwitchChatMessage()
            {
                Message = e.ChatMessage.Message,
                TwitchUsername = e.ChatMessage.DisplayName,
                TwitchUserId = e.ChatMessage.UserId
            });

            if(null != output) twitchclient.SendMessage(destinationChannelName, output.ToReplyMessage());
        }

        #endregion


        #endregion

        #endregion
    }
}