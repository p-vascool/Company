namespace Company.Data.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Company.Data.Common.Models;

    public class City : BaseModel<string>, IDeletableEntity
    {
        public City()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ZipCodes = new HashSet<ZipCode>();
            this.Users = new HashSet<ApplicationUser>();
        }

        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [ForeignKey(nameof(State))]
        public int StateId { get; set; }

        public State State { get; set; }

        [Required]
        [ForeignKey(nameof(Country))]
        public string CountryId { get; set; }

        public Country Country { get; set; }

        public ICollection<ZipCode> ZipCodes { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
