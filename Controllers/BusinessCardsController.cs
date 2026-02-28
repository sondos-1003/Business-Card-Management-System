using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeoRxTask.Data;
using NeoRxTask.DTOs;
using NeoRxTask.Entities;
using NeoRxTask.Repositories;
using NeoRxTask.Services;
using NeoRxTask.Services.IServices;

namespace NeoRxTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessCardsController : ControllerBase
    {
        private readonly IBusinessCardService _service;
        public BusinessCardsController(IBusinessCardService service)
        {
            //_context = context;
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 5)
        {
            var(data,totalCount)= await _service.GetAllAsync(page,pageSize);
            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                data
            });
        }

        [HttpPost("withphoto")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateWithPhoto([FromBody] CreateBusinessCardDto dto)
        {
            var result = await _service.CreateWithPhotoAsync(dto);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {

            var card = await _service.GetByIdAsync(id);
            if (card == null)

                return NotFound();
            return Ok(card);

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBusinessCardDto dto)
        {
            //_context.BusinessCard.Add(card);
            var card =await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = card.Id }, card);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,UpdateBusinessCardDto dto)
        {

       

            await _service.UpdateAsync(id,dto);
             
            return Ok();
            
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] BusinessCardFilterDto filter)
        {
            var result = await _service.FilterAsync(filter);
            return Ok(result);
        }

        [HttpPost("preview")]
        public IActionResult Preview([FromBody] CreateBusinessCardDto dto)
        {
            return Ok(dto); // View Without Saving
        }
        
    }
    }
