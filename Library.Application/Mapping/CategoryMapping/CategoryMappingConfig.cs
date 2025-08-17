using Library.Application.Dto.CategoryDtos;
using Library.Application.Features.Categories.Commands.Add;
using Library.Domain.Entities;
using Mapster;

namespace Library.Application.Mapping.CategoryMapping;

internal sealed class CategoryMappingConfig : IRegister
{

    public void Register(TypeAdapterConfig config)
    {
        // Entity → EntityDto
        config.NewConfig<Category, CategoryDto>()
            .MapWith(src => new CategoryDto(
                src.Name,
                src.Description
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
        config.NewConfig<CategoryDto, Category>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description);
            
        // Request → Entity
        config.NewConfig<AddCategoryCommand, Category>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description);
    }
}