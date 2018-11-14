using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EditablePrototype.Models
{
    public class Section
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public bool IsVisible { get; set; }
        public bool CanBeInvisible { get; set; }
    }
}
