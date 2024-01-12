using IBSConnect.Business;
using Microsoft.Extensions.DependencyInjection;

namespace IBSConnect.ServiceConfiguration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusiness(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IUserBL, UserBL>();
        serviceCollection.AddTransient<IMetaDataBL, MetaDataBL>();
        serviceCollection.AddTransient<IMemberBL, MemberBL>();
        serviceCollection.AddTransient<IMemberSessionBL, MemberSessionBL>();
        serviceCollection.AddTransient<ISettingBL, SettingBL>();
        serviceCollection.AddTransient<IBillingPeriodBL, BillingPeriodBL>();
        serviceCollection.AddTransient<IBackupBL, BackupBL>();
        serviceCollection.AddTransient<IReportsBL, ReportsBL>();
        return serviceCollection;
    }
}