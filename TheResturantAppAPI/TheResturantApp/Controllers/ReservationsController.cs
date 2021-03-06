﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TheResturantApp.Models;
using TheResturantApp.Models.DTO;

namespace TheResturantApp.Controllers
{
    public class ReservationsController : ApiController
    {
        private TRAContext db = new TRAContext();

        // GET: api/Reservations
        [Authorize]
        public IQueryable<ReservationDTO> GetReservations()
        {
            var reservation = (from d in db.Reservations
                              select new ReservationDTO()
                              {
                                  Comment = d.Comments,
                                  Date = d.Date,
                                  Time = d.Time,
                                  Email = d.Email,
                                  Guests = d.Guests,
                                  Name = d.Name,
                                  Phone = d.Phone
                              });

            return reservation;
        }

        //GET: api/getreservationbyphone
        [Authorize]
        [Route("api/getreservationbyphone")]
        public async Task<IEnumerable<ReservationDTO>> GetReservations(string uniqueId)
        {
            var list = await GetReservationList(uniqueId, 5);
            return list;
        }


        // GET: api/Reservations/00-000
        [Authorize]
        [ResponseType(typeof(ReservationDTO))]
        public async Task<IHttpActionResult> GetReservation(string phone)
        {
            var reservation = await db.Reservations.Select(c => new ReservationDTO()
            {
                Time = c.Time,
                Date = c.Date,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                Guests = c.Guests,
                Comment = c.Comments
            }).FirstOrDefaultAsync(b => b.Phone == phone);

            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        #region PUT/Post
        
        // PUT: api/Reservations/5
        [Authorize]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutReservation(decimal id, Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reservation.ID)
            {
                return BadRequest();
            }

            db.Entry(reservation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
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
        


        // POST: api/Reservations
        [Authorize]
        [ResponseType(typeof(Reservation))]
        public async Task<IHttpActionResult> PostReservation(Reservation res)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            var paramName = new SqlParameter("@pv_name", res.Name);
            var paramGuest = new SqlParameter("@pn_guest", res.Guests);
            var paramEmail = new SqlParameter("@pv_email", res.Email);
            var paramPhone = new SqlParameter("@pv_phone", res.Phone);
            var paramComment = new SqlParameter("@pv_comment", res.Comments);
            var paramDate = new SqlParameter("@pv_date", res.Date);
            var paramTime = new SqlParameter("@pv_time", res.Time);

            var id = new SqlParameter();
            id.ParameterName = "@pn_output_id";
            id.Direction = ParameterDirection.Output;
            id.SqlDbType = SqlDbType.Decimal;

            try
            {
                await db.Database.ExecuteSqlCommandAsync("Exec dbp_add_reservation  @pv_name, @pn_guest, @pv_email,@pv_phone, @pv_comment,@pv_date,@pv_time, @pn_output_id OUTPUT",
                paramName, paramGuest, paramEmail, 
                paramPhone, paramComment, 
                paramDate, paramTime, id).ContinueWith((result)=> {

                    var spResult = result.Result;
                    if (Convert.ToDecimal(id.Value) > 1)
                    {
                        res.ID = Convert.ToDecimal(id.Value);
                    }
                });
                
                await db.SaveChangesAsync();
            }
            catch(AggregateException ex)
            {
                var excep = ex.Message;
            }
            catch (DbUpdateException)
            {
                if (ReservationExists(res.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = res.ID }, res);
        }
        #endregion

        
        // POST: api/ReservationsDTO
        //http://localhost:32661/api/ReservationDTO
        [Authorize]
        [Route("api/ReservationDTO")]
        [ResponseType(typeof(ReservationDTO))]
        public async Task<IHttpActionResult> PostReservationDTO(ReservationDTO res)
        {
            var paramName = new SqlParameter("@pv_name", res.Name);
            var paramGuest = new SqlParameter("@pn_guest", res.Guests);
            var paramEmail = new SqlParameter("@pv_email", res.Email);
            var paramPhone = new SqlParameter("@pv_phone", res.Phone);
            var paramComment = new SqlParameter("@pv_comment", res.Comment);
            var paramDate = new SqlParameter("@pv_date", res.Date);
            var paramTime = new SqlParameter("@pv_time", res.Time);
            var param_custId = new SqlParameter("@pv_cust_id", res.CustomerId);

            var id = new SqlParameter();
            id.ParameterName = "@pn_output_id";
            id.Direction = ParameterDirection.Output;
            id.SqlDbType = SqlDbType.Decimal;

            try
            {
                await db.Database.ExecuteSqlCommandAsync("Exec dbp_add_reservation  @pv_name, @pn_guest, @pv_email,@pv_phone, @pv_comment,@pv_date,@pv_time, @pv_cust_id, @pn_output_id OUTPUT",
                paramName, paramGuest, paramEmail,
                paramPhone, paramComment,
                paramDate, paramTime, param_custId, id).ContinueWith((result) =>
                {

                    var spResult = result.Result;
                    if (Convert.ToDecimal(id.Value) > 1)
                    {
                        res.ID = Convert.ToDecimal(id.Value);
                    }
                });

                await db.SaveChangesAsync();
            }
            catch (AggregateException ex)
            {
                var excep = ex.Message;
                throw new ApplicationException(ex.Message);
            }
            catch (DbUpdateException)
            {

                if (ReservationExists(res.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            var reservation = new Reservation()
            {
                Name = res.Name,
                Phone = res.Phone,
                Comments =  res.Comment,
                Email =  res.Email,
                Guests = res.Guests,
                Time = res.Time,
                Date = res.Date,
                Active = "Y",
                InsertDateTime = DateTime.Now,
                InsertProcess = "Irfan",
                InsertUser = "Irfan",
                ID = res.ID
            };

            //var reservationlist = await GetReservationList(res.Phone, 5);
            return CreatedAtRoute("DefaultApi", new { controller = "Reservations", id = res.ID }, reservation);
        }
        
        // DELETE: api/Reservations/5
        [Authorize]
        [ResponseType(typeof(Reservation))]
        public async Task<IHttpActionResult> DeleteReservation(decimal id)
        {
            Reservation reservation = await db.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            db.Reservations.Remove(reservation);
            await db.SaveChangesAsync();

            return Ok(reservation);
        }

        #region HelperMethods
        private async Task<IEnumerable<ReservationDTO>> GetReservationList(string uniqueId, int noOfRows = 5)
        {
            var cust_uid = new Guid(uniqueId);
            var reservation = await db.Reservations
                .Where(c => c.CustomerId==cust_uid)
                .OrderByDescending(c => c.Date)
                .Select(c => new ReservationDTO()
                {
                    Time = c.Time,
                    Date = c.Date,
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.Phone,
                    Guests = c.Guests,
                    Comment = c.Comments
                }).Take(noOfRows).ToListAsync();

            return reservation;
        }

        #endregion



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReservationExists(decimal id)
        {
            return db.Reservations.Count(e => e.ID == id) > 0;
        }
    }
}