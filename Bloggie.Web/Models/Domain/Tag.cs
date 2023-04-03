namespace Bloggie.Web.Models.Domain
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        // relationship between blog post and tag is defined
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
