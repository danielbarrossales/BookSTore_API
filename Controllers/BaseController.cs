

using BookStore_API.Contracts;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System;

namespace BookStore_API.Controllers
{
    public abstract class BaseController : Controller
    {
        protected static readonly string MESSAGE_ATTEMPTED_CALL = "Attempted Call.";
        protected static readonly string MESSAGE_SUCCESSFUL_CALL = "Successful.";
        protected static readonly string MESSAGE_ATTEMPTED_CALL_ID = "Attempted Call for id";
        protected static readonly string MESSAGE_FAILED_RETRIEVE_CALL = "Failed to retrieve record with id =";
        protected static readonly string MESSAGE_SUCCESS_RETRIEVE_CALL = "Successfully got record with id =";
        protected static readonly string MESSAGE_CREATE_ATTEMPED = "Create attempted.";
        protected static readonly string MESSAGE_EMPTY_SUBMISSION = "Empty request was submitted.";
        protected static readonly string MESSAGE_INCOMPLETE_DATA = "Data was incomplete.";
        protected static readonly string MESSAGE_CREATE_FAILED = "Creation failed.";
        protected static readonly string MESSAGE_CREATE_SUCCESSFUL = "Creation was successful.";
        protected static readonly string MESSAGE_UPDATE_ATTEMPTED = "Update attempted with id =";
        protected static readonly string MESSAGE_UPDATE_BAD_DATA = "Update failed with bad data - id =";
        protected static readonly string MESSAGE_UPDATE_FAILED = "Update failed";
        protected static readonly string MESSAGE_UPDATE_SUCCESSFUL = "Successfully updated record with id =";
        protected static readonly string MESSAGE_UPDATE_ID_NOT_FOUND = "Update failed, unable to find record with id =";
        protected static readonly string MESSAGE_DELETE_ATTEMPTED = "Delete attempted with id =";
        protected static readonly string MESSAGE_DELETE_BAD_DATA = "Delete failed with bad data - id =";
        protected static readonly string MESSAGE_DELETE_ID_NOT_FOUND = "Delete failed, unable to find record with id =";
        protected static readonly string MESSAGE_DELETE_SUCCESS = "Delete successfully deleted record with id =";
        protected static readonly string MESSAGE_DELETE_FAILED = "Delete failed with id =";


        
        protected readonly ILoggerService _logger;
        protected readonly IMapper _mapper;

        protected BaseController(ILoggerService logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        protected string GetControllerActionNames()
        {
            return $"{ControllerContext.ActionDescriptor.ControllerName}: " +
                $"{ControllerContext.ActionDescriptor.ActionName}";
        }

        protected string GenerateLogMessage(string location, string message)
        {
            return $"{location}: {message}";
        }

        protected string GenerateLogMessage(string location, Exception e)
        {
            return $"{location}: {e.Message} - {e.InnerException}";
        }

        protected string GenerateLogMessage(string location, string message, long id)
        {
            return $"{location}: {message} {id}.";
        }

        protected ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong on server-side, Please contact the Administrator");
        }
    }

    
}
