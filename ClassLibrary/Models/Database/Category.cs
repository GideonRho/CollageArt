using System.Collections.Generic;

namespace ClassLibrary.Models.Database
{
    public class Category
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        
        public bool IsPrivate { get; set; }
        
        public Art TitleArt { get; set; }
        public List<Art> Arts { get; set; } = new();

        public override string ToString()
        {
            return Name;
        }
    }
}