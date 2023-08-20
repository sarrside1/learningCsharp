using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using PlatformServices.AsyncDataServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    public class PlatformController : Controller
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformController(
            IPlatformRepo platformRepo,
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient
            )
        {
            _platformRepo = platformRepo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }
        // GET: api/values
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms ....");
            var platforms = _platformRepo.GetAllPlatform();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            Console.WriteLine("--> Getting Platform "+ id +" ....");
            var platformItem = _platformRepo.GetPlatformById(id);
            if(platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }
            else
            {
                return NotFound("Platform with this id not found");
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform([FromBody] PlatformCreateDto platformCreate)
        {
            var platformModel = _mapper.Map<Platform>(platformCreate);
            _platformRepo.CreatePlatform(platformModel);
            _platformRepo.SaveChanges();
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
            // Send sync message
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"--> Could not send synchronously : {ex.Message}");
            }

            //Send async message
            try
            {
                var platformPublishedDtoPlatform = _mapper.Map<PlatformPublishedDto>(platformReadDto);
                platformPublishedDtoPlatform.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDtoPlatform);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"--> Could not send synchronously : {ex.Message}");
            }
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

