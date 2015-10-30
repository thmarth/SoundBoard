using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SoundBoard
{
	class MusicPlayer
	{
		Dictionary<String, MediaPlayer> mediaPlayers = new Dictionary<string, MediaPlayer>();

		public MusicPlayer(Settings settings)
		{
			for (int i = 1; i <= settings.GetInt("General", "Rows"); i++)
			{
				for (int j = 1; j <= settings.GetInt("General", "Columns"); j++)
				{
					mediaPlayers.Add("Button" + i + j, new MediaPlayer());
					Load("Button" + i + j, settings.GetString("Button" + i + j, "File"));
				}
			}
		}

		public void Stop()
		{
			foreach (KeyValuePair<String, MediaPlayer> entry in mediaPlayers)
			{
				entry.Value.Stop();
			}
		}

		public void Load(String button, String path)
		{
			if (path != "")
			{
				try
				{
					mediaPlayers[button].Open(new Uri(path));
				}
				catch (UriFormatException)
				{
					MessageBox.Show("Error loading " + path + " for " + button, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		public void Play(String button)
		{
			mediaPlayers[button].Play();
		}
	}
}
