using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TetrisRoyal.Web.Data
{
    public static class DatabaseContextExtensions
    {

        public static void Edit<Entity>(this TetrisDatabaseContext _database,Entity entity) where Entity:class
        {
            _database.Entry(entity).State = EntityState.Modified;
        }
    }
}