using SharpConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SoundBoard
{
	public class Settings
	{
		private Configuration cfg;
		private String filename;

		public Settings()
		{
			Configuration.IgnoreInlineComments = true;
			Load("soundboard.cfg");
		}

		public Settings(String filename)
		{
			Configuration.IgnoreInlineComments = true;
			Load(filename);
		}

		public void Load()
		{
			if (File.Exists(filename))
			{
				try
				{
					cfg = Configuration.LoadFromFile(filename);
				}
				catch (ParserException)
				{
					MessageBoxResult result = MessageBox.Show("An error occured during loading of the configuration.\nDo you want to generate a new blank configuration?", "Configuration Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
					if (result == MessageBoxResult.OK)
						cfg = new Configuration();
					else
						Environment.Exit(1);
				}
			}
			else
				cfg = new Configuration();

			VerifyConfiguration();
			Save();
		}

		public void Load(String filename)
		{
			this.filename = filename;
			Load();
		}

		public void Save()
		{
			cfg.SaveToFile(filename);
		}

		public void Save(String filename)
		{
			this.filename = filename;
			Save();
		}

		private void VerifyConfiguration()
		{
			if (!cfg.Contains("General"))
				cfg.Add(new Section("General"));
			cfg["General"] = VerifyGeneral(cfg["General"]);

			if (!cfg.Contains("Screen"))
				cfg.Add(new Section("Screen"));
			cfg["Screen"] = VerifyScreen(cfg["Screen"]);

            if (!cfg.Contains("Profile"))
                cfg.Add(new Section("Profile"));
            cfg["Profile"] = VerifyProfile(cfg["Profile"]);

            for (int i = 0; i < GetProfiles().Length; i++)
            {
                for (int j = 1; j <= cfg["General"]["Rows"].IntValue; j++)
                {
                    for (int k = 1; k <= cfg["General"]["Columns"].IntValue; k++)
                    {
                        String buttonName = "Button" + j + k + "P" + i;
                        if (!cfg.Contains(buttonName))
                            cfg.Add(new Section(buttonName));

                        cfg[buttonName] = VerifyGenericButton(cfg[buttonName]);
                    }
                }
            }
		}

        internal String[] GetProfiles()
        {
            return cfg["Profile"]["Profiles"].GetValueArray<String>();
        }

        private Section VerifyProfile(Section section)
        {
            if (!section.Contains("Profile"))
                section.Add(new Setting("Profile", "0"));

            if (!section.Contains("Profiles"))
                section.Add(new Setting("Profiles", String.Format("{{{0}}}", String.Join(", ", new String[] { "Default" }))));

            return section;
        }

        private Section VerifyGeneral(Section section)
		{
			if (!section.Contains("Rows"))
				section.Add(new Setting("Rows", "3"));

			if (!section.Contains("Columns"))
				section.Add(new Setting("Columns", "3"));

            return section;
		}

		private Section VerifyScreen(Section section)
		{
			if (!section.Contains("Height"))
				section.Add(new Setting("Height", "0"));

			if (!section.Contains("Width"))
				section.Add(new Setting("Width", "0"));

			if (!section.Contains("Top"))
				section.Add(new Setting("Top", "0"));

			if (!section.Contains("Left"))
				section.Add(new Setting("Left", "0"));

			if (section["Height"].DoubleValue != 0 && section["Height"].DoubleValue > SystemParameters.VirtualScreenHeight)
				section["Height"].DoubleValue = SystemParameters.VirtualScreenHeight;

			if (section["Width"].DoubleValue != 0 && section["Width"].DoubleValue > SystemParameters.VirtualScreenWidth)
				section["Width"].DoubleValue = SystemParameters.VirtualScreenWidth;

			if (section["Top"].DoubleValue != 0 && ((section["Top"].DoubleValue + section["Height"].DoubleValue) / 2) > (SystemParameters.VirtualScreenHeight - section["Height"].DoubleValue))
				section["Top"].DoubleValue = (SystemParameters.VirtualScreenHeight - section["Height"].DoubleValue);

			if (section["Left"].DoubleValue != 0 && ((section["Left"].DoubleValue + section["Width"].DoubleValue) / 2) > (SystemParameters.VirtualScreenWidth - section["Width"].DoubleValue))
				section["Left"].DoubleValue = (SystemParameters.VirtualScreenWidth - section["Width"].DoubleValue);

			if (section["Top"].DoubleValue < 0)
				section["Top"].DoubleValue = 1;

			if (section["Left"].DoubleValue < 0)
				section["Left"].DoubleValue = 1;

			return section;
		}

		private Section VerifyGenericButton(Section section)
		{
			if (!section.Contains("Background"))
				section.Add(new Setting("Background", "#D3D3D3"));

			if (!section.Contains("Foreground"))
				section.Add(new Setting("Foreground", "#000000"));

			if (!section.Contains("Text"))
				section.Add(new Setting("Text", "DefaultText"));

			if (!section.Contains("File"))
				section.Add(new Setting("File", ""));

			return section;
		}

		public Brush GetButtonBrush(String section, String key)
		{
			String value = cfg[section][key].StringValue;

			if (value.StartsWith("#"))
				return new SolidColorBrush((Color)ColorConverter.ConvertFromString(cfg[section][key].StringValue));
			else
			{
				ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(value)));
				brush.Stretch = Stretch.Uniform;

				return brush;
			}
		}

		public Color GetColor(String section, String key)
		{
			return (Color)ColorConverter.ConvertFromString(cfg[section][key].StringValue);
        }

		public int GetInt(String section, String key)
		{
			return cfg[section][key].IntValue;
		}

		public void SetInt(String section, String key, int value)
		{
			cfg[section][key].IntValue = value;
		}

		public double GetDouble(String section, String key)
		{
			return cfg[section][key].DoubleValue;
		}

		public void SetDouble(String section, String key, double value)
		{
			cfg[section][key].DoubleValue = value;
		}

		public String GetString(String section, String key)
		{
			return cfg[section][key].StringValue;
		}

		public void SetString(String section, String key, String value)
		{
			cfg[section][key].StringValue = value;
		}
	}
}
