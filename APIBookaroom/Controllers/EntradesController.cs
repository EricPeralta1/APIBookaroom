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
    public class EntradesController : ApiController
    {
        private bookaroomEntities2 db = new bookaroomEntities2();

        // GET: api/Entrades
        public IQueryable<Entrades> GetEntrades()
        {
            return db.Entrades;
        }

        // GET: api/Entrades/5
        [ResponseType(typeof(Entrades))]
        public async Task<IHttpActionResult> GetEntrades(int id)
        {
            Entrades entrades = await db.Entrades.FindAsync(id);
            if (entrades == null)
            {
                return NotFound();
            }

            return Ok(entrades);
        }

        // PUT: api/Entrades/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEntrades(int id, Entrades entrades)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entrades.ticket_id)
            {
                return BadRequest();
            }

            db.Entry(entrades).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntradesExists(id))
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

        // POST: api/Entrades
        [ResponseType(typeof(Entrades))]
        public async Task<IHttpActionResult> PostEntrades(Entrades entrades)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entrades.Add(entrades);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = entrades.ticket_id }, entrades);
        }

        // DELETE: api/Entrades/5
        [ResponseType(typeof(Entrades))]
        public async Task<IHttpActionResult> DeleteEntrades(int id)
        {
            Entrades entrades = await db.Entrades.FindAsync(id);
            if (entrades == null)
            {
                return NotFound();
            }

            db.Entrades.Remove(entrades);
            await db.SaveChangesAsync();

            return Ok(entrades);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EntradesExists(int id)
        {
            return db.Entrades.Count(e => e.ticket_id == id) > 0;
        }
    }
}