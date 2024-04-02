﻿using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using System.Xml.XPath;

namespace PassIn.Application.UseCases.Events.GetById
{
    public class GetEventByIdUseCase
    {
        public ResponseEventJson Execute(Guid id)
        {
            var dbContext = new PassInDbContext();
            
            var entity =  dbContext.Events.Find(id);
            if(entity is null)
                throw new PassInException("An event with this id dont exist.");

            return new ResponseEventJson
            {
                Id = entity.Id,
                Title = entity.Title,
                Details = entity.Details,
                MaximumAttendees = entity.Maximum_Attendees,
                AttendeesAmount = -1
            };
        }
    }
}
