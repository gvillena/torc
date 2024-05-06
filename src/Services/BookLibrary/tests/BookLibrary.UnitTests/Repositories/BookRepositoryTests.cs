using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Torc.Services.BookLibrary.Domain.Entities;
using Torc.Services.BookLibrary.Infrastructure;
using Torc.Services.BookLibrary.Infrastructure.Repositories;
using Torc.Services.BookLibrary.UnitTests.Builders;
using Xunit;

namespace Torc.Services.BookLibrary.UnitTests.Repositories
{
    public class BookRepositoryTests
    {
        private readonly BookRepository _bookRepository;
        private readonly BookLibraryContext _context;

        public BookRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BookLibraryContext>()
                .UseInMemoryDatabase(databaseName: "Test_Database")
                .Options;
            _context = new BookLibraryContext(options);
            _bookRepository = new BookRepository(_context);
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnsAllBooks()
        {
            // Arrange
            var book1 = new BookBuilder()
                .WithTitle("Book 1")
                .Build();

            var book2 = new BookBuilder()
                .WithTitle("Book 2")
                .Build();

            var book3 = new BookBuilder()
                .WithTitle("Book 3")
                .Build();

            await _context.AddRangeAsync(book1, book2, book3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _bookRepository.GetAllBooksAsync();

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsBookById()
        {
            // Arrange            
            var book = new BookBuilder()                
                .WithTitle("Book 1")
                .Build();
            
            await _context.AddAsync(book);
            await _context.SaveChangesAsync();

            // Act
            var result = await _bookRepository.GetBookByIdAsync(book.BookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(book.BookId, result.BookId);
            Assert.Equal("Book 1", result.Title);
        }

        [Fact]
        public async Task AddBookAsync_AddsNewBook()
        {
            // Arrange
            var book = new BookBuilder()
                .WithTitle("New Book")
                .Build();

            // Act
            await _bookRepository.AddBookAsync(book);

            // Assert
            Assert.NotEqual(0, book.BookId);
            var result = await _context.Books.FindAsync(book.BookId);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateBookAsync_UpdatesExistingBook()
        {
            // Arrange
            var book = new BookBuilder()
                .WithTitle("Updated Book")
                .Build();

            await _context.AddAsync(book);
            await _context.SaveChangesAsync();

            // Act
            await _bookRepository.UpdateBookAsync(book);

            // Assert
            var result = await _context.Books.FindAsync(book.BookId);
            Assert.Equal("Updated Book", result.Title);
        }

        [Fact]
        public async Task DeleteBookAsync_DeletesBook()
        {
            // Arrange
            var book = new BookBuilder()
                .WithTitle("Book 1")
                .Build();

            await _context.AddAsync(book);
            await _context.SaveChangesAsync();

            // Act
            await _bookRepository.DeleteBookAsync(book.BookId);

            // Assert
            var result = await _context.Books.FindAsync(book.BookId);
            Assert.Null(result);
        }

        [Fact]
        public async Task FindByAsync_ReturnsMatchingBooks()
        {
            // Arrange
            var book1 = new BookBuilder()
                .WithTitle("Book 1")
                .WithCategory("Fiction")
                .Build();

            var book2 = new BookBuilder()
                .WithTitle("Book 2")
                .WithCategory("Non-Fiction")
                .Build();

            var book3 = new BookBuilder()
                .WithTitle("Book 3")
                .WithCategory("Fiction")
                .Build();

            await _context.AddRangeAsync(book1, book2, book3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _bookRepository.FindByAsync(b => b.Category == "Fiction");

            // Assert
            Assert.Collection(result,
                b => Assert.Equal("Book 1", b.Title),
                b => Assert.Equal("Book 3", b.Title)
            );
        }
    }
}
