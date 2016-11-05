using Microsoft.AspNet.SignalR.Client;
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

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        const string server= "http://tetrisroyal.azurewebsites.net/";

        HubConnection _hub = new HubConnection(server + "game");

        IHubProxy _proxy;

        public MainWindow()
        {
            InitializeComponent();
            _proxy = _hub.CreateHubProxy("game");

            _proxy.On("Login", () =>
            {
                MessageBox.Show("Login");
            });

            _proxy.On("LoginFailed", () =>
            {
                MessageBox.Show("Failed");
            });
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            if (_hub.ConnectionId == null)
            {
                await _hub.Start();

                await _proxy.Invoke("Login", textBox.Text);
            }
        }
    }
}
