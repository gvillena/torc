using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torc.Services.BookLibrary.Domain.Entities
{
    /// <summary>
    /// Represents a book in the library.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Book"/> class.
        /// Protected constructor for inheritance.
        /// </summary>
        protected Book()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Book"/> class with specified properties.
        /// </summary>
        /// <param name="title">The title of the book. Must not be null or empty.</param>
        /// <param name="firstName">The first name of the author. Must not be null or empty.</param>
        /// <param name="lastName">The last name of the author. Must not be null or empty.</param>
        /// <param name="type">The type or genre of the book. Can be null.</param>
        /// <param name="isbn">The International Standard Book Number (ISBN) of the book. Can be null.</param>
        /// <param name="category">The category or classification of the book. Can be null.</param>
        /// <exception cref="ArgumentException">Thrown when the title, first name, or last name is null or empty.</exception>
        public Book(string title, string firstName, string lastName, string? type, string? isbn, string? category) : this()
        {
            // Perform null or empty checks for required parameters
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title must not be null or empty.", nameof(title));
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("First name must not be null or empty.", nameof(firstName));
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Last name must not be null or empty.", nameof(lastName));
            }
            Title = title;
            FirstName = firstName;
            LastName = lastName;
            Type = type;
            ISBN = isbn;
            Category = category;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the book.
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// Gets or sets the title of the book.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the first name of the author.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the author.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the total number of copies of the book in the library.
        /// </summary>
        public int TotalCopies { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of copies currently checked out or in use.
        /// </summary>
        public int CopiesInUse { get; set; } = 0;

        /// <summary>
        /// Gets or sets the type or genre of the book.
        /// Nullable.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the International Standard Book Number (ISBN) of the book.
        /// Nullable.
        /// </summary>
        public string? ISBN { get; set; }

        /// <summary>
        /// Gets or sets the category or classification of the book.
        /// Nullable.
        /// </summary>
        public string? Category { get; set; }
    }
}
