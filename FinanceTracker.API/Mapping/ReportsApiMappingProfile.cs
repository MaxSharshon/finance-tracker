using AutoMapper;
using FinanceTracker.API.Contracts.Reports;
using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.API.Mapping;

public class ReportsApiMappingProfile : Profile
{
    public ReportsApiMappingProfile()
    {
        CreateMap<DailyReportDto, DailyReportResponse>();

        CreateMap<DatePeriodReportDto, DatePeriodReportResponse>();
    }
}
