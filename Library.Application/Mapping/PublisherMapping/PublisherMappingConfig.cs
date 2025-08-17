using Library.Application.Dto.PublisherDtos;
using Library.Application.Features.Publishers.Commands.Add;
using Library.Domain.Entities;
using Mapster;

namespace Library.Application.Mapping.PublisherMapping;

internal sealed class PublisherMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Entity → EntityDto
        config.NewConfig<Publisher, PublisherDto>()
            .MapWith(src => new PublisherDto(
                src.Name,
                src.Website,
                src.Address,
                src.Country
            )
            {
                Id = src.Id,
                Version = src.Version,
                CreatedAt = src.CreatedAt,
                UpdatedAt = src.UpdatedAt,
                CreatedBy = src.CreatedBy,
                UpdatedBy = src.UpdatedBy
            });

        // EntityDto → Entity
        config.NewConfig<PublisherDto, Publisher>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Website, src => src.Website)
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.Country, src => src.Country);

        // Request → Entity
        config.NewConfig<AddPublisherCommand, Publisher>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Website, src => src.Website)
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.Country, src => src.Country);
    }
}