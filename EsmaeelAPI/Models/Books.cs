using System;
using System.Collections.Generic;

namespace EsmaeelAPI.Models
{
    public partial class Books
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public decimal BookPrice { get; set; }
        public int Quantity { get; set; }
    }
}
