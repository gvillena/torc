using System.Net;
using Microsoft.AspNetCore.Mvc;
using Torc.BuildingBlocks.EventBus.Absractions;
using Torc.Services.BookLibrary.API.IntegrationEvents;
using Torc.Services.BookLibrary.API.ViewModels;
using Torc.Services.BookLibrary.Domain.Entities;
using Torc.Services.BookLibrary.Infrastructure.Repositories;

namespace BookLibrary.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookLibraryController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IEventBus _eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookLibraryController"/> class.
        /// </summary>
        /// <param name="bookRepository">The book repository.</param>
        public BookLibraryController(IBookRepository bookRepository, IEventBus eventBus)
        {
            _bookRepository = bookRepository;
            _eventBus = eventBus;
        }

        /// <summary>
        /// Retrieves all books.
        /// </summary>
        /// <returns>The list of all books.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BookViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _bookRepository.GetAllBooksAsync();
            var booksViewModel = books.Select(book => new BookViewModelBuilder()
                .WithBookId(book.BookId)
                .WithTitle(book.Title)
                .WithAuthor($"{book.LastName}, {book.FirstName}")
                .WithPublisher("New York Times")
                .WithType(book.Type ?? string.Empty)
                .WithCategory(book.Category ?? string.Empty)
                .WithIsbn(book.ISBN ?? string.Empty)
                .WithAvailableCopies($"{book.TotalCopies - book.CopiesInUse}/{book.TotalCopies}")
                .Build()).ToList();
            return Ok(booksViewModel);
        }

        /// <summary>
        /// Retrieves a book by its ID.
        /// </summary>
        /// <param name="id">The ID of the book to retrieve.</param>
        /// <returns>The book with the specified ID, or NotFound if not found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BookViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<BookViewModel>> GetBook(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var bookViewModel = new BookViewModelBuilder()
                .WithBookId(book.BookId)
                .WithTitle(book.Title)
                .WithAuthor($"{book.LastName}, {book.FirstName}")
                .WithPublisher("New York Times")
                .WithType(book.Type ?? string.Empty)
                .WithCategory(book.Category ?? string.Empty)
                .WithIsbn(book.ISBN ?? string.Empty)
                .WithAvailableCopies($"{book.TotalCopies - book.CopiesInUse}/{book.TotalCopies}")
                .Build();

            return bookViewModel;
        }

        /// <summary>
        /// Add a new book.
        /// </summary>
        /// <param name="book">The book to add.</param>
        /// <returns>The added book.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Book), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {
            await _bookRepository.AddBookAsync(book);

            var message = new BookAddedIntegrationEvent(book.Title, $"{book.LastName}, {book.FirstName}");
            await _eventBus.Publish(message);

            return CreatedAtAction(nameof(GetBook), new { id = book.BookId }, book);
        }

        /// <summary>
        /// Searches for books based on title, type, and category.
        /// </summary>
        /// <param name="title">The title of the book to search for.</param>
        /// <param name="type">The type of the book to search for.</param>
        /// <param name="category">The category of the book to search for.</param>
        /// <returns>The list of matching books.</returns>
        [HttpGet("Search")]
        [ProducesResponseType(typeof(IEnumerable<BookViewModel>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<BookViewModel>>> SearchBooks([FromQuery] string? title, [FromQuery] string? type, [FromQuery] string? category)
        {
            var books = await _bookRepository.FindByAsync(b =>
                (string.IsNullOrEmpty(title) || b.Title.Contains(title)) &&
                (string.IsNullOrEmpty(type) || b.Type.Contains(type)) &&
                (string.IsNullOrEmpty(category) || b.Category.Contains(category)));

            var booksViewModel = books.Select(book => new BookViewModelBuilder()
                .WithBookId(book.BookId)
                .WithTitle(book.Title)
                .WithAuthor($"{book.LastName}, {book.FirstName}")
                .WithPublisher("New York Times")
                .WithType(book.Type ?? string.Empty)
                .WithCategory(book.Category ?? string.Empty)
                .WithIsbn(book.ISBN ?? string.Empty)
                .WithAvailableCopies($"{book.TotalCopies - book.CopiesInUse}/{book.TotalCopies}")
                .Build()).ToList();

            return Ok(booksViewModel);
        }
    }
}
