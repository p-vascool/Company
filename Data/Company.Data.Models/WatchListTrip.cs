namespace Company.Data.Models
{
    using Company.Data.Common.Models;

    public class WatchListTrip : BaseDeletableModel<int>
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string TripId { get; set; }

        public Trip Trip { get; set; }
    }
}
