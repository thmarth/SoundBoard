using SharpConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SoundBoard {
    /// <summary>
    /// Interaction logic for ProfileEdit.xaml
    /// </summary>
    public partial class ProfileEdit : Window
    {
        private Settings settings;
        private List<String> profiles;

        public ProfileEdit(Settings settings)
        {
            InitializeComponent();
            this.settings = settings;
            this.profiles = settings.GetProfiles().ToList<String>();
            PopulateList();
        }

        private void PopulateList()
        {
            listView.Items.Clear();
            foreach (string profile in profiles)
                listView.Items.Add(profile);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            settings.SetString("Profile", "Profiles", String.Format("{{{0}}}", String.Join(", ", profiles)));
            this.Close();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            InputForm input = new InputForm();
            input.Owner = this;
            input.Title = "New Profile";
            input.ShowDialog();
            if (!input.GetInput().Equals(""))
                profiles.Add(input.GetInput());
            PopulateList();
        }

        private void buttonRename_Click(object sender, RoutedEventArgs e)
        {
            InputForm input = new InputForm();
            input.Owner = this;
            input.Title = "Rename";
            input.ShowDialog();
            if (!input.GetInput().Equals(""))
                profiles[listView.SelectedIndex] = input.GetInput();
            PopulateList();
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (profiles.Count == 1)
                return;
            RemoveProfileButtons(listView.SelectedIndex);
            profiles.RemoveAt(listView.SelectedIndex);
            PopulateList();
        }

        private void RemoveProfileButtons(int index)
        {
            for (int i = 1; i <= settings.GetInt("General", "Rows"); i++)
                for (int j = 1; j <= settings.GetInt("General", "Columns"); j++)
                    settings.RemoveSection("Button" + i + j + "P" + index);

            for (int i = index; i < profiles.Count; i++)
                for (int j = 1; j <= settings.GetInt("General", "Rows"); j++)
                    for (int k = 1; k <= settings.GetInt("General", "Columns"); k++)
                        settings.RenameSection("Button" + j + k + "P" + (i + 1), "Button" + j + k + "P" + i);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            buttonClose_Click(sender, new RoutedEventArgs());
        }
    }
}
