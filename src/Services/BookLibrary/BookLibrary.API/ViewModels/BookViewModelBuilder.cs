namespace Torc.Services.BookLibrary.API.ViewModels
{
    public class BookViewModelBuilder
    {
        private int _bookId;
        private string _title;
        private string _publisher;
        private string _author;
        private string _type = "No type"; // Default value
        private string _isbn = "No ISBN"; // Default value
        private string _category = "No category"; // Default value
        private string _availableCopies;

        public BookViewModelBuilder WithBookId(int bookId)
        {
            _bookId = bookId;
            return this;
        }

        public BookViewModelBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public BookViewModelBuilder WithPublisher(string publisher)
        {
            _publisher = publisher;
            return this;
        }

        public BookViewModelBuilder WithAuthor(string author)
        {
            _author = author;
            return this;
        }

        public BookViewModelBuilder WithType(string type)
        {
            _type = string.IsNullOrEmpty(type) ? "No type" : type;
            return this;
        }

        public BookViewModelBuilder WithIsbn(string isbn)
        {
            _isbn = string.IsNullOrEmpty(isbn) ? "No ISBN" : isbn;
            return this;
        }

        public BookViewModelBuilder WithCategory(string category)
        {
            _category = string.IsNullOrEmpty(category) ? "No category" : category;
            return this;
        }

        public BookViewModelBuilder WithAvailableCopies(string availableCopies)
        {
            _availableCopies = availableCopies;
            return this;
        }

        public BookViewModel Build()
        {
            return new BookViewModel
            {
                BookId = _bookId,
                Title = _title,
                Publisher = _publisher,
                Author = _author,
                Type = _type,
                Isbn = _isbn,
                Category = _category,
                AvailableCopies = _availableCopies
            };
        }
    }
}
