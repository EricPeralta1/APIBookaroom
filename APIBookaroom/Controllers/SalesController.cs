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
    public class SalesController : ApiController
    {
        private bookaroomEntities2 db = new bookaroomEntities2();

        // GET: api/Sales
        public IQueryable<Sales> GetSales()
        {
            return db.Sales;
        }

        // GET: api/Sales/5
        [ResponseType(typeof(Sales))]
        public async Task<IHttpActionResult> GetSales(int id)
        {
            Sales sales = await db.Sales.FindAsync(id);
            if (sales == null)
            {
                return NotFound();
            }

            return Ok(sales);
        }

        // GET: api/Sales/availableRooms/
        [HttpGet]
        [Route("api/Sales/availableRooms/{startDate}/{endDate}")]
        [ResponseType(typeof(object))]
        public IHttpActionResult GetAvailableRooms(DateTime startDate, DateTime endDate)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var eventosActivos = db.Esdeveniments
                .Where(e => e.active == 1)
                .Select(e => new
                {
                    event_id = e.event_id,
                    room_id = e.room_id,
                    start_date = e.start_date,
                    end_date = e.end_date
                })
                .ToList();

            var salas = db.Sales
                .Select(s => new
                {
                    room_id = s.room_id,
                    inventory_id = s.inventory_id,
                    dimensions = s.dimensions,
                    max_capacity = s.max_capacity,
                    num_seats = s.num_seats,
                    status = s.status
                })
                .ToList();

            var salasNoOcupadas = salas.Where(s => !eventosActivos.Any(e =>
     e.room_id == s.room_id &&
    ((e.start_date < endDate && e.end_date > startDate))
)).ToList();

            return Ok(salasNoOcupadas);
        }

        // PUT: api/Sales/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSales(int id, Sales sales)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sales.room_id)
            {
                return BadRequest();
            }

            db.Entry(sales).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesExists(id))
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

        // POST: api/Sales
        [ResponseType(typeof(Sales))]
        public async Task<IHttpActionResult> PostSales(Sales sales)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sales.Add(sales);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = sales.room_id }, sales);
        }

        // DELETE: api/Sales/5
        [ResponseType(typeof(Sales))]
        public async Task<IHttpActionResult> DeleteSales(int id)
        {
            Sales sales = await db.Sales.FindAsync(id);
            if (sales == null)
            {
                return NotFound();
            }

            db.Sales.Remove(sales);
            await db.SaveChangesAsync();

            return Ok(sales);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SalesExists(int id)
        {
            return db.Sales.Count(e => e.room_id == id) > 0;
        }
    }
}