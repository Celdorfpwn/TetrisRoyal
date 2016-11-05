using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TetrisRoyal.Web.Models;

namespace TetrisRoyal.Web.Data
{
    public class PlayerMap : EntityTypeConfiguration<Player>
    {
        public PlayerMap()
        {
            HasKey(player => player.ConnectionId);
        }
    }
}