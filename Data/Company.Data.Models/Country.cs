namespace Company.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Company.Data.Common.Models;

    public class Country : BaseModel<string>, IDeletableEntity
    {
        public Country()
        {
            this.Id = Guid.NewGuid().ToString();
            this.States = new HashSet<State>();
            this.Cities = new HashSet<City>();
            this.Users = new HashSet<ApplicationUser>();
        }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [ForeignKey(nameof(CountryCode))]
        public string CountryCodeId { get; set; }

        public CountryCode CountryCode { get; set; }

        public ICollection<State> States { get; set; }

        public ICollection<City> Cities { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
