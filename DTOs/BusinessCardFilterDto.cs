using Microsoft.AspNetCore.Mvc;
using Moq;

namespace NeoRxTask.DTOs
{
    public class BusinessCardFilterDto
    {
        [FromQuery(Name = "name")]
        public string? Name { get; set; }

        //[FromQuery]
        //tells ASP.NET Core:“Take the value from the query string in the URL and assign it to this property.

        [FromQuery(Name = "email")]
        public string? Email { get; set; }

        [FromQuery(Name = "phoneNumber")]
        public string? PhoneNumber { get; set; }

        [FromQuery(Name = "gender")]
        public string? Gender { get; set; }

        [FromQuery(Name = "dob")]
        public DateTime? DOB { get; set; }//? for Nullable
    }
}
