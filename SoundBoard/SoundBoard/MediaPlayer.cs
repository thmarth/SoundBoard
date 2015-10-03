using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundBoard
{
	class MediaPlayer
	{
		Dictionary<String, System.Windows.Media.MediaPlayer> mediaPlayers = new Dictionary<string, System.Windows.Media.MediaPlayer>();

		public MediaPlayer(Settings settings)
		{
			for (int i = 1; i <= settings.GetInt("General", "Rows"); i++)
			{
				for (int j = 1; j <= settings.GetInt("General", "Columns"); j++)
				{
					mediaPlayers.Add("Button" + i + j, new System.Windows.Media.MediaPlayer());
					Load("Button" + i + j, settings.GetString("Button" + i + j, "File"));
				}
			}
		}

		public void Stop()
		{
			foreach (KeyValuePair<String, System.Windows.Media.MediaPlayer> entry in mediaPlayers)
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
					System.Windows.MessageBox.Show("Error loading " + path + " for " + button, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
				}
			}
		}

		public void Play(String button)
		{
			mediaPlayers[button].Play();
		}
	}
}
