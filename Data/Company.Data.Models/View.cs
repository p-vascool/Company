namespace Company.Data.Models
{
    using System;

    using Company.Data.Common.Models;

    public class View : BaseModel<string>
    {
        public View()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public Trip Trip { get; set; }

        public string UserId { get; set; }
    }
}
