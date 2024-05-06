using Torc.Services.BookLibrary.Domain.Entities;

namespace Torc.Services.BookLibrary.UnitTests.Builders
{
    public class BookBuilder
    {
        private string _title = "Book Title";
        private string _firstName = "John";
        private string _lastName = "Doe";
        private int _totalCopies = 10;
        private int _copiesInUse = 5;
        private string _type = "Hardcover";
        private string _isbn = "1234567890";
        private string _category = "Fiction";

        public BookBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public BookBuilder WithFirstName(string firstName)
        {
            _firstName = firstName;
            return this;
        }

        public BookBuilder WithLastName(string lastName)
        {
            _lastName = lastName;
            return this;
        }

        public BookBuilder WithTotalCopies(int totalCopies)
        {
            _totalCopies = totalCopies;
            return this;
        }

        public BookBuilder WithCopiesInUse(int copiesInUse)
        {
            _copiesInUse = copiesInUse;
            return this;
        }

        public BookBuilder WithType(string type)
        {
            _type = type;
            return this;
        }

        public BookBuilder WithISBN(string isbn)
        {
            _isbn = isbn;
            return this;
        }

        public BookBuilder WithCategory(string category)
        {
            _category = category;
            return this;
        }

        public Book Build()
        {
            return new Book(_title, _firstName, _lastName, _type, _isbn, _category)
            {
                TotalCopies = _totalCopies,
                CopiesInUse = _copiesInUse
            };
        }
    }
}