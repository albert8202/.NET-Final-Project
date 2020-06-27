using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MyModels;
using Params;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Web.Http.Cors;


namespace MyMall.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RecordController : ApiController
    {
        private MySqlContext db = new MySqlContext();

        // GET: api/History
        public IQueryable<Records> GetTHistories()
        {
            return db.Record;
        }

        // GET: api/History/5
        [ResponseType(typeof(Records))]
        public async Task<IHttpActionResult> GetTHistory(int id)
        {
            Records tHistory = await db.Record.FindAsync(id);
            if (tHistory == null)
            {
                return NotFound();
            }

            return Ok(tHistory);
        }

        // PUT: api/History/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTHistory(int id, Records tHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tHistory.Id)
            {
                return BadRequest();
            }

            db.Entry(tHistory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!THistoryExists(id))
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

        // POST: api/History
        [ResponseType(typeof(Records))]
        public async Task<IHttpActionResult> PostTHistory(Records tHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Record.Add(tHistory);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tHistory.Id }, tHistory);
        }

        // DELETE: api/History/5
        [ResponseType(typeof(Records))]
        public async Task<IHttpActionResult> DeleteTHistory(int id)
        {
            Records tHistory = await db.Record.FindAsync(id);
            if (tHistory == null)
            {
                return NotFound();
            }

            db.Record.Remove(tHistory);
            await db.SaveChangesAsync();

            return Ok(tHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool THistoryExists(int id)
        {
            return db.Record.Count(e => e.Id == id) > 0;
        }
    }
}