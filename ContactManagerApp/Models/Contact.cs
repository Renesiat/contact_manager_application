using System.ComponentModel.DataAnnotations;

namespace ContactManagerApp.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBbirth { get; set; }
        public bool Married { get; set; }
        public string? Phone { get; set; }
        public decimal Salary { get; set; }

    }
}
