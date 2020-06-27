using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyModels
{
    public partial class Records
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // 自增
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AlbumId { get; set; }
        public int Count { get; set; }
        public int OrderId { get; set; }

        public virtual Albums Album { get; set; }
        public virtual Orders Order { get; set; }
        public virtual Users User { get; set; }
    }
}
