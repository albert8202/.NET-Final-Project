using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyModels
{
    public partial class Categorys
    {
        [Key]
        [Column(Order = 1)]
        public int AlbumId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int CategoryId { get; set; }

        public virtual Albums Album { get; set; }
        public virtual CategoryNames CategoryName { get; set; }
    }
}
