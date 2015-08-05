using Registration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Services
{
    public interface IRegistrationService
    {
        Task Submit(PlayerContestRegistration registration);
    }
}
