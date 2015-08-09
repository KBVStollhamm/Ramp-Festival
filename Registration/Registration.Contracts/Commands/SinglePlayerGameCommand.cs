using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Messaging;

namespace Registration.Commands
{
    public abstract class SinglePlayerGameCommand : ICommand
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
    }
}
