using Torc.BuildingBlocks.EventBus.Events;

namespace Torc.Services.BookLibrary.API.IntegrationEvents
{
    public class BookAddedIntegrationEvent : IntegrationEvent
    {
        public string BookTitle { get; init; }
        public string Author { get; init; }

        public BookAddedIntegrationEvent(string bookTitle, string author)
        {
            BookTitle = bookTitle;
            Author = author;
        }
    }
}
