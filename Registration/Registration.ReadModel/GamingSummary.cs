using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.ReadModel
{
    public class GamingSummary
    {
        public Guid StationId { get; set; }
        public Guid? ActiveGameId { get; set; }
        public Guid? NextGameId { get; set; }
    }
}
