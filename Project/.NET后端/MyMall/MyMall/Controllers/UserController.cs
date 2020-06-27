using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MyModels;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Warnings;
using System.Threading.Tasks;
using Params;
using System.Web.Http.Cors;


namespace MyMall.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private MySqlContext db = new MySqlContext();

        // GET: api/User
        //public IQueryable<TUser> GetTUsers()
        //{
        //    return db.TUser;
        //}

        // GET: api/User/5
        /// <summary>
        /// 根据 id 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Users))]
        public async Task<IHttpActionResult> GetTUser(int id)
        {
            Users tUser = await db.User.FindAsync(id);
            if (tUser == null)
            {
                return NotFound();
            }
            tUser.Password = null;
            return Ok(tUser);
        }

        // GET: api/User/5
        /// <summary>
        /// 根据 phone 获取用户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [ResponseType(typeof(Users))]
        public async Task<IHttpActionResult> GetTUser(string phone)
        {
            var found = await (from u in db.User
                               where u.Phone == phone
                               select u)
                         .ToListAsync();
            if (found == null || found.Count() != 1)
            {
                return NotFound();
            }

            found[0].Password = null;
            return Ok(found[0]);
        }

        // POST: api/User
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="requestLogin"></param>
        /// <returns></returns>
        [ResponseType(typeof(Users))]
        [HttpPost]
        [Route("api/user/login")]
        public async Task<IHttpActionResult> Login(ParamLogin requestLogin)
        {
            var found = await (from u in db.User
                               where u.Phone == requestLogin.Id && u.Password == requestLogin.Password
                               select u)
                         .ToListAsync();
            if (found == null || found.Count() != 1)
            {
                return NotFound();
            }
            found[0].Password = null;
            return Ok(found[0]);
        }

        // PUT: api/User/5
        /// <summary>
        /// 更改用户个人信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tUser"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> PutTUser(Users tUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Users u = await db.User.FindAsync(tUser.Id);
            u.Province = tUser.Province;
            u.City = tUser.City;
            u.District = tUser.District;
            u.Address = tUser.Address;
            db.Entry(u).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TUserExists(tUser.Id))
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

        // POST: api/User
        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="tUser"></param>
        /// <returns></returns>
        [ResponseType(typeof(Users))]
        public async Task<IHttpActionResult> PostTUser(Users tUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (TUserExists(tUser.Phone))
            {
                return BadRequest(Warnings.Class1.UserExists);
            }
            db.User.Add(tUser);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                await db.SaveChangesAsync();
            }
            tUser.Password = null;
            return CreatedAtRoute("DefaultApi", new { id = tUser.Id }, tUser);
        }

        // DELETE: api/User/5
        /// <summary>
        /// 删除用户（注销账号）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Users))]
        public async Task<IHttpActionResult> DeleteTUser(int id)
        {
            Users tUser = await db.User.FindAsync(id);
            if (tUser == null)
            {
                return NotFound();
            }

            db.User.Remove(tUser);
            await db.SaveChangesAsync();

            tUser.Password = null;
            return Ok(tUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TUserExists(int id)
        {
            return db.User.Count(e => e.Id == id) > 0;
        }

        private bool TUserExists(string phone)
        {
            return db.User.Count(e => e.Phone == phone) > 0;
        }
    }
}