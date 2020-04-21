﻿using DiabloII.Application.Responses.Notifications;
using DiabloII.Application.Tests.Contexts.Bases;

namespace DiabloII.Application.Tests.Contexts.Domains.Notifications
{
    public interface INotificationsTestContext :
        ITestContextAll<NotificationDto>,
        ITestContextCreated<NotificationDto>
    {
    }
}