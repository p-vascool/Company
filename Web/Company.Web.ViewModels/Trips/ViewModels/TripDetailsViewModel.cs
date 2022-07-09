namespace Company.Web.ViewModels.Trips.ViewModels
{
    using System;
    using System.Collections.Generic;

    using Company.Data.Models;
    using Company.Web.ViewModels.Car.ViewModels;

    public class TripDetailsViewModel
    {
        public string Id { get; set; }

        public ApplicationUser User { get; set; }

        public string UserId { get; set; }

        public string UserUserName { get; set; }

        public decimal PricePerPassenger { get; set; }

        public ICollection<View> Views { get; set; }

        public CarViewModel Car { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime DateOfDeparture { get; set; }

        public string DateAsString => this.DateOfDeparture.ToString("d");

        public string FromDestinationName { get; set; }

        public string ToDestinationName { get; set; }

        public string AdditionalInformation { get; set; }

        public bool SendRequest { get; set; }

        public string GroupName { get; set; }

        public IEnumerable<ApplicationUser> Passengers { get; set; }

        public IEnumerable<TripRequest> TripRequests { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
