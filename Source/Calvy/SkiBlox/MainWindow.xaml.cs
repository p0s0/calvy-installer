using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.Win32;

namespace SkiBlox
{
	// Token: 0x02000008 RID: 8
	public partial class MainWindow : Window
	{
		// Token: 0x06000017 RID: 23 RVA: 0x00002360 File Offset: 0x00000560
		public MainWindow()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000023B6 File Offset: 0x000005B6
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.bw.DoWork += this.Bw_DoWork;
			this.bw.RunWorkerAsync();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000023E0 File Offset: 0x000005E0
		private void changetext(string txt)
		{
			base.Dispatcher.Invoke<object>(() => this.text.Content = txt);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000241C File Offset: 0x0000061C
		private void fail(string txt)
		{
			base.Dispatcher.Invoke<object>(() => this.text.Content = txt);
			base.Dispatcher.Invoke<Brush>(() => this.prbar.Foreground = Brushes.Red);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002470 File Offset: 0x00000670
		private void Bw_DoWork(object sender, DoWorkEventArgs e)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			string text = Array.Find<string>(commandLineArgs, (string element) => element.Contains("calvy://"));
			bool flag = Array.Find<string>(commandLineArgs, (string element) => element.Contains("install")) != null;
			bool flag2 = flag;
			if (flag2)
			{
				WindowsPrincipal windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
				bool flag3 = windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
				bool flag4 = !flag3;
				if (flag4)
				{
					string location = Assembly.GetExecutingAssembly().Location;
					ProcessStartInfo processStartInfo = new ProcessStartInfo();
					processStartInfo.Verb = "runas";
					processStartInfo.FileName = location;
					processStartInfo.Arguments = "install";
					try
					{
						Process.Start(processStartInfo);
					}
					catch (Win32Exception)
					{
						MessageBox.Show("Error: admin access denied");
						Environment.Exit(0);
					}
					Environment.Exit(0);
					return;
				}
				bool flag5 = !Process.GetCurrentProcess().MainModule.FileName.Contains("Installer.exe");
				if (flag5)
				{
					try
					{
						Directory.Delete("C:\\Program Files (x86)\\calvy", true);
					}
					catch (Exception ex)
					{
					}
					this.changetext("Creating directories...");
					Directory.CreateDirectory("C:\\Program Files (x86)\\calvy");
					while (!Directory.Exists("C:\\Program Files (x86)\\calvy"))
					{
					}
					File.Copy(Assembly.GetExecutingAssembly().Location, "C:\\Program Files (x86)\\calvy\\Installer.exe");
					string fileName = "C:\\Program Files (x86)\\calvy\\Installer.exe";
					ProcessStartInfo processStartInfo2 = new ProcessStartInfo();
					processStartInfo2.Verb = "runas";
					processStartInfo2.FileName = fileName;
					processStartInfo2.Arguments = "install";
					try
					{
						Process.Start(processStartInfo2);
					}
					catch (Win32Exception)
					{
						MessageBox.Show("Error: admin access denied");
						Environment.Exit(0);
					}
					Environment.Exit(0);
				}
				this.changetext("Installing...");
				this.failed = true;
				base.Dispatcher.Invoke<bool>(() => this.prbar.IsIndeterminate = false);
				try
				{
					Directory.Delete("C:\\Program Files (x86)\\calvy", true);
				}
				catch (Exception ex2)
				{
				}
				this.changetext("Creating directories...");
				Directory.CreateDirectory("C:\\Program Files (x86)\\calvy");
				this.changetext("Downloading files...");
				while (!Directory.Exists("C:\\Program Files (x86)\\calvy"))
				{
				}
				WebClient webClient = new WebClient();
				webClient.DownloadProgressChanged += this.Wc_DownloadProgressChanged;
				webClient.DownloadFileCompleted += this.Wc_DownloadFileCompleted;
				webClient.DownloadFileAsync(new Uri("http://calvyy.xyz/api/download/calvy.zip"), "C:\\Program Files (x86)\\calvy\\calvy.zip");
			}
			bool flag6 = text == null && !flag;
			if (flag6)
			{
				MessageBoxResult messageBoxResult = MessageBox.Show("Would you like to install Calvy?", "Calvy", MessageBoxButton.YesNo, MessageBoxImage.Question);
				bool flag7 = messageBoxResult == MessageBoxResult.Yes;
				if (flag7)
				{
					string location2 = Assembly.GetExecutingAssembly().Location;
					ProcessStartInfo processStartInfo3 = new ProcessStartInfo();
					processStartInfo3.Verb = "runas";
					processStartInfo3.FileName = location2;
					processStartInfo3.Arguments = "install";
					try
					{
						Process.Start(processStartInfo3);
					}
					catch (Win32Exception)
					{
						MessageBox.Show("Error: admin access denied");
						Environment.Exit(0);
					}
					Environment.Exit(0);
				}
				else
				{
					Environment.Exit(0);
				}
			}
			bool flag8 = text != null;
			if (flag8)
			{
				try
				{
					string text2 = HttpUtility.UrlDecode(text).Replace("calvy://", "").Replace("/", "");
					string[] array = text2.Split("|".ToCharArray());
					this.gameid = array[1];
					this.ticket = array[0];
				}
				catch (Exception ex3)
				{
					MessageBox.Show(ex3.Message);
				}
				bool flag9 = !this.failed;
				if (flag9)
				{
					this.changetext("Checking version...");
					bool flag10 = !Functions.checkversion();
					if (flag10)
					{
						this.failed = true;
						this.fail("Old laucnher");
						string location3 = Assembly.GetExecutingAssembly().Location;
						ProcessStartInfo processStartInfo4 = new ProcessStartInfo();
						processStartInfo4.Verb = "runas";
						processStartInfo4.FileName = location3;
						processStartInfo4.Arguments = "install";
						try
						{
							Process.Start(processStartInfo4);
						}
						catch (Win32Exception)
						{
							MessageBox.Show("Error: admin access denied");
							Environment.Exit(0);
						}
						Environment.Exit(0);
					}
				}
				bool flag11 = !this.failed;
				if (flag11)
				{
					this.changetext("Authenticating...");
					bool flag12 = !Functions.attemptlogin(this.ticket);
					if (flag12)
					{
						this.failed = true;
						this.fail("Invalid account");
					}
				}
				bool flag13 = !this.failed;
				if (flag13)
				{
					this.changetext("Waiting for an available server");
					WebClient webClient2 = new WebClient();
					string text3 = webClient2.DownloadString("http://calvyy.xyz/api/launcher/requestserver.php?id=" + this.gameid);
					bool flag14 = text3 == "false";
					if (flag14)
					{
						try
						{
							this.changetext("Starting server...");
							string a = webClient2.DownloadString("http://calvyy.xyz/api/game/server.php?ticket=" + this.ticket + "&gameid=" + this.gameid);
							bool flag15 = a == "ratelimited";
							if (flag15)
							{
								MessageBox.Show("Ratelimited. Try later.", "Calvy", MessageBoxButton.OK, MessageBoxImage.Hand);
								Environment.Exit(0);
							}
							while (webClient2.DownloadString("http://calvyy.xyz/api/launcher/requestserver.php?id=" + this.gameid) == "false")
							{
							}
							Globals.port = webClient2.DownloadString("http://calvyy.xyz/api/launcher/requestserver.php?id=" + this.gameid);
						}
						catch (Exception ex4)
						{
							MessageBox.Show(ex4.Message);
						}
					}
					else
					{
						this.changetext("Connecting... ");
						Globals.port = text3;
					}
				}
				bool flag16 = !this.failed;
				if (flag16)
				{
					this.changetext("Attempting to launch SkiBlox");
					try
					{
						new Process
						{
							StartInfo = 
							{
								FileName = "C:\\Program Files (x86)\\calvy\\CalvyPlayer.exe",
								UseShellExecute = true,
								WorkingDirectory = "C:\\Program Files (x86)\\calvy",
								Arguments = string.Concat(new string[]
								{
									"-a \"http://www.servtop.tk/\" -j \"http://servtop.tk/Game/join.php?placeid=0&gameid=",
									this.gameid,
									"&ticket=",
									this.ticket,
									"\" -t \"",
									this.ticket,
									"\""
								})
							}
						}.Start();
						Environment.Exit(0);
					}
					catch (Exception ex5)
					{
						MessageBox.Show(ex5.Message);
					}
				}
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002B74 File Offset: 0x00000D74
		private void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			Functions.UnZip("C:\\Program Files (x86)\\calvy\\calvy.zip", "C:\\Program Files (x86)\\calvy");
			File.Delete("C:\\Program Files (x86)\\calvy\\calvy.zip");
			this.changetext("Registering URL");
			Registry.ClassesRoot.CreateSubKey("calvy");
			Registry.ClassesRoot.CreateSubKey("calvy\\DefaultIcon");
			Registry.ClassesRoot.CreateSubKey("calvy\\shell");
			Registry.ClassesRoot.CreateSubKey("calvy\\shell\\open");
			Registry.ClassesRoot.CreateSubKey("calvy\\shell\\open\\command");
			Registry.SetValue("HKEY_CLASSES_ROOT\\calvy", "URL Protocol", "", RegistryValueKind.String);
			Registry.SetValue("HKEY_CLASSES_ROOT\\calvy\\shell\\open\\command", "", "\"C:\\Program Files (x86)\\calvy\\Calvy.exe\" \"%1\"", RegistryValueKind.String);
			MessageBox.Show("Successfully installed Calvy!");
			Environment.Exit(0);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002C38 File Offset: 0x00000E38
		private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			base.Dispatcher.Invoke<double>(() => this.prbar.Value = (double)e.ProgressPercentage);
		}

		// Token: 0x04000010 RID: 16
		private BackgroundWorker bw = new BackgroundWorker();

		// Token: 0x04000011 RID: 17
		private BackgroundWorker bw2 = new BackgroundWorker();

		// Token: 0x04000012 RID: 18
		private bool downloaded = false;

		// Token: 0x04000013 RID: 19
		private bool failed = false;

		// Token: 0x04000014 RID: 20
		private string ticket = "1";

		// Token: 0x04000015 RID: 21
		private string gameid = "1";
	}
}
