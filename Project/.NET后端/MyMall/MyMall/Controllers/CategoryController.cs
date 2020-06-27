using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;
using MyModels;
using Microsoft.EntityFrameworkCore;
using System.Web.Http.Cors;

namespace MyMall.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoryController : ApiController
    {
        private MySqlContext db = new MySqlContext();

        // GET: api/Category
        public IQueryable<CategoryNames> GetTCategories()
        {
            return db.CategoryName;
        }

        // GET: api/Category/5
        [ResponseType(typeof(CategoryNames))]
        public async Task<IHttpActionResult> GetTCategory(int id)
        {
            CategoryNames tCategory = await db.CategoryName.FindAsync(id);
            if (tCategory == null)
            {
                return NotFound();
            }

            return Ok(tCategory);
        }

        // PUT: api/Category/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTCategory(int id, CategoryNames tCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tCategory.Id)
            {
                return BadRequest();
            }

            db.Entry(tCategory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TCategoryExists(id))
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

        // POST: api/Category
        [ResponseType(typeof(CategoryNames))]
        public async Task<IHttpActionResult> PostTCategory(CategoryNames tCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CategoryName.Add(tCategory);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tCategory.Id }, tCategory);
        }

        // DELETE: api/Category/5
        [ResponseType(typeof(CategoryNames))]
        public async Task<IHttpActionResult> DeleteTCategory(int id)
        {
            CategoryNames tCategory = await db.CategoryName.FindAsync(id);
            if (tCategory == null)
            {
                return NotFound();
            }

            db.CategoryName.Remove(tCategory);
            await db.SaveChangesAsync();

            return Ok(tCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TCategoryExists(int id)
        {
            return db.CategoryName.Count(e => e.Id == id) > 0;
        }
    }
}