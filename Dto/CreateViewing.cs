namespace RealEstateManagementAPI.Dto
{
    public class CreateViewing
    {
        public int PropertyId { get; set; }
        public int UserId { get; set; }
       public DateTime ScheduledDate { get; set;  } 
    }
}
