using DistantStars.Client.Model.Models.Systems;
using Prism.Events;

namespace DistantStars.Client.Common.Events
{
    public class CurrentUserUpdateEvent : PubSubEvent<UserInfoModel>
    {
    }
}
