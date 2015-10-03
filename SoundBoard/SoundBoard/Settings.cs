﻿using SharpConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			Load(filename);
		}

		public void Load(String filename)
		{
			this.filename = filename;
			if (File.Exists(this.filename))
				cfg = Configuration.LoadFromFile(this.filename);
			else
				cfg = new Configuration();

			VerifySettingsFile(ref cfg);
			Save();
		}

		public void Save()
		{
			Save(filename);
		}

		public void Save(String filename)
		{
			this.filename = filename;
			cfg.SaveToFile(this.filename);
		}

		private void VerifySettingsFile(ref Configuration cfg)
		{
			if (!cfg.Contains("General"))
				cfg.Add(new Section("General"));

			cfg["General"] = VerifyGeneral(cfg["General"]);

			for (int i = 1; i <= cfg["General"]["Rows"].IntValue; i++)
			{
				for (int j = 1; j <= cfg["General"]["Columns"].IntValue; j++)
				{
					String buttonName = "Button" + i + j;
                    if (!cfg.Contains(buttonName))
						cfg.Add(new Section(buttonName));

					cfg[buttonName] = VerifyGenericButton(cfg[buttonName]);
				}
			}
		}

		private Section VerifyGeneral(Section section)
		{
			if (!section.Contains("Rows"))
				section.Add(new Setting("Rows", "3"));

			if (!section.Contains("Columns"))
				section.Add(new Setting("Columns", "3"));

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