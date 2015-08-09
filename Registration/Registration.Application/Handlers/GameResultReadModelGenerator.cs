using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Registration.Domain.Contest;
using Registration.ReadModel;
using Registration.ReadModel.Implementation;

namespace Registration.Application.Handlers
{
    public class GameResultReadModelGenerator
        : Consumes<SinglePlayerGamePlaced>.All
        , Consumes<TeamGamePlaced>.All
        , Consumes<PlayerScored>.All
        , Consumes<PlayerScoreUpdated>.All
    {
        private readonly Func<RegistrationDbContext> _contextFactory;
        private readonly IServiceBus _eventBus;

        public GameResultReadModelGenerator(Func<RegistrationDbContext> contextFactory, IServiceBus eventBus)
        {
            _contextFactory = contextFactory;
            _eventBus = eventBus;
        }

        public void Consume(SinglePlayerGamePlaced message)
        {
            if (message == null) throw new ArgumentNullException("message");

            using (var context = _contextFactory.Invoke())
            {
                var dto = context.Find<GameResult>(message.SourceId);
                if (dto == null)
                {
                    dto = new GameResult
                    {
                        GameId = message.SourceId
                    };
                }

                context.Save(dto);
            }
        }



        public void Consume(TeamGamePlaced message)
        {
            if (message == null) throw new ArgumentNullException("message");

            using (var context = _contextFactory.Invoke())
            {
                var dto = context.Find<GameResult>(message.SourceId);
                if (dto == null)
                {
                    dto = new GameResult
                    {
                        GameId = message.SourceId
                    };
                }


                context.Save(dto);
            }
        }

        public void Consume(PlayerScored message)
        {
            if (message == null) throw new ArgumentNullException("message");

            using (var context = _contextFactory.Invoke())
            {
                var dto = context.Find<GameResult>(message.SourceId);

                if (dto == null)
                {
                    dto = new GameResult
                    {
                        GameId = message.SourceId
                    };
                }

                var score = dto.Scores.FirstOrDefault(x => x.ShotNumber == message.ShotNumber);
                if (score != null)
                {
                    score.Points = message.Points;
                }
                else
                {
                    dto.Scores.Add(new Shot
                    {
                        ShotNumber = message.ShotNumber,
                        Points = message.Points
                    });
                }

                dto.TotalScore = dto.Scores.Sum(x => x.Points);

                context.Save(dto);
            }
        }

        public void Consume(PlayerScoreUpdated message)
        {
            if (message == null) throw new ArgumentNullException("message");

            using (var context = _contextFactory.Invoke())
            {
                var dto = context.Find<GameResult>(message.SourceId);

                if (dto == null)
                {
                    dto = new GameResult
                    {
                        GameId = message.SourceId
                    };
                }

                var score = dto.Scores.FirstOrDefault(x => x.ShotNumber == message.ShotNumber);
                if (score != null)
                {
                    score.Points = message.Points;
                }
                else
                {
                    dto.Scores.Add(new Shot
                    {
                        ShotNumber = message.ShotNumber,
                        Points = message.Points
                    });
                }

                dto.TotalScore = dto.Scores.Sum(x => x.Points);

                context.Save(dto);
            }
        }

    }
}
