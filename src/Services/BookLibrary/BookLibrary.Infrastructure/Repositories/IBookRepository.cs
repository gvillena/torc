using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Torc.Services.BookLibrary.Domain.Entities;

namespace Torc.Services.BookLibrary.Infrastructure.Repositories
{
    /// <summary>
    /// Interface for interacting with the book repository.
    /// </summary>
    public interface IBookRepository
    {
        /// <summary>
        /// Retrieves all books from the database.
        /// </summary>
        /// <returns>A list of all books.</returns>
        Task<List<Book>> GetAllBooksAsync();

        /// <summary>
        /// Retrieves a book by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the book to retrieve.</param>
        /// <returns>The book with the specified ID, or null if not found.</returns>
        Task<Book> GetBookByIdAsync(int id);

        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        /// <param name="book">The book to add.</param>
        Task AddBookAsync(Book book);

        /// <summary>
        /// Updates an existing book in the database.
        /// </summary>
        /// <param name="book">The book to update.</param>
        Task UpdateBookAsync(Book book);

        /// <summary>
        /// Deletes a book from the database.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        Task DeleteBookAsync(int id);

        /// <summary>
        /// Finds books in the database that match the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter books.</param>
        /// <returns>A list of books that match the predicate.</returns>
        Task<List<Book>> FindByAsync(Expression<Func<Book, bool>> predicate);
    }
}
