using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdeoZadanie.Models
{
    public class Tree
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Display(Name ="Parent Name")]
        public int? ParentId { get; set; }
        public virtual Tree Parent { get; set; }
        public virtual ICollection<Tree> Childrens { get; set; }
    }
}