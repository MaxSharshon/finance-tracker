using FinanceTracker.Contracts.Notifications;
using FinanceTracker.UI.Services.Interfaces;

namespace FinanceTracker.UI.Services;

public class NotificationClientService(HttpClient client)
    : Service<NotificationRequest, NotificationResponse>(client, "Notifications"), INotificationClientService;
