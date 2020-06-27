using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyModels;
using Warnings;
using Params;
using System.Web.Http.Cors;
using Utils;

namespace MyMall.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AlbumController : ApiController
    {
        private MySqlContext db = new MySqlContext();

        // GET: api/Book
        /// <summary>
        /// 获取所有书
        /// </summary>
        /// <returns></returns>
        public IQueryable<Albums> GetTBooks()
        {
            return db.Album;
        }

        // GET: api/Book/5
        /// <summary>
        /// 获取某一本书（id）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Albums))]
        public async Task<IHttpActionResult> GetTBook(int id)
        {
            Albums Album = await db.Album.FindAsync(id);
            if (Album == null)
            {
                return BadRequest("未找到该数据");
            }

            var cates = await (from c in db.Category
                               where c.AlbumId == Album.Id
                               select c.CategoryId).ToListAsync();
            var categories = await (from c in db.CategoryName
                                    where cates.Contains(c.Id)
                                    select c.Name).ToListAsync();
            Album.Category = Utils.Convert.CategoryToString(categories);
            return Ok(Album);
        }

        // GET: api/Book?isbn=
        /// <summary>
        /// 获取某一本书（isbn）
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        [ResponseType(typeof(Albums))]
        public async Task<IHttpActionResult> GetTBook(string isbn)
        {
            var found = await (from b in db.Album
                               where b.Isbn == isbn
                               select b).ToListAsync();
            if (found == null || found.Count() <= 0)
            {
                return Ok();
            }

            return Ok(found[0]);
        }

        // GET: api/Book?keyword=
        /// <summary>
        /// 搜索书
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [ResponseType(typeof(Albums))]
        public async Task<IHttpActionResult> Search(string keyword)
        {
            var found = await (from b in db.Album
                               where b.Name.Contains(keyword) || b.Author.Contains(keyword) ||
                               b.Publisher.Contains(keyword) || b.Description.Contains(keyword)
                               select b).ToListAsync();
            if (found == null || found.Count() <= 0)
            {
                return Ok();
            }
            return Ok(found);
        }

        // PUT: api/Book/5
        /// <summary>
        /// 修改书的信息
        /// </summary>
        /// <param name="tBook"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> PutTBook(Albums tBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(tBook).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TBookExists(tBook.Id))
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

        /// <summary>
        /// 进货，增加书的库存
        /// </summary>
        /// <param name="requestStock"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/book/stock")]
        public async Task<IHttpActionResult> PutStock(ParamStock requestStock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var found = await (from b in db.Album
                               where b.Isbn == requestStock.isbn
                               select b).ToListAsync();
            if (found == null || found.Count() <= 0)
            {
                return NotFound();
            }

            Albums book = found[0];
            book.Stock = requestStock.stock;
            db.Entry(book).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TBookExists(book.Id))
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

        // POST: api/Book
        /// <summary>
        /// 添加书
        /// </summary>
        /// <param name="tBook"></param>
        /// <returns></returns>
        [ResponseType(typeof(Albums))]
        public async Task<IHttpActionResult> PostTBook(Albums tBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Album.Add(tBook);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tBook.Id }, tBook);
        }

        // DELETE: api/Book/5
        /// <summary>
        /// 删除书（下架）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Albums))]
        public async Task<IHttpActionResult> DeleteTBook(string isbn)
        {
            var found = await (from b in db.Album
                               where b.Isbn == isbn
                               select b).ToListAsync();
            if (found == null || found.Count() <= 0)
            {
                return NotFound();
            }
            Albums tBook = found[0];
            db.Album.Remove(tBook);
            await db.SaveChangesAsync();

            return Ok(tBook);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TBookExists(int id)
        {
            return db.Album.Count(e => e.Id == id) > 0;
        }
    }
}