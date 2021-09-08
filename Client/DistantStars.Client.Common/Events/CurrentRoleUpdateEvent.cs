using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistantStars.Client.Model.Models;

namespace DistantStars.Client.Common.Events
{
    public class CurrentRoleUpdateEvent : PubSubEvent<RoleInfoModel>
    {
    }
}
