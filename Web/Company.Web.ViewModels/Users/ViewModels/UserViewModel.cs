namespace Company.Web.ViewModels.Users.ViewModels
{
    using System;
    using System.Collections.Generic;

    using Company.Data.Models;
    using Company.Data.Models.Enums;
    using Company.Services.Mapping;

    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public string UserImageUrl { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string PhoneNumber { get; set; }

        public virtual ICollection<UserTrip> UserTrips { get; set; }
    }
}
