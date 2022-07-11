namespace Company.Web.ViewModels.Contacts.InputModel
{
    using System.ComponentModel.DataAnnotations;

    using Company.Data.Models;
    using Company.Services.Mapping;

    public class ContactInputModel : IMapFrom<ContactFormEntry>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
