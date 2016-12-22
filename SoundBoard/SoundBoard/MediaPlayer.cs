using NAudio.Wave;
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
		private Dictionary<string, string> paths = new Dictionary<string, string>();
        private Dictionary<string, WaveOut> players = new Dictionary<string, WaveOut>();
        private int audioDevice;

        public MusicPlayer(Settings settings)
		{
			for (int i = 1; i <= settings.GetInt("General", "Rows"); i++)
			{
				for (int j = 1; j <= settings.GetInt("General", "Columns"); j++)
				{
                    String button = "Button" + i + j + "P" + settings.GetString("Profile", "Profile");
                    paths.Add(button, settings.GetString(button, "File"));
				}
			}
            SetOutputDevice(settings.GetString("Audio", "DeviceName"));
		}

		public void Stop()
		{
			foreach (KeyValuePair<string, WaveOut> entry in players)
			{
				entry.Value.Stop();
			}
            players.Clear();
		}

        private void Stop(object sender, StoppedEventArgs e)
        {
            Stop();
        }

		public void Play(string button)
		{
			if (paths[button] != "" && !players.ContainsKey(button))
			{
				try
				{
                    WaveOut player = new WaveOut();
                    player.DeviceNumber = audioDevice;
                    player.Init(new AudioFileReader(paths[button]));
                    player.PlaybackStopped += Stop;
                    player.Play();
                    players.Add(button, player);
                }
				catch (Exception)
				{
					MessageBox.Show("Error loading " + paths[button] + " for " + button, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

        internal bool SetOutputDevice(string name)
        {
            Stop();
            int index = -1;
            for (int i = 0; i < WaveOut.DeviceCount; i++)
                if (WaveOut.GetCapabilities(i).ProductName.Equals(name))
                    index = i;
            audioDevice = index;
            return index >= 0;
        }
    }
}
