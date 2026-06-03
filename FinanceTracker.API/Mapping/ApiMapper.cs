using AutoMapper;
using FinanceTracker.API.Contracts.BalanceChanges;
using FinanceTracker.API.Contracts.Budgets;
using FinanceTracker.API.Contracts.Categories;
using FinanceTracker.API.Contracts.FinancialOperations;
using FinanceTracker.API.Contracts.Notifications;
using FinanceTracker.API.Contracts.Reports;
using FinanceTracker.API.Contracts.Tags;
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
            .ForMember(dest => dest.TagIds,
                opt => opt.MapFrom(src => src.TagIds));

        CreateMap<FinancialOperationDto, FinancialOperationResponse>()
            .ForMember(dest => dest.TagIds, 
                opt => opt.MapFrom(src => src.TagIds));

        CreateMap<CategoryRequest, CategoryDto>()
            .ForMember(dest => dest.OperationType,
                opt => opt.MapFrom(src => ConvertOperationType(src.OperationType)));

        CreateMap<CategoryDto, CategoryResponse>()
            .ForMember(dest => dest.OperationType,
                opt => opt.MapFrom(src => Enum.GetName(src.OperationType) ?? src.OperationType.ToString()));
        
        CreateMap<DailyReportDto, DailyReportResponse>();
        
        CreateMap<DatePeriodReportDto, DatePeriodReportResponse>();
        
        CreateMap<TagRequest, TagDto>();
        
        CreateMap<TagDto, TagResponse>();
        
        CreateMap<BudgetRequest, BudgetDto>();

        CreateMap<BudgetDto, BudgetResponse>();
        
        CreateMap<BudgetMemberDto, BudgetMemberResponse>();
        
        CreateMap<NotificationRequest, NotificationDto>();
        
        CreateMap<NotificationDto, NotificationResponse>();
    }

    private static OperationType ConvertOperationType(string operationType)
    {
        return Enum.TryParse<OperationType>(operationType, true, out var result)
            ? result
            : throw new ArgumentException($"Invalid operation type: {operationType}");
    }
}