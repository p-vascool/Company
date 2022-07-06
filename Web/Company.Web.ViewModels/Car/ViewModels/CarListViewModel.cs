using System.Collections.Generic;

namespace Company.Web.ViewModels.Car.ViewModels
{
    public class CarListViewModel
    {
        public string UserUsername { get; set; }

        public string UserId { get; set; }

        public IEnumerable<CarViewModel> Cars { get; set; }
    }
}
