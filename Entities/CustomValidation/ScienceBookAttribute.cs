using System.ComponentModel.DataAnnotations;

namespace Entities.CustomValidation
{
    public class ScienceBookAttribute : ValidationAttribute
    {
        public BookGenre Genre { get; set; }
        public string Error => $"The genre of the book must be {BookGenre.Science}";

        public ScienceBookAttribute(BookGenre genre)
        {
            Genre = genre;
        }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            var book = (Book)validationContext.ObjectInstance;

            if (!book.Genre.Equals(Genre.ToString()))
                return new ValidationResult(Error);

            return ValidationResult.Success;
        }
    }

    public enum BookGenre
    {
        Science,
        Fiction,
        History,
        Biography,
        Fantasy,
    }

    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Range(10, int.MaxValue)]
        public int Price { get; set; }

        [ScienceBook(BookGenre.Science)]
        public string? Genre { get; set; }
    }

    public class Book1 : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Range(10, int.MaxValue)]
        public int Price { get; set; }
        public string? Genre { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errorMessage = $"The genre of the book must be {BookGenre.Science}";
            if (!Genre.Equals(BookGenre.Science.ToString()))
                yield return new ValidationResult(errorMessage, new[] { nameof(Genre) });
        }
    }
}
