using System.ComponentModel.DataAnnotations;

namespace How_To_Approved_Record_MVC_Application.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        // Add other properties as needed
    }
}
