using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApi4.Models;
using WebApi4.Models.DTO.Requests;
using WebApi4.Models.DTO.Responses;

namespace WebApi4.Services
{
    public class DataBaseServise : IDataBaseService
    {
        private readonly _2019SBDContext _context;

        public DataBaseServise(_2019SBDContext context)
        {
            _context = context;
        }

        public async Task<ICollection<TripsResponseDto>> GetTrips()
        {
            return await _context.Trips.Select(x => new TripsResponseDto
            {
                Name = x.Name,
                Description = x.Description,
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                MaxPeople = x.MaxPeople,
                Countries = x.CountryTrips.Select(z => z.IdCountryNavigation).Select(c => new
                {
                    Name = c.Name
                }).ToList(),
                Clients = x.ClientTrips.Select(v => v.IdClientNavigation).Select(b => new
                {
                    FirstName = b.FirstName,
                    LastName = b.LastName
                }).ToList()
            }).OrderBy(trip => trip.DateFrom).ToListAsync();
        }

        public async Task<HttpStatusCodeResult> DeleteClientData(int IdClient) {
            var clientExist = await _context.Clients.Where(x => x.IdClient == IdClient).CountAsync();
            var haveTrips = await _context.ClientTrips.Where(client => client.IdClient == IdClient).CountAsync();
            if (clientExist == 0)
            {
                return new HttpStatusCodeResult(404, "Brak klienta o podanym id w bazie");
            }
            else
            {
                if (haveTrips >= 1)
                {
                    return new HttpStatusCodeResult(400, "Klient o podanym id posiada wycieczke i nie moze zostac usuniety");
                }
                else
                {
                    var client = await _context.Clients.Where(c => c.IdClient == IdClient).FirstAsync();
                    _context.Clients.Remove(client);
                    await _context.SaveChangesAsync();
                    return new HttpStatusCodeResult(200, "Klient o podanym id został usuniety");
                }
            }
        }

        public async Task<HttpStatusCodeResult> AssignAClientToTrip(TripsRequestDto tripsRequestDto, int IdTrip) {
            var clientExist = await _context.Clients.Where(c => c.Pesel == tripsRequestDto.Pesel).CountAsync();
            var tripExist = await _context.Trips.Where(x => x.IdTrip == IdTrip).CountAsync();
            if (clientExist == 0)
            {
                _context.Clients.Add(new Client
                {
                    IdClient = _context.Clients.Select(x => x.IdClient).Max() + 1,
                    FirstName = tripsRequestDto.FirstName,
                    LastName = tripsRequestDto.LastName,
                    Email = tripsRequestDto.Email,
                    Telephone = tripsRequestDto.Telephone,
                    Pesel = tripsRequestDto.Pesel
                });
                if (tripExist == 0)
                {
                    return new HttpStatusCodeResult(404,"Wycieczka o podanym id nie istnieje");
                }
                else
                {
                    await _context.SaveChangesAsync();
                    _context.ClientTrips.Add(new ClientTrip
                    {
                        IdClient = await _context.Clients.Where(x => x.Pesel == tripsRequestDto.Pesel).Select(x => x.IdClient).FirstAsync(),
                        IdTrip = IdTrip,
                        RegisteredAt = DateTime.Now,
                        PaymentDate = tripsRequestDto.PaymentDate
                    });
                    await _context.SaveChangesAsync();
                    return new HttpStatusCodeResult(200, "Dodano nowego klienta i wpisano go na wycieczke");
                }
            }
            else
            {
                var id = await _context.Clients.Where(x => x.Pesel == tripsRequestDto.Pesel).Select(x => x.IdClient).FirstAsync();
                var clientHasTrip = await _context.ClientTrips.Where(x => x.IdTrip == IdTrip).Where(x => x.IdClient == id).CountAsync();
                if (clientHasTrip > 0)
                {
                    return new HttpStatusCodeResult(400, "Klient o podananym peselu jest juz zapisany na wycieczke");
                }
                else
                {
                    if (tripExist == 0)
                    {
                        return new HttpStatusCodeResult(404,"Wycieczka o podanym id nie istnieje");
                    }
                    else
                    {
                        _context.ClientTrips.Add(new ClientTrip
                        {
                            IdClient = await _context.Clients.Where(x => x.Pesel == tripsRequestDto.Pesel).Select(x => x.IdClient).FirstAsync(),
                            IdTrip = IdTrip,
                            RegisteredAt = DateTime.Now,
                            PaymentDate = tripsRequestDto.PaymentDate
                        });
                        await _context.SaveChangesAsync();
                        return new HttpStatusCodeResult(200,"Wpisano klienta na wycieczke");
                    }
                }
            }
        }
    }
}
