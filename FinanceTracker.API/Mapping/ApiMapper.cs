using AutoMapper;
using FinanceTracker.API.Contracts;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Core.Enums;

namespace FinanceTracker.API.Mapping;

public class ApiMapper : Profile
{
    public ApiMapper()
    {
        CreateMap<BalanceChangeDto, BalanceChangeResponse>()
            .ForMember(dest => dest.OperationType, opt => 
                opt.MapFrom(src => Enum.GetName(src.OperationType)));

        CreateMap<BalanceChangeRequest, BalanceChangeDto>()
            .ForMember(dest => dest.OperationType, opt => 
                    opt.MapFrom(src => ConvertOperationType(src.OperationType)));

        CreateMap<FinancialOperationRequest, FinancialOperationDto>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => ParseDate(src.Date)));

        CreateMap<FinancialOperationDto, FinancialOperationResponse>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-ddTHH:mm:ss")));

        CreateMap<DailyReportDto, DailyReportResponse>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-ddTHH:mm:ss")));
        
        CreateMap<DatePeriodReportDto, DatePeriodReportResponse>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToString("yyyy-MM-dd")));
    }

    private static DateTime ParseDate(string date)
    {
        if (DateTime.TryParse(date, out var parsedDate))
            return parsedDate;

        throw new ArgumentException("Invalid date format");
    }

    private static OperationType ConvertOperationType(string operationType)
    {
        if (Enum.TryParse<OperationType>(operationType, true, out var result))
            return result;

        throw new ArgumentException($"Invalid operation type: {operationType}");
    }
}