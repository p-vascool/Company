namespace Company.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Company.Data.Common.Models;

    public class State : BaseModel<string>, IDeletableEntity
    {
        public State()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [ForeignKey(nameof(Country))]
        public string CountryId { get; set; }

        public ICollection<City> Cities { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        public Country Country { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
