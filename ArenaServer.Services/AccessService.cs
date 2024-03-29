﻿using System.IO;
using System.Text;

namespace ArenaServer.Services
{
	public class AccessService
	{
		#region Fields

		private readonly string path = "E:/Programmierung/Temp/Connections.ini";

		#endregion

		#region Constructor

		public AccessService()
		{

		}

		#endregion

		#region Properties



		#endregion

		#region Methods

		public string GetTwitchClientID()
		{
			var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			string text;

			using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
			{
				text = streamReader.ReadToEnd();
			}

			return text.Split('#')[0];
		}

		public string GetTwitchAccessToken()
		{
			var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			string text;

			using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
			{
				text = streamReader.ReadToEnd();
			}

			return text.Split('#')[1];
		}

		public string GetTwitchOAuth()
        {
			var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			string text;

			using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
			{
				text = streamReader.ReadToEnd();
			}

			return text.Split('#')[3];
		}

		#endregion
	}
}