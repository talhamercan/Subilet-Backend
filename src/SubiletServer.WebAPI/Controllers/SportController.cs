using MediatR;
using Microsoft.AspNetCore.Mvc;
using SubiletServer.Application.SportEvents.Commands;
using SubiletServer.Application.SportEvents.Queries;
using SubiletServer.Domain.Entities;

namespace SubiletServer.WebAPI.Controllers
{
    [ApiController]
    [Route("spor")]
    public class SportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSportEvents()
        {
            var query = new GetSportEventsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("futbol")]
        public async Task<IActionResult> GetFootballEvents()
        {
            var query = new GetSportEventsQuery { Genre = SportGenre.Football };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("basketbol")]
        public async Task<IActionResult> GetBasketballEvents()
        {
            var query = new GetSportEventsQuery { Genre = SportGenre.Basketball };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("tenis")]
        public async Task<IActionResult> GetTennisEvents()
        {
            var query = new GetSportEventsQuery { Genre = SportGenre.Tennis };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSportEvent([FromBody] CreateSportEventCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAllSportEvents), new { id = result }, result);
        }

        [HttpPost("seed")]
        public async Task<IActionResult> SeedSportEvents()
        {
            var events = new List<CreateSportEventCommand>
            {
                // Futbol Etkinlikleri
                new CreateSportEventCommand
                {
                    Title = "Galatasaray vs Fenerbahçe",
                    Description = "Süper Lig'in en büyük derbisi! Galatasaray vs Fenerbahçe maçı.",
                    Date = new DateTime(2025, 8, 20),
                    Location = "Türk Telekom Stadyumu",
                    Price = 800,
                    Capacity = 52000,
                    ImageUrl = "/images/gs-fb.jpg",
                    Genre = SportGenre.Football
                },
                new CreateSportEventCommand
                {
                    Title = "Beşiktaş vs Trabzonspor",
                    Description = "Beşiktaş vs Trabzonspor maçı, heyecan dolu bir karşılaşma!",
                    Date = new DateTime(2025, 9, 5),
                    Location = "Vodafone Park",
                    Price = 600,
                    Capacity = 41000,
                    ImageUrl = "/images/bjk-ts.jpg",
                    Genre = SportGenre.Football
                },
                new CreateSportEventCommand
                {
                    Title = "Adana Demirspor vs Antalyaspor",
                    Description = "Adana Demirspor vs Antalyaspor maçı, güney derbisi!",
                    Date = new DateTime(2025, 9, 15),
                    Location = "5 Ocak Fatih Terim Stadyumu",
                    Price = 400,
                    Capacity = 33000,
                    ImageUrl = "/images/ads-ats.jpg",
                    Genre = SportGenre.Football
                },

                // Basketbol Etkinlikleri
                new CreateSportEventCommand
                {
                    Title = "Anadolu Efes vs Fenerbahçe",
                    Description = "Türk basketbolunun en büyük derbisi! Anadolu Efes vs Fenerbahçe.",
                    Date = new DateTime(2025, 8, 25),
                    Location = "Sinan Erdem Spor Salonu",
                    Price = 350,
                    Capacity = 16000,
                    ImageUrl = "/images/efes-fb.jpg",
                    Genre = SportGenre.Basketball
                },
                new CreateSportEventCommand
                {
                    Title = "Galatasaray vs Beşiktaş",
                    Description = "Galatasaray vs Beşiktaş basketbol maçı!",
                    Date = new DateTime(2025, 9, 8),
                    Location = "Abdi İpekçi Arena",
                    Price = 300,
                    Capacity = 12000,
                    ImageUrl = "/images/gs-bjk.jpg",
                    Genre = SportGenre.Basketball
                },
                new CreateSportEventCommand
                {
                    Title = "Tofaş vs Pınar Karşıyaka",
                    Description = "Tofaş vs Pınar Karşıyaka maçı!",
                    Date = new DateTime(2025, 9, 18),
                    Location = "Bursa Atatürk Spor Salonu",
                    Price = 200,
                    Capacity = 8000,
                    ImageUrl = "/images/tofas-karsiyaka.jpg",
                    Genre = SportGenre.Basketball
                },

                // Tenis Etkinlikleri
                new CreateSportEventCommand
                {
                    Title = "İstanbul Cup Tenis Turnuvası",
                    Description = "WTA İstanbul Cup tenis turnuvası, dünya yıldızları sahalarda!",
                    Date = new DateTime(2025, 8, 30),
                    Location = "İstanbul Tenis Kulübü",
                    Price = 500,
                    Capacity = 5000,
                    ImageUrl = "/images/istanbul-cup.jpg",
                    Genre = SportGenre.Tennis
                },
                new CreateSportEventCommand
                {
                    Title = "Antalya Tenis Turnuvası",
                    Description = "Antalya'da düzenlenen uluslararası tenis turnuvası!",
                    Date = new DateTime(2025, 9, 10),
                    Location = "Antalya Tenis Merkezi",
                    Price = 400,
                    Capacity = 3000,
                    ImageUrl = "/images/antalya-tenis.jpg",
                    Genre = SportGenre.Tennis
                },
                new CreateSportEventCommand
                {
                    Title = "İzmir Tenis Şampiyonası",
                    Description = "İzmir'de düzenlenen yerel tenis şampiyonası!",
                    Date = new DateTime(2025, 9, 20),
                    Location = "İzmir Tenis Kulübü",
                    Price = 250,
                    Capacity = 2000,
                    ImageUrl = "/images/izmir-tenis.jpg",
                    Genre = SportGenre.Tennis
                }
            };

            var results = new List<Guid>();
            foreach (var evt in events)
            {
                var result = await _mediator.Send(evt);
                results.Add(result);
            }

            return Ok(new { message = $"{results.Count} spor etkinliği başarıyla eklendi.", eventIds = results });
        }
    }
} 