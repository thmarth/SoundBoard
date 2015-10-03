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
using System.Windows.Shapes;

namespace SoundBoard
{
	/// <summary>
	/// Interaction logic for ButtonEdit.xaml
	/// </summary>
	public partial class ButtonEdit : Window
	{
		public ButtonEdit(Settings settings, String target)
		{
			InitializeComponent();

			Brush backgroundBrush = settings.GetButtonBrush(target, "Background");
			if (backgroundBrush.GetType() == typeof(ImageBrush))
			{
				backgroundTypeImage.IsChecked = true;
				backgroundUri.Text = settings.GetString(target, "Background");
			}
			else if (backgroundBrush.GetType() == typeof(SolidColorBrush))
			{
				backgroundTypeColor.IsChecked = true;
				backgroundColor.SelectedColor = settings.GetColor(target, "Background");
			}

			foregroundColor.SelectedColor = settings.GetColor(target, "Foreground");
			foregroundText.Text = settings.GetString(target, "Text");
		}

		private void backgroundImage_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png|All files (*.*)|*.*";
			if (dialog.ShowDialog() == true)
				backgroundUri.Text = dialog.FileName;
		}

		private void musicfile_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
			if (dialog.ShowDialog() == true)
				musicfileUri.Text = dialog.FileName;
		}

		private void buttonOK_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}

		private void buttonCancel_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}
	}
}
