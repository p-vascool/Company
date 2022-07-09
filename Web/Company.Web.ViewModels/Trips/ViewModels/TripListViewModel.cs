namespace Company.Web.ViewModels.Trips.ViewModels
{
    using System.Collections.Generic;

    using Company.Web.ViewModels.Trips.InputModels;

    public class TripListViewModel
    {
        public IEnumerable<TripDetailsViewModel> Trips { get; set; }

        public TripSearchInputModel SearchQuery { get; set; }
    }
}
