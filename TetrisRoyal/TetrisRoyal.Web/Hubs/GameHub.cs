using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TetrisRoyal.Web.Data;
using TetrisRoyal.Web.Models;

namespace TetrisRoyal.Web.Hubs
{
    [HubName("game")]
    public class GameHub : Hub
    {
        private readonly TetrisDatabaseContext _database;

        public GameHub()
        {
            _database = new TetrisDatabaseContext();
        }


        public async Task CreateGame()
        {
            var player = await _database.Set<Player>().FindAsync(Context.ConnectionId);

            var game = new Game
            {
                Id = Guid.NewGuid(),
                HostId = player.ConnectionId
            };

            _database.Set<Game>().Add(game);

            await _database.SaveChangesAsync();

            await Clients.All.AddGame(new
            {
                Id = game.Id,
                HostId = game.HostId,
                HostName = player.Username
            });
        }

        public async Task Login(string username)
        {
            var existing = _database.Set<Player>().FirstOrDefault(player => player.Username == username);

            if (existing != null)
            {
                await Clients.Client(Context.ConnectionId).LoginFailed();

            }
            else
            {
                var player = await _database.Set<Player>().FindAsync(Context.ConnectionId);

                player.Username = username;

                _database.Entry(player).State = EntityState.Modified;

                await _database.SaveChangesAsync();

                await Clients.Client(Context.ConnectionId).Login();
            }
        }

        public async override Task OnConnected()
        {
            _database.Set<Player>().Add(new Player
            {
                ConnectionId = Context.ConnectionId
            });

            await _database.SaveChangesAsync();
            await base.OnConnected();
        }



        public async override Task OnDisconnected(bool stopCalled)
        {
            var player = await _database.Set<Player>().FindAsync(Context.ConnectionId);


            foreach (var game in _database.Set<Game>().Where(g => g.HostId == player.ConnectionId))
            {
                _database.Set<Game>().Remove(game);
                await Clients.Client(game.ChallengerId).PlayerQuit(new
                {
                    GameId = game.Id
                });
            }

            foreach (var game in _database.Set<Game>().Where(g => g.ChallengerId == player.ConnectionId))
            {
                _database.Set<Game>().Remove(game);
                await Clients.Client(game.HostId).PlayerQuit(new
                {
                    GameId = game.Id
                });
            }

            _database.Set<Player>().Remove(player);

            await _database.SaveChangesAsync();

            await base.OnDisconnected(stopCalled);
        }
    }
}