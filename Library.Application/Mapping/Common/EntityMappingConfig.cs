using Library.Application.Dto.Abstractions;
using Library.Domain.Abstractions;
using Mapster;

namespace Library.Application.Mapping.Common;

internal sealed class EntityMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Entity → EntityDto
        config.NewConfig<Entity, EntityDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Version, src => src.Version)
            .Map(dest => dest.CreatedBy, src => src.CreatedBy)
            .Map(dest => dest.UpdatedBy, src => src.UpdatedBy)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);

        // EntityDto → Entity
        config.NewConfig<EntityDto, Entity>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Version, src => src.Version)
            .Map(dest => dest.CreatedBy, src => src.CreatedBy)
            .Map(dest => dest.UpdatedBy, src => src.UpdatedBy)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);
    }
}

