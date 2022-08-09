using ClassLibrary.Models.Database;
using Microsoft.Extensions.Configuration;

namespace ClassLibrary.Services
{
    public class LinkService
    {
        private IConfiguration Configuration { get; }
     
        public LinkService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string ArtPath(Art art)
        {
            return $"{WebRoot}/art/{art.Id}";
        }
        
        public string CategoryPath(Category category)
        {
            return $"{WebRoot}/category/{category.Id}";
        }
        
        public string TextPath(TextPage text)
        {
            return $"{WebRoot}/text/{text.Id}";
        }
        
        public string HomePath()
        {
            return $"{WebRoot}";
        }
        
        private string WebRoot => Configuration["WebRoot"];

    }
}