using MassTransit;
using Registration.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Server
{
    public class CommandProcessor
        : Consumes<RegisterPlayerToContest>.All
        
    {
        Guid _id;
        public CommandProcessor()
        {
            _id = Guid.NewGuid();
            Console.WriteLine("CommandProcessor with Id: {0}", _id);
        }

        public void Consume(RegisterPlayerToContest message)
        {
            Console.WriteLine("Register player to conference: {0} on CommandProcessor with Id: {1}", message.PlayerName, _id);
        }
    }
}
