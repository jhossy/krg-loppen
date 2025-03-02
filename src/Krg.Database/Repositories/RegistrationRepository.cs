using Krg.Database.Interfaces;
using Krg.Database.Models;
namespace Krg.Database.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly KrgContext _context;
        public RegistrationRepository(KrgContext context)
        {
            _context = context;
        }

        public void AddRegistration(EventRegistration registration)
        {
            _context.EventRegistrations.Add(registration);
        }

        public void UpdateRegistration(EventRegistration registration)
        {
            EventRegistration eventRegistration = GetById(registration.Id);

            if (eventRegistration != null)
            {
                eventRegistration.BringsTrailer = registration.BringsTrailer;
                eventRegistration.Department = registration.Department;
                eventRegistration.EventDate = registration.EventDate;
                eventRegistration.IsCancelled = registration.IsCancelled;
                eventRegistration.Name = registration.Name;
                eventRegistration.NoOfAdults = registration.NoOfAdults;
                eventRegistration.NoOfChildren = registration.NoOfChildren;
                eventRegistration.PhoneNo = registration.PhoneNo;
                eventRegistration.ShowName = registration.ShowName;
                eventRegistration.UmbracoEventNodeId = registration.UmbracoEventNodeId;
                eventRegistration.UpdateTimeUtc = registration.UpdateTimeUtc;

                _context.EventRegistrations.Update(eventRegistration);
            }
        }

        public EventRegistration GetById(int id)
        {
            var registration = _context.EventRegistrations.Single(x => x.Id == id);

            return registration;
        }
        
        public List<EventRegistration> GetAllRegistrations(DateRange dateRange)
        {
            var registrations = _context.EventRegistrations
                .Where(x => DateOnly.FromDateTime(x.EventDate) >= dateRange.StartDate && DateOnly.FromDateTime(x.EventDate) <= dateRange.EndDate)
                .ToList();

            return registrations;
        }

        public List<EventRegistration> GetNonDeletedRegistrations(DateRange dateRange)
        {
            var registrations = _context.EventRegistrations
                .Where(x => DateOnly.FromDateTime(x.EventDate) >= dateRange.StartDate && DateOnly.FromDateTime(x.EventDate) <= dateRange.EndDate && x.IsCancelled == false)
                .ToList();

            return registrations;
        }

        public void RemoveRegistration(int id)
        {
            EventRegistration eventFound = GetById(id);

            if (eventFound == null) return;

            eventFound.IsCancelled = true;

            UpdateRegistration(eventFound);
        }
    }
}
