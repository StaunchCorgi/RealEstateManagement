namespace RealEstateManagementAPI.Dto
{
    public class CreatePropertyDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string Location { get; set; } = null!;
        public string PropertyType { get; set; } = null!;
        public string Status { get; set;  } = null!;
        public int AgentId { get; set; }
    }
}
