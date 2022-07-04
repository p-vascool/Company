﻿namespace Company.Data.Models
{
    using System;

    using Company.Data.Common.Models;

    public class UserTrip : BaseModel<string>, IDeletableEntity
    {
        public UserTrip()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string TripId { get; set; }

        public Trip Trip { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
