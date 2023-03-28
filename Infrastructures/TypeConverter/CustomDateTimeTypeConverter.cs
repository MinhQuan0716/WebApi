using AutoMapper;
using Domain.Entities.TrainingClassRelated;

namespace Infrastructures.TypeConverter
{
    public class CustomDateTimeTypeConverter : ITypeConverter<HighlightedDates, DateTime>
    {
        public DateTime Convert(HighlightedDates source, DateTime destination, ResolutionContext context)
        {
            return source.HighlightedDate;
        }
    }
}
