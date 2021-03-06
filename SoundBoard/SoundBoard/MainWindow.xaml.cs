﻿using Microsoft.Win32;
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
			for (int i = 1; i <= settings.GetInt("General", "Rows"); i++)
				for (int j = 1; j <= settings.GetInt("General", "Columns"); j++)
					UpdateButton("Button" + i + j);
		}

		private void UpdateButton(String button)
		{
			Button control = (Button)this.FindName(button);
			control.Background = settings.GetButtonBrush(button, "Background");
			control.Foreground = settings.GetButtonBrush(button, "Foreground");
			control.Content = settings.GetString(button, "Text");
			control.ToolTip = settings.GetString(button, "File");
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;

			if (checkBox.IsChecked == true)
			{
				ButtonEdit editor = new ButtonEdit(settings, button.Name);
				editor.Owner = this;
				if (editor.ShowDialog() == true)
				{
					if (editor.backgroundTypeColor.IsChecked == true)
						settings.SetString(button.Name, "Background", editor.backgroundColor.SelectedColor.ToString());
					else if (editor.backgroundTypeImage.IsChecked == true)
						settings.SetString(button.Name, "Background", editor.backgroundUri.Text);

					settings.SetString(button.Name, "Foreground", editor.foregroundColor.SelectedColor.ToString());
					settings.SetString(button.Name, "Text", editor.foregroundText.Text);
					settings.SetString(button.Name, "File", editor.musicfileUri.Text);
					mediaPlayer.Load(button.Name, editor.musicfileUri.Text);

					settings.Save();
					UpdateButton(button.Name);
				}
			}
			else
			{
				mediaPlayer.Play(button.Name);
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
	}
}
