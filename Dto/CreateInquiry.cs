namespace RealEstateManagementAPI.Dto
{
    public class CreateInquiry
    {
        public int PropertyId { get; set; }
        public int UserId { get; set; }
       public string Message { get; set;  } = null!;
    }
}
