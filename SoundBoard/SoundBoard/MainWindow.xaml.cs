using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SoundBoard
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Settings settings;
		private MusicPlayer mediaPlayer;

		public MainWindow()
		{
			InitializeComponent();

			settings = new Settings();
			mediaPlayer = new MusicPlayer(settings);

			WindowUpdate();

			UpdateComponents();
		}

		private void WindowUpdate()
		{
			if (settings.GetDouble("Screen", "Height") != 0)
				this.Height = settings.GetDouble("Screen", "Height");

			if (settings.GetDouble("Screen", "Width") != 0)
				this.Width = settings.GetDouble("Screen", "Width");

			if (settings.GetDouble("Screen", "Top") != 0)
				this.Top = settings.GetDouble("Screen", "Top");

			if (settings.GetDouble("Screen", "Left") != 0)
				this.Left = settings.GetDouble("Screen", "Left");
		}

		private void UpdateComponents()
		{
            InitializeButtons();

            InitializeProfileDropdown();
		}

        private void InitializeProfileDropdown()
        {
            comboBoxProfile.Items.Clear();
            foreach (String profile in settings.GetProfiles())
                comboBoxProfile.Items.Add(profile);
            comboBoxProfile.Items.Add("<Edit...>");
            comboBoxProfile.SelectedIndex = settings.GetInt("Profile", "Profile");
        }

        private void InitializeButtons()
        {
            for (int i = 1; i <= settings.GetInt("General", "Rows"); i++)
                for (int j = 1; j <= settings.GetInt("General", "Columns"); j++)
                    UpdateButton("Button" + i + j);
        }

        private void UpdateButton(String button)
		{
			Button control = (Button)this.FindName(button);
            String buttonWithProfile = button + "P" + settings.GetString("Profile", "Profile");
            control.Background = settings.GetButtonBrush(buttonWithProfile, "Background");
			control.Foreground = settings.GetButtonBrush(buttonWithProfile, "Foreground");
			control.Content = settings.GetString(buttonWithProfile, "Text");
			control.ToolTip = settings.GetString(buttonWithProfile, "File");
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
            String buttonName = button.Name + "P" + settings.GetString("Profile", "Profile");

            if (checkBox.IsChecked == true)
			{
                ButtonEdit editor = new ButtonEdit(settings, buttonName);
				editor.Owner = this;
				if (editor.ShowDialog() == true)
				{
					if (editor.backgroundTypeColor.IsChecked == true)
						settings.SetString(buttonName, "Background", editor.backgroundColor.SelectedColor.ToString());
					else if (editor.backgroundTypeImage.IsChecked == true)
						settings.SetString(buttonName, "Background", editor.backgroundUri.Text);

					settings.SetString(buttonName, "Foreground", editor.foregroundColor.SelectedColor.ToString());
					settings.SetString(buttonName, "Text", editor.foregroundText.Text);
					settings.SetString(buttonName, "File", editor.musicfileUri.Text);
					mediaPlayer.Load(buttonName, editor.musicfileUri.Text);

					settings.Save();
					UpdateButton(button.Name);
				}
			}
			else
			{
				mediaPlayer.Play(buttonName);
			}
		}

		private void Stop_Click(object sender, RoutedEventArgs e)
		{
			mediaPlayer.Stop();
		}

		private void Fade_Click(object sender, RoutedEventArgs e)
		{
			new AboutBox().ShowDialog();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			settings.SetDouble("Screen", "Height", this.Height);
			settings.SetDouble("Screen", "Width", this.Width);
			settings.SetDouble("Screen", "Top", this.Top);
			settings.SetDouble("Screen", "Left", this.Left);
			settings.Save();
		}

        private void SetProfile(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex == settings.GetProfiles().Length)
            {
                ProfileEdit profileEditor = new ProfileEdit(settings);
                profileEditor.Owner = this;
                profileEditor.ShowDialog();
                settings.SetString("Profile", "Profile", 0.ToString());
                InitializeProfileDropdown();
                settings.Save();
                settings = new Settings();
            }
            else
            {
                settings.SetString("Profile", "Profile", (sender as ComboBox).SelectedIndex.ToString());
            }
            InitializeButtons();
            mediaPlayer = new MusicPlayer(settings);
        }
    }
}
