using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Torc.Services.BookLibrary.Domain.Entities;

namespace Torc.Services.BookLibrary.Infrastructure.EntityTypeConfigurations
{
    public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            // Specify the table name
            builder.ToTable("Books");

            // Specify the primary key
            builder.HasKey(e => e.BookId);

            // Configure column names and constraints for each property

            // BookId column configuration
            builder.Property(e => e.BookId)
                   .HasColumnName("book_id");

            // Title column configuration
            builder.Property(e => e.Title)
                   .HasColumnName("title")
                   .IsRequired();

            // FirstName column configuration
            builder.Property(e => e.FirstName)
                   .HasColumnName("first_name")
                   .IsRequired();

            // LastName column configuration
            builder.Property(e => e.LastName)
                   .HasColumnName("last_name")
                   .IsRequired();

            // TotalCopies column configuration
            builder.Property(e => e.TotalCopies)
                   .HasColumnName("total_copies")
                   .IsRequired();

            // CopiesInUse column configuration
            builder.Property(e => e.CopiesInUse)
                   .HasColumnName("copies_in_use")
                   .IsRequired();

            // Type column configuration
            builder.Property(e => e.Type)
                   .HasColumnName("type")
                   .HasMaxLength(50);

            // ISBN column configuration
            builder.Property(e => e.ISBN)
                   .HasColumnName("isbn")
                   .HasMaxLength(80);

            // Category column configuration
            builder.Property(e => e.Category)
                   .HasColumnName("category")
                   .HasMaxLength(50);
        }
    }
}
