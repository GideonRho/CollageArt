using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Models.Database
{
    public class Art
    {
        
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Date { get; set; } = "";
        public string Image { get; set; } = "";
        public int Width { get; set; }
        public int Height { get; set; }
        public string Text { get; set; } = "";
        
        public bool IsPrivate { get; set; }
        
        [InverseProperty("Arts")]
        public Category Category { get; set; }
        public List<Detail> Details { get; set; } = new ();

        public string Size() => $"{Width}x{Height}";
        
        public override string ToString()
        {
            return Title;
        }

    }
}