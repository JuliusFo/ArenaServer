using ArenaServer.Data.Common.Models;
using ArenaServer.Data.Transfer;
using ArenaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaServer.Services.UserFightService
{
    public static class UserfightCheckService
    {
        #region Fields



        #endregion


        #region Constructor



        #endregion

        #region Properties



        #endregion

        #region Methods

        public static TwitchChatReplyMessage CheckUsersExisting(TransferTwitchuser attacker, string attackerName, TransferTwitchuser defender, string defenderName)
        {
            string missingUserName = string.Empty;
            bool checkFailed = false;

            if(null == attacker)
            {
                missingUserName = attackerName;
                checkFailed = true;
            }

            if(null == defender)
            {
                checkFailed = true;
                missingUserName = defenderName;
            }

            if (checkFailed)
            {
                LogOutput.LogInformation($"[Userfight] User {missingUserName} not registered.");
                return new TwitchChatReplyMessage(missingUserName, "Da ist etwas schiefgelaufen - Du hast die Registrierung noch nicht abgeschlossen.");
            }

            return null;
        }

        public static TwitchChatReplyMessage CheckUserHasTeam(TransferTwitchuser attacker, TransferTwitchuser defender,  bool teamFight)
        {
            string failedUserName = string.Empty;
            bool checkFailed = false;

            if (teamFight)
            {
                if (!attacker.HasFullSelectedFightingTeam())
                {
                    checkFailed = true;
                    failedUserName = attacker.DisplayName;
                }

                if (!defender.HasFullSelectedFightingTeam())
                {
                    checkFailed = true;
                    failedUserName = defender.DisplayName;
                }

                if (checkFailed)
                {
                    LogOutput.LogInformation($"[Userfight (Selected)] User {failedUserName} does not has a full selected fighting team.");
                    return new TwitchChatReplyMessage(failedUserName, "Da ist etwas schiefgelaufen - Du hast dir noch kein Kampf-Team zusammengestellt (6 ausgewählte gefangene Pokemon).");
                }
            }
            else
            {
                if (!attacker.HasFullFightingTeam())
                {
                    checkFailed = true;
                    failedUserName = attacker.DisplayName;
                }

                if (!defender.HasFullFightingTeam())
                {
                    checkFailed = true;
                    failedUserName = defender.DisplayName;
                }

                if (checkFailed)
                {
                    LogOutput.LogInformation($"[Userfight (Random)] User {failedUserName} does not has a full fighting team.");
                    return new TwitchChatReplyMessage(failedUserName, "Da ist etwas schiefgelaufen - Du hast kein vollständiges Team (6 gefangene Pokemon).");
                }
            }

            return null;
        }

        public static TwitchChatReplyMessage CheckUserTimeout(TransferTwitchuser attacker, TransferTwitchuser defender, TimeSpan timeoutSpan)
        {
            string failedUserName = string.Empty;
            bool checkFailed = false;
            TimeSpan? cooldownSpan = null;

            if(attacker.LastUserFight != null && timeoutSpan > (DateTime.Now - attacker.LastUserFight))
            {
                checkFailed = true;
                failedUserName = attacker.DisplayName;
                cooldownSpan = timeoutSpan - (DateTime.Now - attacker.LastUserFight);
            }

            if (defender.LastUserFight != null && timeoutSpan > (DateTime.Now - defender.LastUserFight))
            {
                checkFailed = true;
                failedUserName = defender.DisplayName;
                cooldownSpan = timeoutSpan - (DateTime.Now - defender.LastUserFight);
            }

            if (checkFailed)
            {
                LogOutput.LogInformation($"[Userfight] User {failedUserName} is on timeout.");
                return new TwitchChatReplyMessage(failedUserName, $"Da ist etwas schiefgelaufen - Du musst noch {cooldownSpan.Value.TotalSeconds} Sekunden warten, bis du wieder kämpfen kannst.");
            }

            return null;
        }

        #endregion
    }
}