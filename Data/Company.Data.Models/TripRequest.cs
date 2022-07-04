namespace Company.Data.Models
{
    using System;

    using Company.Data.Common.Models;
    using Company.Data.Models.Enums;

    public class TripRequest : BaseModel<string>, IDeletableEntity, IAuditInfo
    {
        public TripRequest()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public Trip Trip { get; set; }

        public string TripId { get; set; }

        public RequestStatus Status { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
