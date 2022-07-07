namespace Company.Web.ViewModels.Car.ViewModels
{
    using System.Collections.Generic;

    public class CarListViewModel
    {
        public string UserUsername { get; set; }

        public string UserId { get; set; }

        public IEnumerable<CarViewModel> Cars { get; set; }
    }
}
