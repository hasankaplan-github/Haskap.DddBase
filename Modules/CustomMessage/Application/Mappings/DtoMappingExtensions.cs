using Modules.CustomMessage.Application.Dtos;

namespace Modules.CustomMessage.Application.Mappings;

public static class DtoMappingExtensions
{
    extension(Domain.CustomMessageAggregate.CustomMessage customMessage)
    {
        public CustomMessageDto ToCustomMessageDto()
        {
            return new()
            {
                Icon = customMessage.Icon,
                Title = customMessage.Title,
                Text = customMessage.Text
            };
        }
    }
}