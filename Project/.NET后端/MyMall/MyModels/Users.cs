using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyModels
{
    public partial class Users
    {
        public Users()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // 自增
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Carts> Cart { get; set; }
        public virtual ICollection<Records> Record { get; set; }
        public virtual ICollection<Orders> Order { get; set; }
    }
}
