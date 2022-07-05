namespace Company.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Company.Data.Common.Models;

    public class CountryCode : BaseModel<string>, IDeletableEntity
    {
        public CountryCode()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Countries = new HashSet<Country>();
            this.Users = new HashSet<ApplicationUser>();
        }

        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        public ICollection<Country> Countries { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
