using System.ComponentModel.DataAnnotations;

namespace NeoRxTask.Entities
{
    public class BusinessCard
    {
        [Key] public int Id { get; set; }

        [MaxLength(100)]
        public string Name {  get; set; }
        public string Gender { get; set; }
        public DateTime DOB {  get; set; }
        //public int CardId { get; set; } = 0;
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        [Required]
        public String PhoneNumber {  get; set; }
        public String Address {  get; set; }
        //public BusinessCard() { }   
        public string? PhotoPath { get; set; }

    }
}
