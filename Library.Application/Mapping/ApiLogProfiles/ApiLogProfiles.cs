using AutoMapper;
using Library.Application.Dto.ApiLogDTOs;
using Library.Application.Extensions;
using Library.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace Library.Application.Mapping.ApiLogProfiles;

public sealed class ApiLogProfiles : Profile
{
    public ApiLogProfiles()
    {
        CreateMap<ApiLog, ApiLogDto>()
            .ForMember(dest => dest.requestTime,
                opt => opt.MapFrom(src => src.RequestTime.ToTurkeyTime()))
            .ForMember(dest => dest.responseTime,
                opt => opt.MapFrom(src => src.ResponseTime.ToTurkeyTime()))
            .ForMember(dest => dest.id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.iPAddress,
                opt => opt.MapFrom(src => src.IPAddress))
            .ForMember(dest => dest.path,
                opt => opt.MapFrom(src => src.Path))
            .ForMember(dest => dest.method,
                opt => opt.MapFrom(src => src.Method))
            .ForMember(dest => dest.requestBody,
                opt => opt.MapFrom(src => ParseJsonOrRaw(src.RequestBody)))
            .ForMember(dest => dest.responseBody,
                opt => opt.MapFrom(src => ParseJsonOrRaw(src.ResponseBody)))
            .ForMember(dest => dest.statusCode,
                opt => opt.MapFrom(src => src.StatusCode));
    }

    private static object? ParseJsonOrRaw(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        try
        {
            return JToken.Parse(json);
        }
        catch
        {
            return json;
        }
    }
}