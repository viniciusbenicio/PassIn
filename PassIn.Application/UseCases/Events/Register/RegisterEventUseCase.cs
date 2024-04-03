using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entites;

namespace PassIn.Application.UseCases.Events.Register
{
    public class RegisterEventUseCase
    {
        public ResponseRegisterJson Execute(RequestEventJson request)
        {
            Validate(request);

            var dbContext = new PassInDbContext();

            var entity = new Event
            {
                Title = request.Title,
                Details = request.Details,
                Maximum_Attendees = request.MaximumAttendees,
                Slug = request.Title.ToLower().Replace(" ", "-") 
            };


            dbContext.Events.Add(entity);
            dbContext.SaveChanges();


            return new ResponseRegisterJson
            {
                Id = entity.Id
            };
        }

        private void Validate(RequestEventJson request)
        {
            if(request.MaximumAttendees <= 0) 
                throw new ErrorOnValidationException("The Maximum attendees is invalid.");
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ErrorOnValidationException("The title is invalid.");
            if (string.IsNullOrWhiteSpace(request.Details))
                throw new ErrorOnValidationException("The Details is invalid.");
        }
    }
}
