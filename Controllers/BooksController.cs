using AutoMapper;
using BookStore_API.Contracts;
using BookStore_API.Database.Entities;
using BookStore_API.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore_API.Controllers
{

    /// <summary>
    /// Endpoint used to interact with the books in the Book Store database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class BooksController : BaseController
    {
        protected IBookRepository _bookRepository;
        protected IAuthorRepository _authorRepository;
        public BooksController(IBookRepository bookRepository, IAuthorRepository authorRepository, ILoggerService logger, IMapper mapper) : base(logger, mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }
        
        /// <summary>
        /// Get All Books
        /// </summary>
        /// <returns>List of Authors</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks()
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo(GenerateLogMessage(location, MESSAGE_ATTEMPTED_CALL));
                var books = await _bookRepository.FindAll();
                var response = _mapper.Map<IList<BookDto>>(books);
                _logger.LogInfo(GenerateLogMessage(location, MESSAGE_SUCCESSFUL_CALL));
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError(GenerateLogMessage(location, e));
            }
        }

        /// <summary>
        /// Get Book by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The books record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBook(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo(GenerateLogMessage(location, MESSAGE_ATTEMPTED_CALL_ID, id));
                var book = await _bookRepository.FindById(id);
                if (book == null)
                {
                    _logger.LogWarn(GenerateLogMessage(location, MESSAGE_FAILED_RETRIEVE_CALL, id));
                    return NotFound();
                }
                var response = _mapper.Map<BookDto>(book);
                _logger.LogInfo(GenerateLogMessage(location, MESSAGE_SUCCESS_RETRIEVE_CALL, id));
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError(GenerateLogMessage(location, e));
            }
        }

        /// <summary>
        /// Creates a new Book 
        /// </summary>
        /// <param name="bookDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateBookDto bookDto)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo(GenerateLogMessage(location, MESSAGE_CREATE_ATTEMPED));
                if (bookDto == null)
                {
                    _logger.LogWarn(GenerateLogMessage(location, MESSAGE_EMPTY_SUBMISSION));
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarn(GenerateLogMessage(location, MESSAGE_INCOMPLETE_DATA));
                    return BadRequest(ModelState);
                }

                if (! await _authorRepository.IsInDatabase(bookDto.AuthorId))
                {
                    _logger.LogWarn(GenerateLogMessage(location, MESSAGE_CREATE_FAILED));
                    return BadRequest(ModelState);
                }

                var book = _mapper.Map<Book>(bookDto);
                var isSuccess = await _bookRepository.Create(book);

                if (!isSuccess)
                {
                    return InternalError(GenerateLogMessage(location, MESSAGE_CREATE_FAILED));
                }

                _logger.LogInfo(GenerateLogMessage(location, MESSAGE_CREATE_SUCCESSFUL));
                return Created("Create", new { book });
            }
            catch (Exception e)
            {
                return InternalError(GenerateLogMessage(location, e));
            }
        }

        /// <summary>
        /// Updates Book
        /// </summary>
        /// <param name="id">Id of book to be updated</param>
        /// <param name="bookDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateBookDto bookDto)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo(GenerateLogMessage(location, MESSAGE_UPDATE_ATTEMPTED, id));
                if (id < 0 || bookDto == null)
                {
                    _logger.LogError(GenerateLogMessage(location, MESSAGE_UPDATE_BAD_DATA, id));
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError(GenerateLogMessage(location, MESSAGE_UPDATE_BAD_DATA, id));
                    return BadRequest();
                }

                if (!await _bookRepository.IsInDatabase(id))
                {
                    _logger.LogError(GenerateLogMessage(location, MESSAGE_UPDATE_ID_NOT_FOUND, id));
                    return BadRequest();
                }

                //var book = _mapper.Map<Book>(bookDto);
                // Update values passed to objects dto, null properties are replaced by the current value of each property
                var book = await _bookRepository.FindById(id);
                book.Image = bookDto.Image ?? book.Image;
                book.Title = bookDto.Title ?? book.Title;
                book.Price = bookDto.Price ?? book.Price;
                book.Summary = bookDto.Summary ?? book.Summary;

                var isSuccess = await _bookRepository.Update(book);

                if (isSuccess)
                {
                    _logger.LogInfo(GenerateLogMessage(location, MESSAGE_UPDATE_SUCCESSFUL, id));
                    return NoContent();
                }

                _logger.LogError(GenerateLogMessage(location, MESSAGE_UPDATE_FAILED));
                return BadRequest();
            }
            catch (Exception e)
            {
                return InternalError(GenerateLogMessage(location, e));
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(long id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo(GenerateLogMessage(location, MESSAGE_DELETE_ATTEMPTED, id));
                if (id < 0)
                {
                    _logger.LogError(GenerateLogMessage(location, MESSAGE_DELETE_BAD_DATA, id));
                    return BadRequest();
                }

                var book = await _bookRepository.FindById(id);
                
                if (book == null)
                {
                    _logger.LogError(GenerateLogMessage(location, MESSAGE_DELETE_ID_NOT_FOUND, id));
                    return NotFound();
                }

                var isSuccess = await _bookRepository.Delete(book);

                if (isSuccess)
                {
                    _logger.LogInfo(GenerateLogMessage(location, MESSAGE_DELETE_SUCCESS));
                    return NoContent();
                }

                _logger.LogError(GenerateLogMessage(location, MESSAGE_DELETE_FAILED, id));
                return BadRequest();
            }
            catch (Exception e)
            {
                return InternalError(GenerateLogMessage(location, e));
            }
        }

    }

    


}
