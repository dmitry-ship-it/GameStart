using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GameStart.CatalogService.Data.EntityConfigurations.ValueConverters
{
    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter() : base(
            dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
            dateOnly => DateOnly.FromDateTime(dateOnly))
        { }
    }
}
