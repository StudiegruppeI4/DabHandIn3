using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DabHandIn3.Models.Objects
{
    public class Comment
    {
        public string Content { get; set; }
        public User Author { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
