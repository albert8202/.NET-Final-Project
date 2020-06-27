using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyModels
{
    public partial class Deliverys
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // 自增
        public int Id { get; set; }
        public string Province { get; set; }
        public decimal DeliveryFee { get; set; }
        public int DeliveryTime { get; set; }
    }
}
