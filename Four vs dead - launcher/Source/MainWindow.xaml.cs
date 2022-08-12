using SimpleJSON;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

/* Copyright (c) 2021 Rugbug Redfern */

namespace RedlabsUpdateUtility
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Properties to Change
		const string applicationName = "Four vs dead";
		const string fileToRun = "Four VS dead.exe";
		const string launchArgs = "";

		const string infoURL = "https://yellowsink.pl/fourvsdead/update.json"; // The URL containing the information required to check the hash, to download the update, and to send a last resort message
		
		const bool developerMode = false; // Enable developer mode in order to generate a hash file for new builds
		#endregion

		#region Runtime Variables
		string rootPath;
		string zipPath;
		string applicationFolderPath;

		string hashURL; // This file will be hosted on Github
		string downloadURL; // This file will be hosted on Google Drive
		string message; // If it breaks for some reason, and the update is no longer possible, display a last resort message to the player

		string updateHash; // The hash of the updated game, to be downloaded from the server
		string installedHash = "-1"; // The hash of the current game files, to be generated. -1 by default so that if the game is not installed it just starts installing.

		Stopwatch downloadStopwatch;
		#endregion

		public MainWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Called when the UI is rendered for the first time
		/// </summary>
		private void Window_ContentRendered(object sender, EventArgs e)
		{
			rootPath = Directory.GetCurrentDirectory(); // The path of the launcher directory
			zipPath = Path.Combine(rootPath, "Application.zip"); // The folder for the zip file that will be downloaded, extracted, then deleted
			applicationFolderPath = Path.Combine(rootPath, "Application"); // The folder for the application

			if(developerMode) // Make it clear when it's built in developer mode so that we don't accidentally distribute it
			{
				Title = $"{applicationName} Launcher (DEVELOPER MODE)";
			}
			else
			{
				Title = $"{applicationName} Launcher"; // "Launcher" sounds better than "Update Utility"
			}

			// Create the application folder because otherwise we're going to get errors later when it doesn't exist
			Directory.CreateDirectory(applicationFolderPath);

			// Check if the updater is already running, if so, close this one
			Process[] updaterProcesses = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
			if(updaterProcesses.Length > 1)
			{
				ShowError("Launcher already open");
				Close();
				return;
			}

			// Check if the game is running, if so, attempt to close it
			Process[] gameProcesses = Process.GetProcessesByName(applicationName);
			if(gameProcesses.Length > 0)
			{
				Process gameProcess = gameProcesses[0];
				TitleBlock.Text = "Closing game...";

				gameProcess.CloseMainWindow();
				for(int i = 0; i < 5; i++)
				{
					if(!gameProcess.HasExited)
					{
						gameProcess.Refresh();
						Thread.Sleep(1000);
					}
					else
					{
						break;
					}
				}

				if(!gameProcess.HasExited)
				{
					TitleBlock.Text = "Forcefully closing game...";
					gameProcess.Kill();

					for(int i = 0; i < 10; i++)
					{
						if(!gameProcess.HasExited)
						{
							gameProcess.Refresh();
							Thread.Sleep(1000);
						}
						else
						{
							break;
						}
					}

					if(!gameProcess.HasExited)
					{
						ShowError("Unable to launch game: game is already open.");
						Close();
						return;
					}
				}
			}

			// Generate a hash for the installed game files
			try
			{
				BackgroundWorker worker = new BackgroundWorker { WorkerReportsProgress = false };
				worker.DoWork += new DoWorkEventHandler(HashGenerator_DoWork);
				worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(HashGenerator_RunWorkerCompleted);

				worker.RunWorkerAsync();
			}
			catch(Exception ex)
			{
				ShowError($"Error when calculating hash: {ex}");
				PlayButton.IsEnabled = true;
				//StartApplication();
				return;
			}
		}

		/// <summary>
		/// Generate a hash for the game files
		/// </summary>
		void HashGenerator_DoWork(object sender, DoWorkEventArgs e)
		{
			installedHash = HashAlgorithmExtensions.CreateMd5ForFolder(applicationFolderPath);
		}

		/// <summary>
		/// When the files have been extracted, delete the downloaded zip folder and start the application
		/// </summary>
		void HashGenerator_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			// Save the hash to a file and close the program if we are running in developer mode
			if(developerMode)
			{
				File.WriteAllText(Path.Combine(rootPath, "hash.txt"), installedHash);
				Close();
				return;
			}

			TitleBlock.Text = "Checking for updates...";

			// Get the update info
			using(WebClient client = new WebClient())
			{
				client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore); // Disable caching

				try
				{
					// Download the string and parse it into JSON
					JSONNode updateInfo = JSONNode.Parse(client.DownloadString(infoURL.ToAntiCacheURL()));
					hashURL = updateInfo["hashURL"];
					downloadURL = updateInfo["downloadURL"];
					message = updateInfo["message"];

					// Make sure that the URLs exist
					if(string.IsNullOrEmpty(hashURL))
					{
						ShowError("Invalid hash URL.");
						PlayButton.IsEnabled = true;
						//StartApplication();
						return;
					}
					if(string.IsNullOrEmpty(downloadURL))
					{
						ShowError("Invalid download URL.");
						PlayButton.IsEnabled = true;
						//StartApplication();
						return;
					}
				}
				catch(Exception ex)
				{
					ShowError($"Error when checking for update: {ex}");
					PlayButton.IsEnabled = true;
					//StartApplication();
					return;
				}
			}

			// If there is a message, show it
			if(!string.IsNullOrEmpty(message))
			{
				MessageBox.Show(message, "Message", MessageBoxButton.OK, MessageBoxImage.Information);
			}

			// Get the update hash
			using(WebClient client = new WebClient())
			{
				client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore); // Disable caching

				try
				{
					updateHash = client.DownloadString(hashURL.ToAntiCacheURL());
				}
				catch(Exception ex)
				{
					ShowError($"Error when checking for update: {ex}");
					PlayButton.IsEnabled = true;
					//StartApplication();
					return;
				}
			}

			// Check the hashes. If they are the same, the game files have not been changed, and we can start as normal.
			if(installedHash == updateHash)
			{
				TitleBlock.Text = "No updates found. Launching application.";
				PlayButton.IsEnabled = true;
				//StartApplication();
			}
			else // They do not match, the game files have been changed, so download the new ones
			{
				// If the game is installed, we are updating it, otherwise we are downloading it for the first time.
				if(IsInstalled())
				{
					TitleBlock.Text = $"Downloading update...";
				}
				else
				{
					TitleBlock.Text = $"Downloading {applicationName}...";
				}

				// Start the download timer which will be used to predict when the download is complete
				downloadStopwatch = new Stopwatch();
				downloadStopwatch.Start();

				// Start the download
				FileDownloader fileDownloader = new FileDownloader();
				fileDownloader.DownloadProgressChanged += OnDownloadProgressChanged;
				fileDownloader.DownloadFileCompleted += OnDownloadCompleted;
				fileDownloader.DownloadFileAsync(downloadURL, zipPath);
				WindowIconTools.SetProgressState(TaskbarProgressBarState.Normal);
			}
		}

		/// <summary>
		/// Called when the download progress changes, and updates the UI accordingly.
		/// </summary>
		void OnDownloadProgressChanged(object sender, FileDownloader.DownloadProgress e)
		{
			if(downloadStopwatch.Elapsed.TotalSeconds > 0.5)
			{
				if(e.BytesReceived >= e.TotalBytesToReceive && e.TotalBytesToReceive != 0)
				{
					TitleBlock.Text = "Extracting... Please wait"; // We say please wait because it can take quite a while
				}
				else
				{
					double downloadedMB = e.BytesReceived / 1000000;
					double elapsedTime = downloadStopwatch.Elapsed.TotalSeconds;

					double downloadPerSecond = downloadedMB / elapsedTime;
					double downloadLeft = e.TotalBytesToReceive / 1000000 - downloadedMB;

					double secondsLeft = Math.Round(downloadLeft / downloadPerSecond);

					string downloadTimeLeftFormatted;

					if(secondsLeft >= 60)
					{
						double minutesLeft = Math.Round(secondsLeft / 60, MidpointRounding.AwayFromZero);
						if(minutesLeft >= 60)
						{
							double hoursLeft = Math.Round(minutesLeft / 60, MidpointRounding.AwayFromZero);
							downloadTimeLeftFormatted = string.Format("{0} hour{1} left", hoursLeft, hoursLeft == 1 ? "" : "s");
						}
						else
						{
							downloadTimeLeftFormatted = string.Format("{0} minute{1} left", minutesLeft, minutesLeft == 1 ? "" : "s");
						}
					}
					else
					{
						downloadTimeLeftFormatted = string.Format("{0} second{1} left", secondsLeft, secondsLeft == 1 ? "" : "s");
					}

					TitleBlock.Text = string.Format("Downloaded {0}/{1}mb... {2}", downloadedMB.ToString("0.0"), (e.TotalBytesToReceive / 1000000).ToString("0.0"), downloadTimeLeftFormatted);
					ProgressBar.Value = e.ProgressPercentage;
					WindowIconTools.SetProgressValue((ulong)e.ProgressPercentage, 100);
				}
			}
		}

		/// <summary>
		/// Called when the download is completed.
		/// </summary>
		void OnDownloadCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if(IsInstalled())
			{
				try
				{
					Directory.Delete(applicationFolderPath, true); // Delete the current game files and all of their subfolders
				}
				catch(UnauthorizedAccessException) // Technically we should never get this since we closed the game at the start, but if it reopened somehow, better safe than sorry.
				{
					ShowError("The game must be closed to successfully update. Please close the game and then click OK.");

					try
					{
						Directory.Delete(applicationFolderPath, true);
					}
					catch(UnauthorizedAccessException ex)
					{
						ShowError($"Unable to remove old game files: {ex}");

						Close();
						return;
					}
				}
			}

			// Extract the file in a BackgroundWorker so it doesn't stop responding
			BackgroundWorker worker = new BackgroundWorker { WorkerReportsProgress = false };
			worker.DoWork += new DoWorkEventHandler(ZipWorker_DoWork);
			worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ZipWorker_RunWorkerCompleted);

			worker.RunWorkerAsync();
		}

		/// <summary>
		/// Extract the downloaded game files
		/// </summary>
		void ZipWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			ZipFile.ExtractToDirectory(zipPath, applicationFolderPath);
		}

		/// <summary>
		/// When the files have been extracted, delete the downloaded zip folder and start the application
		/// </summary>
		void ZipWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			File.Delete(zipPath);

			WindowIconTools.SetProgressState(TaskbarProgressBarState.NoProgress);
			PlayButton.IsEnabled = true;
			
			//StartApplication();
		}

		private void PlayButton_Click(object sender, EventArgs e)
        {
			StartApplication();
        }

		/// <summary>
		/// Start the application
		/// </summary>
		void StartApplication()
		{
			ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(applicationFolderPath, fileToRun), launchArgs);
			startInfo.WorkingDirectory = applicationFolderPath;
			try
			{
				Process.Start(startInfo);
			}
			catch(Exception ex)
			{
				ShowError($"Error starting application: {ex}");
			}
			Close();
		}

		/// <summary>
		/// Show an error to the user which will require them to click OK to continue
		/// </summary>
		void ShowError(string message)
		{
			MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		/// <summary>
		/// Check if the game is installed by seeing if there are any files in the Application folder
		/// </summary>
		bool IsInstalled()
		{
			return Directory.EnumerateFileSystemEntries(applicationFolderPath).Any();
		}

        private void PlayButton_Click_1(object sender, RoutedEventArgs e)
        {
			StartApplication();
        }
    }
}
