namespace Company.Web.ViewModels.Trips.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Company.Web.ViewModels.Trips.ViewModels;

    public class TripSearchInputModel
    {
        [Display(Name = "From town")]
        public int FromDestinationId { get; set; }

        [Display(Name = "To town")]
        public int ToDestinationId { get; set; }

        [Display(Name = "Date of departure")]
        public DateTime? DateOfDeparture { get; set; }

        public ICollection<TripDetailsViewModel> Trips { get; set; }
    }
}
