using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ArenaServer.Services
{
	public class BuildService
	{
		#region Fields

		private readonly string path = "F:/Temp/Build.txt";
		private readonly string major_version = "0";
		private readonly string minor_version = "1";
		private readonly string revision = "0";
		private string build = "";
		private readonly string informational_version = " Alpha";

		#endregion

		#region Constructor

		public BuildService()
		{

		}

		#endregion

		#region Properties



		#endregion

		#region Methods

		public void IncreaseBuildVersion()
		{
			var fileStreamRead = new FileStream(path, FileMode.Open, FileAccess.Read);
			int buildNumber;

			using (var streamReader = new StreamReader(fileStreamRead, Encoding.UTF8))
			{
				build = streamReader.ReadToEnd();
			}

			fileStreamRead.Dispose();

			var fileStreamWrite = new FileStream(path, FileMode.Open, FileAccess.Write);

			using (var streamWriter = new StreamWriter(fileStreamWrite, Encoding.UTF8))
			{
				buildNumber = int.Parse(build);
				buildNumber += 1;
				streamWriter.WriteLine(buildNumber);
			}

			fileStreamWrite.Dispose();
		}

		public string GetBuildVersion()
		{
			var fileStreamRead = new FileStream(path, FileMode.Open, FileAccess.Read);
			int buildNumber;

			using (var streamReader = new StreamReader(fileStreamRead, Encoding.UTF8))
			{
				build = streamReader.ReadToEnd();
				buildNumber = int.Parse(build);
			}

			return major_version + "." + minor_version + "." + revision + "." + build + informational_version;
		}

		#endregion
	}
}