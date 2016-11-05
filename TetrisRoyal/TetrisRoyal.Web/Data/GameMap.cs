using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TetrisRoyal.Web.Models;

namespace TetrisRoyal.Web.Data
{
    public class GameMap : EntityTypeConfiguration<Game>
    {
        public GameMap()
        {
            HasKey(game => game.Id);

            HasRequired(game => game.Host).WithMany().HasForeignKey(game => game.HostId);

            HasOptional(game => game.Challenger).WithMany().HasForeignKey(game => game.ChallengerId);
        }
    }
}