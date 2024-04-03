using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entites;
using System.Net.Mail;

namespace PassIn.Application.UseCases.Events.RegisterAttendee
{
    public class RegisterAttedeeOnEventUseCase
    {
        private readonly PassInDbContext _dbContext;

        public RegisterAttedeeOnEventUseCase()
        {
            _dbContext = new PassInDbContext();
        }

        public ResponseRegisterJson Execute(Guid eventId, RequestRegisterEventJson request)
        {
            Validate(eventId, request);

            var entity = new Attendee
            {
                Name = request.Name,
                Email = request.Email,
                Event_Id = eventId,
                Created_At = DateTime.Now,
            };

            _dbContext.Attendees.Add(entity);
            _dbContext.SaveChanges();

            return new ResponseRegisterJson
            {
                Id = eventId
            };
        }

        private void Validate(Guid eventId, RequestRegisterEventJson request)
        {
            var eventEntity = _dbContext.Events.Find(eventId);
            if (eventEntity is null)
                throw new NotFoundException("An event with this id does not exist");

            if (string.IsNullOrEmpty(request.Name))
                throw new ErrorOnValidationException("The name is invalid.");

            if (EmailIsValid(request.Email) is false)
                throw new ErrorOnValidationException("The e-mail is invalid.");

            var attendeeAlreadyRegistered = _dbContext.Attendees.Any(attendee => attendee.Email.Equals(request.Email));

            if (attendeeAlreadyRegistered)
                throw new ConflictException("You can not register twice on the same event");

            var attendeesForEvent = _dbContext.Attendees.Count(attendee => attendee.Event_Id == eventId);
            if (attendeesForEvent == eventEntity.Maximum_Attendees)
                throw new ErrorOnValidationException("There is no room for this event.");
        }

        private bool EmailIsValid(string email)
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
