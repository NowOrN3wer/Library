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
              .Map(dest => dest.id, src => src.Id)
              .Map(dest => dest.firstName, src => src.FirstName)
              .Map(dest => dest.lastName, src => src.LastName)
              .Map(dest => dest.biography, src => src.Biography)
              .Map(dest => dest.nationality, src => src.Nationality)
              .Map(dest => dest.birthDate, src => src.BirthDate)
              .Map(dest => dest.deathDate, src => src.DeathDate)
              .Map(dest => dest.website, src => src.Website)
              .Map(dest => dest.email, src => src.Email);

        // EntityDto → Entity
        config.NewConfig<WriterDto, Writer>()
              .Map(dest => dest.Id, src => src.id)
              .Map(dest => dest.FirstName, src => src.firstName)
              .Map(dest => dest.LastName, src => src.lastName)
              .Map(dest => dest.Biography, src => src.biography)
              .Map(dest => dest.Nationality, src => src.nationality)
              .Map(dest => dest.BirthDate, src => src.birthDate)
              .Map(dest => dest.DeathDate, src => src.deathDate)
              .Map(dest => dest.Website, src => src.website)
              .Map(dest => dest.Email, src => src.email);
        
        // Request → Entity
        config.NewConfig<AddWriterCommand, Writer>()
            .Map(dest => dest.FirstName, src => src.firstName)
            .Map(dest => dest.LastName, src => src.lastName)
            .Map(dest => dest.Biography, src => src.biography)
            .Map(dest => dest.Nationality, src => src.nationality)
            .Map(dest => dest.BirthDate, src => src.birthDate)
            .Map(dest => dest.DeathDate, src => src.deathDate)
            .Map(dest => dest.Website, src => src.website)
            .Map(dest => dest.Email, src => src.email);

    }
}
