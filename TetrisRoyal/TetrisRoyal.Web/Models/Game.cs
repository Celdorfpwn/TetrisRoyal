﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetrisRoyal.Web.Models
{
    public class Game
    {
        public Guid Id { get; set; }

        public string HostId { get; set; }

        public virtual Player Host { get; set; }

        public string ChallengerId { get; set; }

        public virtual Player Challenger { get; set; }
    }
}