namespace ArenaServer.Data.Common.Models
{
    public static class TwitchChatResponse
    {
        public static string REG_ERROR_NOTVALIDMSG = "Deine Registrierung enthält einen Fehler. Bitte überprüfe deine Nachricht auf eine korrekte Schreibweise.";
        public static string REG_SUCCESS = "Deine Registrierung war erfolgreich!";
        public static string REG_ERROR_UNKNOWN = "Bei deiner Registrierung ist ein unbekannter Fehler aufgetreten.";
        public static string REG_ALREADY_REG = "Du bist bereits registriert.";

        public static string GET_STATE_ONLINE = "Der ArenaBot ist online!";
        public static string GET_ARENA_INFO = "In der Poke-Arena kannst du Pokemon fangen und gegeneinander kämpfen. Registriere dich mit !registrieren Pikachu/Glumanda/Schiggy/Bisasam/Evoli oder auf arena.catmozo.de";

        public static string UNKNOWN_COMMAND = "Dieser Befehl ist unbekannt. Verfügbare Befehle: !Commands/!Befehle";
    }
}