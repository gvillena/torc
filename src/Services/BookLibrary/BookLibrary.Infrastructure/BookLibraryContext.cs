using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Torc.Services.BookLibrary.Domain.Entities;
using Torc.Services.BookLibrary.Infrastructure.EntityTypeConfigurations;

namespace Torc.Services.BookLibrary.Infrastructure
{
    /// <summary>
    /// Represents the database context for the book library.
    /// </summary>
    public class BookLibraryContext : DbContext
    {
        /// <summary>
        /// Gets or sets the DbSet for the books in the library.
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookLibraryContext"/> class with the specified options.
        /// </summary>
        /// <param name="options">The options for configuring the context.</param>
        public BookLibraryContext(DbContextOptions<BookLibraryContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Configures the model for the book library.
        /// </summary>
        /// <param name="modelBuilder">The model builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookEntityTypeConfiguration());
        }
    }

}
