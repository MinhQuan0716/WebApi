using AutoMapper;
using Domain.Entities.TrainingClassRelated;

namespace Infrastructures.TypeConverter
{
    public class HighLightDatesTypeConverter : ITypeConverter<DateTime, HighlightedDates>
    {
        public HighlightedDates Convert(DateTime source, HighlightedDates destination, ResolutionContext context)
        {
            var mapped = new HighlightedDates();
            mapped.HighlightedDate = source;
            return mapped;
        }
    }
}
