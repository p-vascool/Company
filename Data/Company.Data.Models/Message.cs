namespace Company.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Company.Data.Common.Models;

    public class Message : BaseDeletableModel<string>
    {
        public Message()
        {
            this.Id = Guid.NewGuid().ToString();
            this.When = DateTime.Now;
        }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime When { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser Sender { get; set; }
    }
}
