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
            .Map(dest => dest.id, src => src.Id)
            .Map(dest => dest.version, src => src.Version)
            .Map(dest => dest.createdBy, src => src.CreatedBy)
            .Map(dest => dest.updatedBy, src => src.UpdatedBy)
            .Map(dest => dest.createdAt, src => src.CreatedAt)
            .Map(dest => dest.updatedAt, src => src.UpdatedAt)
            .Map(dest => dest.isDeleted, src => src.IsDeleted);

        // EntityDto → Entity
        config.NewConfig<EntityDto, Entity>()
            .Map(dest => dest.Id, src => src.id)
            .Map(dest => dest.Version, src => src.version)
            .Map(dest => dest.CreatedBy, src => src.createdBy)
            .Map(dest => dest.UpdatedBy, src => src.updatedBy)
            .Map(dest => dest.CreatedAt, src => src.createdAt)
            .Map(dest => dest.UpdatedAt, src => src.updatedAt)
            .Map(dest => dest.IsDeleted, src => src.isDeleted);
    }
}

