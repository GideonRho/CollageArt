using ClassLibrary.Models.Enums;

namespace ClassLibrary.Models.Database
{
    public class TextPage
    {
        
        public int Id { get; set; }
        public TextType Type { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        
        public bool IsPrivate { get; set; }
        
    }
}