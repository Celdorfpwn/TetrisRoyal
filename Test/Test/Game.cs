using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Game
    {
        public Guid Id { get; set; }

        public string HostId { get; set; }

        public string HostName { get; set; }

        public bool CanJoin { get; set; }
    }
}
