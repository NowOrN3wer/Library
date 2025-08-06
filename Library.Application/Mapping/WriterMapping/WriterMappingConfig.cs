using Library.Application.Dto.WriterDtos;
using Library.Application.Features.Writers.Add;
using Library.Domain.Entities;
using Mapster;

namespace Library.Application.Mapping.WriterMapping;

internal sealed class WriterMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Entity → EntityDto
        config.NewConfig<Writer, WriterDto>()
            .MapWith(src => new WriterDto(
                src.FirstName,
                src.LastName,
                src.Biography,
                src.Nationality,
                src.BirthDate,
                src.DeathDate,
                src.Website,
                src.Email
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
        config.NewConfig<WriterDto, Writer>()
              .Map(dest => dest.Id, src => src.Id)
              .Map(dest => dest.FirstName, src => src.FirstName)
              .Map(dest => dest.LastName, src => src.LastName)
              .Map(dest => dest.Biography, src => src.Biography)
              .Map(dest => dest.Nationality, src => src.Nationality)
              .Map(dest => dest.BirthDate, src => src.BirthDate)
              .Map(dest => dest.DeathDate, src => src.DeathDate)
              .Map(dest => dest.Website, src => src.Website)
              .Map(dest => dest.Email, src => src.Email);
        
        // Request → Entity
        config.NewConfig<AddWriterCommand, Writer>()
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.Biography, src => src.Biography)
            .Map(dest => dest.Nationality, src => src.Nationality)
            .Map(dest => dest.BirthDate, src => src.BirthDate)
            .Map(dest => dest.DeathDate, src => src.DeathDate)
            .Map(dest => dest.Website, src => src.Website)
            .Map(dest => dest.Email, src => src.Email);

    }
}
