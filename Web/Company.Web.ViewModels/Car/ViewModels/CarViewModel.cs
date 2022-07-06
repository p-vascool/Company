namespace Company.Web.ViewModels.Car.ViewModels
{
    using Company.Data.Models;
    using Company.Services.Mapping;

    public class CarViewModel : IMapFrom<Car>
    {
        public int Id { get; set; }

        public string CarImageUrl { get; set; }

        public string Model { get; set; }

        public string Brand { get; set; }

        public string Color { get; set; }

        public int Seats { get; set; }

        public int YearOfManufacture { get; set; }

        public bool IsLuggageAvaliable { get; set; }

        public bool IsSmokingAllowed { get; set; }

        public bool IsAirConditiningAvailable { get; set; }

        public bool IsAllowedForPets { get; set; }
    }
}
