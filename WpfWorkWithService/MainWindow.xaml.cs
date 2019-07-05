using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
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

namespace WpfWorkWithService
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly FriendsService friendsService = new FriendsService();
        readonly StringBuilder stringBuilder = new StringBuilder();

        public MainWindow()
        {
            InitializeComponent();
            ButtonAddData.IsEnabled = false;
            ButtonDeleteData.IsEnabled = false;
        }

        bool IsConnected()
        {
            stringBuilder.Clear();

            if (!CheckingConnection.IsConnectedToInternet(TextBoxUrl.Text))
            {
                TextBox.Text = "Error locating web service";
                ButtonAddData.IsEnabled = false;
                ButtonDeleteData.IsEnabled = false;
                return false;
            }

            ButtonAddData.IsEnabled = true;
            ButtonDeleteData.IsEnabled = true;

            return true;
        }

        private async void ButtonGetData_OnClick(object sender, RoutedEventArgs e)
        {
            if (!IsConnected()) return;

            friendsService.Url = TextBoxUrl.Text;
            
            IEnumerable<Friend> friends = await friendsService.Get();

            foreach (Friend f in friends)
                stringBuilder.AppendLine($"Id: {f.Id}\nName: {f.Name}\nEmail: {f.Email}\nPhone: {f.Phone}\n");

            TextBox.Text = stringBuilder.ToString();

            
        }

        private async void ButtonAddData_OnClick(object sender, RoutedEventArgs e)
        {
            if (!IsConnected()) return;

            friendsService.Url = TextBoxUrl.Text;

            Friend friend = new Friend();
            friend.Name = TextBoxName.Text;
            friend.Email = TextBoxEmail.Text;
            friend.Phone = TextBoxPhone.Text;

            await friendsService.Add(friend);
        }

        private async void ButtonDeleteData_OnClick(object sender, RoutedEventArgs e)
        {
            if (!IsConnected()) return;

            friendsService.Url = TextBoxUrl.Text;

            await friendsService.Delete(int.Parse(TextBoxId.Text));
        }
    }
}
