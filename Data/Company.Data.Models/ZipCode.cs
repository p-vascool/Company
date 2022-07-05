namespace Company.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Company.Data.Common.Models;

    public class ZipCode : BaseModel<string>, IDeletableEntity
    {
        public ZipCode()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public int Code { get; set; }

        // [Required]
        [ForeignKey(nameof(City))]
        public string CityId { get; set; }

        public City City { get; set; }

        public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new HashSet<ApplicationUser>();

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
