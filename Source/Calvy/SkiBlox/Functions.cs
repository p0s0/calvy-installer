using System;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace SkiBlox
{
	internal static class Functions
	{
		public static string id(string user)
		{
			return Functions.wc.DownloadString(api.id + "?user=" + user);
		}
		public static string RandomString(int length)
		{
			return new string((from s in Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
			select s[Functions.random.Next(s.Length)]).ToArray<char>());
		}
		public static bool checkversion()
		{
			string value = Functions.wc.DownloadString(api.getversion);
			return Convert.ToInt64(value) <= Convert.ToInt64(Globals.version);
		}
		public static string getusername(string ticket)
		{
			return Functions.wc.DownloadString(api.getuser + "?ticket=" + ticket);
		}
		public static bool attemptlogin(string ticket)
		{
			string a = Functions.wc.DownloadString("http://calvyy.xyz/api/launcher/login.php?ticket=" + ticket);
			return a == "true";
		}
		public static void UnZip(string file, string result)
		{
			ZipFile.ExtractToDirectory(file, result);
		}

		private static WebClient wc = new WebClient();
		private static Random random = new Random();
	}
}
