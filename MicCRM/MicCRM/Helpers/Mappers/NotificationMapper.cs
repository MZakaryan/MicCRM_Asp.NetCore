using MicCRM.Data.Entities;
using MicCRM.Models.NotificationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicCRM.Helpers.Mappers
{
    public static class NotificationMapper
    {
        public static NotificationInfoViewModel Mapping(Notification notification)
        {
            return new NotificationInfoViewModel()
            {
                Id = notification.Id,
                FirstName = notification.FirstName,
                LastName = notification.LastName,
                Phone1 = notification.Phone1,
                Phone2 = notification.Phone2,
                Email = notification.Email,
                Date = notification.Date.Date,
                Technology = notification.Technology,
                IsMuted = notification.IsMuted
            };
        }
    }
}
