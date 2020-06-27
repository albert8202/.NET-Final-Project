using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MyModels;
using Params;
using System.Web.Http.Cors;
using ATLProject1Lib;

namespace MyMall.Controllers

{
    public class MyOrder
    {
        public List<int> cartList { get; set; }
        public List<string> receiverAddress { get; set; }
        public string receiverName { get; set; }
        public string receiverPhone { get; set; }
        public int userId { get; set; }

    }
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OrderController : ApiController
    {
        private MySqlContext db = new MySqlContext();
        private static Mutex mutex = new Mutex();

        [DllImport("C:\\Users\\admin\\Desktop\\MyMall\\Debug\\Win32dll.dll", CallingConvention = CallingConvention.Cdecl)] //  EntryPoint = "calculate", 
        public static extern float calculatePrice(float a, float b);


        /// GET: api/Order
        //public IQueryable<Orders> GetTOrders()
        //{
        //    return db.Order;
        //}

        // GET: api/Order?userId=
        /// <summary>
        /// 获取某用户的所有订单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetTOrders(int userId)
        {
            var found = await (from o in db.Order
                               where o.UserId == userId
                               select o).ToListAsync();
            if (found == null || found.Count() < 0)
            {
                return Ok();
            }
            return Ok(found);
        }

        // GET: api/Order?userId=&status=
        /// <summary>
        /// 获取某类订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetTOrders(int userId, int status)
        {
            var found = await (from o in db.Order
                               where o.UserId == userId && o.Status == status
                               select o).ToListAsync();
            if (found == null || found.Count() < 0)
            {
                return Ok();
            }
            return Ok(found);
        }

        // GET: api/Order/5
        /// <summary>
        /// 获取某个订单的列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetTOrder(int id)
        {
            var found = await (from h in db.Record
                               where h.OrderId == id
                               select h).ToListAsync();
            if (found == null || found.Count() <= 0)
            {
                return Ok();
            }

            return Ok(found);
        }

        // Put: api/Order/5
        /// <summary>
        /// 更改订单状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTOrder(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Orders tOrder = await db.Order.FindAsync(id);
            if (tOrder == null)
            {
                return NotFound();
            }

            IATLSimpleObject c = new ATLSimpleObject();
            int newStatus = tOrder.Status;
            c.changeStatus(newStatus, out newStatus);
            tOrder.Status = newStatus;
            
            db.Entry(tOrder).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Order
        /// <summary>
        /// 生成新订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ResponseType(typeof(Orders))]
        public async Task<IHttpActionResult> PostTOrder(MyOrder request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Orders tOrder = new Orders();
            tOrder.UserId = request.userId;
            tOrder.ReceiverName = request.receiverName;
            tOrder.ReceiverPhone = request.receiverPhone;

            // 拼接字符串
            StringBuilder address = new StringBuilder();
            foreach (string s in request.receiverAddress)
            {
                address.Append(s);
            }
            tOrder.ReceiverAddress = address.ToString();

            // 计算运费
            var fee = await (from d in db.Delivery
                             where d.Province == request.receiverAddress[0]
                             select d.DeliveryFee).ToListAsync();
            if (fee == null || fee.Count() <= 0)
            {
                tOrder.DeliveryFee = 10;
            }
            else
            {
                tOrder.DeliveryFee = fee[0];
            }

            // 计算价格
            mutex.WaitOne();
            tOrder.TotalPrice = 0;
            foreach (int id in request.cartList)
            {
                Carts cart = await db.Cart.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
                if (cart == null)
                {
                    return NotFound();
                }
                Albums book = await db.Album.Where(x => x.Id == cart.AlbumId).AsNoTracking().FirstOrDefaultAsync();
                if (book == null)
                {
                    return NotFound();
                }
                else if (book.Stock < cart.Count)
                {
                    return BadRequest(Warnings.Class1.AlbumOutOfStock);
                }
                tOrder.TotalPrice += cart.Count * book.Price;
            }
            tOrder.TotalPrice = tOrder.TotalPrice + tOrder.DeliveryFee;
            // tOrder.TotalPrice = (decimal)calculatePrice((float)tOrder.TotalPrice, (float)tOrder.DeliveryFee);

            tOrder.Id = 0;
            db.Order.Add(tOrder);
            try
            {
            await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                    throw ex;
            }

            foreach (int id in request.cartList)
            {
                Carts cart = await db.Cart.FindAsync(id);
                Albums book = await db.Album.FindAsync(cart.AlbumId);
                Records history = new Records();
                history.AlbumId = cart.AlbumId;
                history.UserId = cart.UserId;
                history.Count = cart.Count;
                history.OrderId = tOrder.Id;

                book.Sales += cart.Count;
                book.Stock -= cart.Count;
                db.Album.Attach(book);
                db.Entry(book).State = EntityState.Modified;
                db.Cart.Remove(cart);
                db.Record.Add(history);
            }
            await db.SaveChangesAsync();
            mutex.ReleaseMutex();
            return Ok(tOrder.Id);
            // return CreatedAtRoute("DefaultApi", new { id = tOrder.Id }, tOrder);
        }

        // DELETE: api/Order/5
        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Orders))]
        public async Task<IHttpActionResult> DeleteTOrder(int id)
        {
            Orders tOrder = await db.Order.FindAsync(id);
            if (tOrder == null)
            {
                return NotFound();
            }

            db.Order.Remove(tOrder);
            await db.SaveChangesAsync();

            return Ok(tOrder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TOrderExists(int id)
        {
            return db.Order.Count(e => e.Id == id) > 0;
        }
    }
}