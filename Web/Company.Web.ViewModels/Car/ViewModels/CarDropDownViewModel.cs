namespace Company.Web.ViewModels.Car.ViewModels
{
    using Company.Data.Models;
    using Company.Services.Mapping;

    public class CarDropDownViewModel : IMapFrom<Car>
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string CarBrandName => $"{this.Brand} {this.Model}";
    }
}
