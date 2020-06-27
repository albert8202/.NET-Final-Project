using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MyModels;
using Params;
using System.Web.Http.Cors;


namespace MyMall.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CartController : ApiController
    {
        private MySqlContext db = new MySqlContext();

        // GET: api/Cart
        //public IQueryable<TCart> GetTCarts()
        //{
        //    return db.TCart;
        //}

        // GET: api/Cart/5
        /// <summary>
        /// 获取某用户的所有购物车信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetTCarts(int userId)
        {
            return Ok(await db.Cart.Where(c => c.UserId == userId).ToListAsync());
        }

        /// <summary>
        /// 获取某个购物车条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetTCart(int id)
        {
            Carts cart = await db.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        // PUT: api/Cart/5
        /// <summary>
        /// 更改某个购物车条目（数量）
        /// </summary>
        /// <param name="tCart"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> PutTCart(ParamCartCount request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Carts tCart = await db.Cart.FindAsync(request.id);
            if (tCart == null)
            {
                return NotFound();
            }
            tCart.Count = request.count;
            db.Entry(tCart).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TCartExists(tCart.Id))
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

        // POST: api/Cart
        /// <summary>
        /// 添加购物车条目
        /// </summary>
        /// <param name="tCart"></param>
        /// <returns></returns>
        [ResponseType(typeof(Carts))]
        public async Task<IHttpActionResult> PostTCart(Carts tCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cart.Add(tCart);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }

            return CreatedAtRoute("DefaultApi", new { id = tCart.Id }, tCart);
        }

        // DELETE: api/Cart/5
        /// <summary>
        /// 删除购物车条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Carts))]
        public async Task<IHttpActionResult> DeleteTCart(int id)
        {
            Carts tCart = await db.Cart.FindAsync(id);
            if (tCart == null)
            {
                return NotFound();
            }

            db.Cart.Remove(tCart);
            await db.SaveChangesAsync();

            return Ok(tCart);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TCartExists(int id)
        {
            return db.Cart.Count(e => e.Id == id) > 0;
        }
    }
}