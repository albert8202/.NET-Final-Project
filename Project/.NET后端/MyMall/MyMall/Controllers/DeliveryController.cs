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
using System.Web.Http.Cors;

namespace MyMall.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DeliveryController : ApiController
    {
        private MySqlContext db = new MySqlContext();

        // GET: api/Delivery
        public IQueryable<Deliverys> GetTDeliveries()
        {
            return db.Delivery;
        }

        // GET: api/Delivery/5
        [ResponseType(typeof(Deliverys))]
        public async Task<IHttpActionResult> GetTDelivery(int id)
        {
            Deliverys tDelivery = await db.Delivery.FindAsync(id);
            if (tDelivery == null)
            {
                return NotFound();
            }

            return Ok(tDelivery);
        }

        // PUT: api/Delivery/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTDelivery(int id, Deliverys tDelivery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tDelivery.Id)
            {
                return BadRequest();
            }

            db.Entry(tDelivery).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TDeliveryExists(id))
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

        // POST: api/Delivery
        [ResponseType(typeof(Deliverys))]
        public async Task<IHttpActionResult> PostTDelivery(Deliverys tDelivery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Delivery.Add(tDelivery);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tDelivery.Id }, tDelivery);
        }

        // DELETE: api/Delivery/5
        [ResponseType(typeof(Deliverys))]
        public async Task<IHttpActionResult> DeleteTDelivery(int id)
        {
            Deliverys tDelivery = await db.Delivery.FindAsync(id);
            if (tDelivery == null)
            {
                return NotFound();
            }

            db.Delivery.Remove(tDelivery);
            await db.SaveChangesAsync();

            return Ok(tDelivery);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TDeliveryExists(int id)
        {
            return db.Delivery.Count(e => e.Id == id) > 0;
        }
    }
}