namespace Medical_clinic.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
