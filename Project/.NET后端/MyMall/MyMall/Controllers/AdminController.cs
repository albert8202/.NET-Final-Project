using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MyModels;
using System.Web.Http.Cors;

using System.Threading.Tasks;

namespace MyMall.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AdminController : ApiController
    {
        private MySqlContext db = new MySqlContext();
      

        [Route("api/admin/initialize")]
        [HttpGet]
        public IHttpActionResult Wall()
        {
            var admin = db.Admin;
            var user = db.User;
            var book = db.Album;
            var cart = db.Cart;
            var order = db.Order;
            var history = db.Record;
            var delivery = db.Delivery;
            var category = db.Category;
            return Ok();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AdminExists(int id)
        {
            return db.Admin.Count(e => e.Id == id) > 0;
        }

  
        // POST: api/Admin/login
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="requestLogin"></param>
        /// <returns></returns>
        /*[ResponseType(typeof(Admins))]
        [HttpPost]
        [Route("api/admin/login")]
        public async Task<IHttpActionResult> Login(RequestLogin requestLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var found = await (from a in db.Admin
                               where a.Name == requestLogin.Id && a.Password == requestLogin.Password
                               select a)
                         .ToListAsync();
            if (found.Count() != 1)
            {
                return NotFound();
            }
            else
            {
                found[0].Password = null;
                return Ok(found[0]);
            }
        }*/

        // POST: api/Admin
        /// <summary>
        /// 新增管理员
        /// </summary>
        /// <param name="tAdmin"></param>
        /// <returns></returns>
        [ResponseType(typeof(Admins))]
        public async Task<IHttpActionResult> PostTAdmin(Admins tAdmin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Admin.Add(tAdmin);
            await db.SaveChangesAsync();
            tAdmin.Password = null;
            return CreatedAtRoute("DefaultApi", new { id = tAdmin.Id }, tAdmin);
        }

     
    }
}