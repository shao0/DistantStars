﻿using DistantStars.Client.Model.Models.Systems;
using Prism.Events;

namespace DistantStars.Client.Model.Events
{
    public class CurrentUserUpdateEvent : PubSubEvent<UserInfoModel>
    {
    }
}
