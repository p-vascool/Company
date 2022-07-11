namespace Company.Web.ViewModels.Users.ViewModels
{
    using Company.Data.Models.Enums;

    public class UserProfileViewModel
    {
        public ProfileTab ActiveTab { get; set; }

        public ApplicationUserViewModel ApplicationUser { get; set; }

        public bool HasAdmin { get; set; }

        public int Page { get; set; }

        public int CreatedPosts { get; set; }

        public int LikedPosts { get; set; }

        public int CommentsCount { get; set; }

        public double RatingScore { get; set; }

        public int LatestScore { get; set; }
    }
}
