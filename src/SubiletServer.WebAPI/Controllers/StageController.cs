using MediatR;
using Microsoft.AspNetCore.Mvc;
using SubiletServer.Application.StageEvents.Commands;
using SubiletServer.Application.StageEvents.Queries;
using SubiletServer.Domain.Entities;

namespace SubiletServer.WebAPI.Controllers
{
    [ApiController]
    [Route("sahne")]
    public class StageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStageEvents()
        {
            var query = new GetStageEventsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("tiyatro")]
        public async Task<IActionResult> GetTheatreEvents()
        {
            var query = new GetStageEventsQuery { Genre = StageGenre.Theatre };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("dans")]
        public async Task<IActionResult> GetDanceEvents()
        {
            var query = new GetStageEventsQuery { Genre = StageGenre.Dance };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("opera")]
        public async Task<IActionResult> GetOperaEvents()
        {
            var query = new GetStageEventsQuery { Genre = StageGenre.Opera };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStageEvent([FromBody] CreateStageEventCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAllStageEvents), new { id = result }, result);
        }

        [HttpPost("seed")]
        public async Task<IActionResult> SeedStageEvents()
        {
            var events = new List<CreateStageEventCommand>
            {
                // Tiyatro Etkinlikleri
                new CreateStageEventCommand
                {
                    Title = "Romeo ve Juliet",
                    Description = "Shakespeare'in ölümsüz eseri Romeo ve Juliet, modern bir yorumla sahnede!",
                    Date = new DateTime(2025, 8, 15),
                    Location = "İstanbul Şehir Tiyatrosu",
                    Price = 350,
                    Capacity = 200,
                    ImageUrl = "/images/romeo-juliet.jpg",
                    Genre = StageGenre.Theatre
                },
                new CreateStageEventCommand
                {
                    Title = "Hamlet",
                    Description = "Shakespeare'in en büyük trajedisi Hamlet, unutulmaz bir performansla!",
                    Date = new DateTime(2025, 9, 5),
                    Location = "Ankara Devlet Tiyatrosu",
                    Price = 300,
                    Capacity = 150,
                    ImageUrl = "/images/hamlet.jpg",
                    Genre = StageGenre.Theatre
                },
                new CreateStageEventCommand
                {
                    Title = "Kral Lear",
                    Description = "Kral Lear'ın trajik hikayesi, güçlü oyunculukla sahnede!",
                    Date = new DateTime(2025, 9, 20),
                    Location = "İzmir Devlet Tiyatrosu",
                    Price = 280,
                    Capacity = 120,
                    ImageUrl = "/images/kral-lear.jpg",
                    Genre = StageGenre.Theatre
                },

                // Dans Etkinlikleri
                new CreateStageEventCommand
                {
                    Title = "Kuğu Gölü",
                    Description = "Klasik bale eseri Kuğu Gölü, büyüleyici dans performansıyla!",
                    Date = new DateTime(2025, 8, 25),
                    Location = "İstanbul Zorlu PSM",
                    Price = 450,
                    Capacity = 180,
                    ImageUrl = "/images/kugu-golu.jpg",
                    Genre = StageGenre.Dance
                },
                new CreateStageEventCommand
                {
                    Title = "Modern Dans Gösterisi",
                    Description = "Çağdaş modern dans performansı, yaratıcı koreografiyle!",
                    Date = new DateTime(2025, 9, 10),
                    Location = "Ankara CSO Ada",
                    Price = 250,
                    Capacity = 100,
                    ImageUrl = "/images/modern-dans.jpg",
                    Genre = StageGenre.Dance
                },
                new CreateStageEventCommand
                {
                    Title = "Flamenko Gecesi",
                    Description = "İspanyol flamenko dansı, tutkulu ritimlerle!",
                    Date = new DateTime(2025, 9, 15),
                    Location = "İzmir Ahmed Adnan Saygun Sanat Merkezi",
                    Price = 320,
                    Capacity = 80,
                    ImageUrl = "/images/flamenko.jpg",
                    Genre = StageGenre.Dance
                },

                // Opera Etkinlikleri
                new CreateStageEventCommand
                {
                    Title = "La Traviata",
                    Description = "Verdi'nin ölümsüz operası La Traviata, muhteşem seslerle!",
                    Date = new DateTime(2025, 8, 30),
                    Location = "İstanbul Süreyya Operası",
                    Price = 600,
                    Capacity = 120,
                    ImageUrl = "/images/la-traviata.jpg",
                    Genre = StageGenre.Opera
                },
                new CreateStageEventCommand
                {
                    Title = "Carmen",
                    Description = "Bizet'in Carmen operası, tutkulu hikayesiyle!",
                    Date = new DateTime(2025, 9, 12),
                    Location = "Ankara Opera Sahnesi",
                    Price = 550,
                    Capacity = 90,
                    ImageUrl = "/images/carmen.jpg",
                    Genre = StageGenre.Opera
                },
                new CreateStageEventCommand
                {
                    Title = "Tosca",
                    Description = "Puccini'nin Tosca operası, dramatik hikayesiyle!",
                    Date = new DateTime(2025, 9, 25),
                    Location = "İzmir Devlet Opera ve Balesi",
                    Price = 480,
                    Capacity = 110,
                    ImageUrl = "/images/tosca.jpg",
                    Genre = StageGenre.Opera
                }
            };

            var results = new List<Guid>();
            foreach (var evt in events)
            {
                var result = await _mediator.Send(evt);
                results.Add(result);
            }

            return Ok(new { message = $"{results.Count} sahne etkinliği başarıyla eklendi.", eventIds = results });
        }
    }
} 