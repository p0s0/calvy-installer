using System;

namespace SkiBlox
{
	internal class api
	{
		public static string apihost = Globals.host + "/api";
		public static string getuser = api.apihost + "/getuser.php";
		public static string id = api.apihost + "/getid.php";
		public static string getversion = "http://calvyy.xyz/api/download/version.txt";
		public static string login = api.apihost + "/login.php";
	}
}
