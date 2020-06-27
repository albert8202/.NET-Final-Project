using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyModels
{
    public partial class Orders
    {
        public Orders()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // 自增
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset OrderTime { get; set; }
        public int Status { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverAddress { get; set; }
        public decimal DeliveryFee { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<Records> Record { get; set; }
    }
}
