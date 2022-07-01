using AutoMapper;
using Microservices.Dtos;
using Microservices.Models;
using Microservices.Repositories;
using Microsoft.AspNetCore.Mvc;
using PlatformsService.SyncDataServices.Http;

namespace Microservices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : Controller
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataCilent _commandDataClient;

        public PlatformsController(IPlatformRepo repo, IMapper mapper, ICommandDataCilent commandDataCilent)
        {
            _repo = repo;
            _mapper = mapper;
            _commandDataClient = commandDataCilent;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
             var platforms = _repo.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("{id}", Name="GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platform = _repo.GetPlatfromById(id);

            if (platform == null) return NotFound();

            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }


        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repo.CreatePlatform(platformModel);
            _repo.SaveChange();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            try
            { 
                await _commandDataClient.SendPlatfromToCommand(platformReadDto); 
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById),new {Id = platformReadDto.Id}, platformReadDto);
        }
    }
}
