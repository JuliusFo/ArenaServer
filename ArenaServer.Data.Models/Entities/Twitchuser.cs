﻿using System;
using System.Collections.Generic;

namespace ArenaServer.Data.Models
{
	public class Twitchuser
	{
		#region Properties

		public string Twitchuser_Id { get; set; }

		public string DisplayName { get; set; }

		public bool Kz_Log_Enabled { get; set; }

		public DateTimeOffset? Dt_Last_Userfight { get; set; }

		public virtual IEnumerable<CatchedPokemon> CatchedPokemon { get; set; } = new List<CatchedPokemon>();

		public virtual IEnumerable<Achievements> Achievements { get; set; } = new List<Achievements>();

		#endregion
	}
}