using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Models
{
    public class PlayerContestRegistration
    {
        public Guid ContestId { get; set; }
		public Guid GameId { get; set; }
        public string PlayerName { get; set; }
    }
}
