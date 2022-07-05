namespace Company.Data.Models
{
    using System;

    using Company.Data.Common.Models;

    public class ZipCode : BaseModel<string>, IDeletableEntity
    {
        public ZipCode()
        {

        }
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
