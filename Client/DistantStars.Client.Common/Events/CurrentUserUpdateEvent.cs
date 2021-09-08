using Prism.Events;
using DistantStars.Client.Model.Models;

namespace DistantStars.Client.Common.Events
{
    public class CurrentUserUpdateEvent : PubSubEvent<UserInfoModel>
    {
    }
}
