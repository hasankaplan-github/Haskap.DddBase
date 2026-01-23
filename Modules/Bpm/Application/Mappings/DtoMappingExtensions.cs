using Modules.Bpm.Application.Dtos;
using Modules.Bpm.Domain.ProcessAggregate;

namespace Modules.Bpm.Application.Mappings;

public static class DtoMappingExtensions
{
    extension(Command command)
    {
        public CommandOutputDto ToCommandOutputDto()
        {
            return new()
            {
                Id = command.Id,
                ProcessId = command.ProcessId,
                DisplayName = command.DisplayName
            };
        }
    }

    extension(Domain.ProcessAggregate.Path path)
    {
        public AvailablePathOutputDto ToAvailablePathOutputDto()
        {
            return new()
            {
                Id = path.Id,
                ProcessId = path.ProcessId,
                FromStateId = path.FromStateId,
                ToStateId = path.ToStateId,
                CommandId = path.CommandId,
                Command = path.Command.ToCommandOutputDto(),
                ViewName = path.ViewName
            };
        }
    }
}