using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace NeoRxTask.DTOs
{
    public class CreateBusinessCardDto
    {
        [JsonPropertyName("name")]
        public string ? Name { get; set; }

        //JsonPropertyName //map Your C# property name is Gender But the JSON coming from frontend or API uses gender(lowercase or different name)

        [JsonPropertyName("gender")]
        public string ? Gender { get; set; }

        [JsonPropertyName("dob")]
        public DateTime DOB { get; set; }

        [JsonPropertyName("email")]
        public string ? Email { get; set; }

        [JsonPropertyName("phoneNumber")]
        public String ? PhoneNumber { get; set; }

        [JsonPropertyName("address")]
        public String ? Address { get; set; }
        public IFormFile? Photo { get; set; }

    }
}
