namespace How_To_Approved_Record_MVC_Application.Models
{
   
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsApproved { get; set; }
    }

}
