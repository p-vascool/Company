namespace Company.Web.ViewModels.Destinations.ViewModels
{
    using Company.Data.Models;
    using Company.Services.Mapping;

    public class DestinationViewModel : IMapFrom<Destination>
    {
        public int Id { get; set; }

        public string Region { get; set; }

        public string Name { get; set; }
    }
}