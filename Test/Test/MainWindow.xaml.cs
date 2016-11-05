using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        const string server = "http://localhost:49167/";

        //"http://tetrisroyal.azurewebsites.net/";

        HubConnection _hub = new HubConnection(server + "game");

        IHubProxy _proxy;

        ObservableCollection<Game> _games = new ObservableCollection<Game>();

        Game _currentGame;

        public MainWindow()
        {
            InitializeComponent();

            gamesListView.ItemsSource = _games;

            _proxy = _hub.CreateHubProxy("game");

            _proxy.On("Login", async () =>
             {
                 MessageBox.Show("Login");

                 await _proxy.Invoke("RequestGamesList");

                 Dispatcher.Invoke(() =>
                 {
                     loginButton.IsEnabled = false;
                     createGameButton.IsEnabled = true;
                 });
             });

            _proxy.On("ReceiveGames", (IEnumerable<Game> result) =>
             {
                 Dispatcher.Invoke(() =>
                 {
                     foreach (var game in result)
                     {
                         _games.Add(game);
                     }
                 });
             });

            _proxy.On("AddGame", (Game game) =>
            {
                Dispatcher.Invoke(() =>
                {
                    _games.Add(game);
                });
            });

            _proxy.On("LoginFailed", () =>
            {
                MessageBox.Show("Failed");
            });

            _proxy.On("GameStarted", (Guid gameId) =>
             {
                 Dispatcher.Invoke(() =>
                 {
                     var game = _games.FirstOrDefault(g => g.Id == gameId);
                     if (game != null)
                     {
                         _games.Remove(game);
                     }
                 });
             });

            _proxy.On("StartGame", (Guid gameId) =>
             {
                 Dispatcher.Invoke(() =>
                 {
                     _currentGame = _games.FirstOrDefault(g => g.Id == gameId);
                     MessageBox.Show("game started " + _currentGame.Id);
                     _games.Remove(_currentGame);
                 });
             });

        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            if (_hub.ConnectionId == null)
            {
                await _hub.Start();
            }

            await _proxy.Invoke("Login", usernameTextBox.Text);
        }

        private async void createGameButton_Click(object sender, RoutedEventArgs e)
        {
            await _proxy.Invoke("CreateGame");
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_hub.ConnectionId != null)
            {
                _hub.Stop();
            }
            base.OnClosed(e);
        }

        private async void joinButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var game = button.DataContext as Game;

            await _proxy.Invoke("JoinGame", game.Id);
        }
    }
}
