using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;
namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    public class PlatformController : Controller
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public PlatformController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public ActionResult <IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting platforms...");
            var platforms = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms)); 
        }



        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Test Inbound Connection");
            return Ok("Test Inbound Connection");
        }
    }
}