using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyModels
{
    public partial class Albums
    {
        public Albums()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // 自增
        public int Id { get; set; }
        public string Isbn { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string CoverUrl { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public int Sales { get; set; }
        [NotMapped]
        public string Category { get; set; }

        public virtual ICollection<Categorys> Categorys { get; set; }
        public virtual ICollection<CategoryNames> CategoryName { get; set; }
        public virtual ICollection<Carts> Cart { get; set; }
        public virtual ICollection<Records> Record { get; set; }
    }
}
