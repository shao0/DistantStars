using System.Collections.Generic;
using Prism.Mvvm;

namespace DistantStars.Client.Model.Models.Systems
{
    public class RoleInfoModel : BindableBase
    {
        public int RoleId { get; set; }


        #region string RoleName 角色名称
        /// <summary>
        /// 角色名称字段
        /// </summary>
        private string _RoleName;
        /// <summary>
        /// 角色名称属性
        /// </summary>
        public string RoleName
        {
            get => _RoleName;
            set => SetProperty(ref _RoleName, value);
        }
        #endregion

        #region int State 角色状态
        /// <summary>
        /// 角色状态字段
        /// </summary>
        private int _State = 1;
        /// <summary>
        /// 角色状态属性
        /// </summary>
        public int State
        {
            get => _State;
            set => SetProperty(ref _State, value);
        }
        #endregion


        public string StateName { get; set; }

        public IEnumerable<MenuInfoModel> Menus { get; set; }
    }
}
