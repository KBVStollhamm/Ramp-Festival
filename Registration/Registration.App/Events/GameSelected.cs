using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.PubSubEvents;
using Registration.ReadModel;

namespace Registration.Events
{
    public class GameSelected : PubSubEvent<SequencingItem>
    {
    }
}
