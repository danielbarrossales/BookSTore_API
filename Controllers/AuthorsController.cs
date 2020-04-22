using AutoMapper;
using BookStore_API.Contracts;
using BookStore_API.Database.Entities;
using BookStore_API.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore_API.Controllers
{
    /// <summary>
    /// Endpoint used to interact with the authors in the Book Store database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepository authorRepository,
            ILoggerService logger, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// Get All Authors
        /// </summary>
        /// <returns>List of Authors</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                _logger.LogInfo("Attempted Get All Authors");
                var authors = await _authorRepository.FindAll();
                var response = _mapper.Map<IList<AuthorDto>>(authors);
                _logger.LogInfo("Successfuly got all Authors");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Get Author by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The authors record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthor(int id)
        {
            try
            {
                _logger.LogInfo($"Attempted Get Author with id: {id}");
                var author = await _authorRepository.FindById(id);
                if (author == null)
                {
                    _logger.LogWarn($"Author with id = {id} was not found");
                    return NotFound();
                }
                var response = _mapper.Map<AuthorDto>(author);
                _logger.LogInfo($"Successfully got Author with id: {id}");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Create An Author 
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto authorDto)
        {
            try
            {
                _logger.LogInfo("Author submission attempted");
                if (authorDto == null)
                {
                    _logger.LogWarn("Empty request was submitted");
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarn("Author data was incomplete");
                    return BadRequest(ModelState);
                }

                var author = _mapper.Map<Author>(authorDto);
                var isSuccess = await _authorRepository.Create(author);

                if (!isSuccess)
                {
                    InternalError("Author creation failed");
                }

                _logger.LogInfo($"Author created with id = {author.Id}");
                return Created("Create", new { author });
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Updates Author
        /// </summary>
        /// <param name="id">Id of author to be updated</param>
        /// <param name="authorDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAuthor(long id, [FromBody] CreateAuthorDto authorDto)
        {
            try
            {
                _logger.LogInfo("Author Update Attempted");
                if (id < 0 || authorDto == null)
                {
                    _logger.LogError("Author update failed with bad data");
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Author update failed with bad data");
                    return BadRequest();
                }

                if (!await _authorRepository.IsInDatabase(id))
                {
                    _logger.LogError("Attempted to update author that dosn't exists on the database.");
                    return BadRequest();
                }

                var author = _mapper.Map<Author>(authorDto);
                author.Id = id;
                var isSuccess = await _authorRepository.Update(author);

                if (isSuccess)
                {
                    _logger.LogInfo($"Author update with id = {id} was successfull");
                    return NoContent();
                }

                _logger.LogError("Author update failed");
                return BadRequest();
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAuthor(long id)
        {
            try
            {
                _logger.LogInfo("Author Delete Attempted");
                if (id < 0)
                {
                    _logger.LogError("Author delete failed with bad data");
                    return BadRequest();
                }

                var author = await _authorRepository.FindById(id);
                
                if (author == null)
                {
                    _logger.LogError("Attempted to delete author that does not exist on database");
                    return NotFound();
                }

                var isSuccess = await _authorRepository.Delete(author);

                if (isSuccess)
                {
                    _logger.LogInfo($"Author delete with id = {id} was successfull");
                    return NoContent();
                }

                _logger.LogError("Author delete failed");
                return BadRequest();
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong on server-side, Please contact the Administrator");
        }
    }

    
}
