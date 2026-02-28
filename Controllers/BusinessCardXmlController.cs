using Microsoft.AspNetCore.Mvc;
using NeoRxTask.DTOs;
using NeoRxTask.Services;
using NeoRxTask.Services.IServices;

namespace NeoRxTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessCardXmlController : ControllerBase
    {
        private readonly IBusinessCardXmlService _xmlService;
        private readonly IBusinessCardService _service;
        public BusinessCardXmlController (IBusinessCardXmlService xmlService,IBusinessCardService service)
        {
            _xmlService= xmlService;
            _service= service;
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportXml([FromQuery] string? name,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        {
            var allCards = await _service.FilterAsync(new BusinessCardFilterDto { Name = name });
            var createdDtos = new List<CreateBusinessCardDto>();
            var pagedCards = allCards
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();




            var xml=_xmlService.ExportToXml(pagedCards);
            return File(System.Text.Encoding.UTF8.GetBytes(xml), "application/xml", "BusinessCards.xml");
        }
        [HttpPost("import")]
        public async Task<IActionResult> ImportXml( IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            using var reader = new StreamReader(file.OpenReadStream());
            var xml = await reader.ReadToEndAsync();
            var cards = _xmlService.ImportFromXml(xml);
            var createdDtos = new List<CreateBusinessCardDto>();

            foreach (var card in cards) // cards = List<BusinessCard> from XML
            {
                var dto = new CreateBusinessCardDto
                {
                    Name = card.Name,
                    Gender = card.Gender,
                    DOB = card.DOB,
                    Email = card.Email,
                    PhoneNumber = card.PhoneNumber,
                    Address = card.Address



                };

                await _service.CreateAsync(dto);
                createdDtos.Add(dto);

            }

            return Ok(new
            {
                Count = createdDtos.Count,
                Created = createdDtos
            });
        }
    }
}
