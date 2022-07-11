namespace Company.Web.ViewModels.TripRequests
{
    using Company.Data.Models;
    using Company.Services.Mapping;

    public class TripRequestViewModel : IMapFrom<TripRequest>
    {
        public Trip Trip { get; set; }

        public string TripId { get; set; }


        public string OwnerId { get; set; }

        public ApplicationUser Sender { get; set; }

        public string SenderId { get; set; }
    }
}
