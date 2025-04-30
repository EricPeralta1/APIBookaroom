using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using APIBookaroom.Models;

namespace APIBookaroom.Controllers
{
    public class EsdevenimentsController : ApiController
    {
        private bookaroomEntities2 db = new bookaroomEntities2();

        // GET: api/Esdeveniments
        public IQueryable<Esdeveniments> GetEsdeveniments()
        {
            db.Configuration.LazyLoadingEnabled = false;

            var baseUrl = "http://10.0.2.60/apibookaroom/Images/";


            var events = db.Esdeveniments
                .Select(u => new
                {
                    event_id = u.event_id,
                    room_id = u.room_id,
                    user_id = u.user_id,
                    capacity = u.capacity,
                    start_date = u.start_date,
                    end_date = u.end_date,
                    price = u.price,
                    name = u.name,
                    description = u.description,
                    event_image = baseUrl + u.event_image,
                    active = u.active
                })
                .ToList();

            return (IQueryable<Esdeveniments>)Ok(events);
        }

        // GET: api/Esdeveniments/5
        [ResponseType(typeof(Esdeveniments))]
        public async Task<IHttpActionResult> GetEsdeveniments(int id)
        {
            Esdeveniments esdeveniments = await db.Esdeveniments.FindAsync(id);
            if (esdeveniments == null)
            {
                return NotFound();
            }

            return Ok(esdeveniments);
        }

        // PUT: api/Esdeveniments/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEsdeveniments(int id, Esdeveniments esdeveniments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != esdeveniments.event_id)
            {
                return BadRequest();
            }

            db.Entry(esdeveniments).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EsdevenimentsExists(id))
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

        // POST: api/Esdeveniments
        [ResponseType(typeof(Esdeveniments))]
        public async Task<IHttpActionResult> PostEsdeveniments(Esdeveniments esdeveniments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Esdeveniments.Add(esdeveniments);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = esdeveniments.event_id }, esdeveniments);
        }

        // DELETE: api/Esdeveniments/5
        [ResponseType(typeof(Esdeveniments))]
        public async Task<IHttpActionResult> DeleteEsdeveniments(int id)
        {
            Esdeveniments esdeveniments = await db.Esdeveniments.FindAsync(id);
            if (esdeveniments == null)
            {
                return NotFound();
            }

            db.Esdeveniments.Remove(esdeveniments);
            await db.SaveChangesAsync();

            return Ok(esdeveniments);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EsdevenimentsExists(int id)
        {
            return db.Esdeveniments.Count(e => e.event_id == id) > 0;
        }
    }
}