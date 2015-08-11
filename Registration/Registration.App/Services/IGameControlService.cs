using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Services
{
    public interface IGameControlService
    {
        Task StartSinglePlayerGame(Guid gameId);
        Task MakePlayerShot(Guid gameId, string playerName, int shotNumber, int scores);

        Task StartTeamGame(Guid gameId, string playerName);
        Task MakeTeamPlayerShot(Guid gameId, string playerName, int shotNumber, int scores);
    }
}
