using System.Collections.ObjectModel;
using DistantStars.Common.DTO.Enums;
using Prism.Mvvm;

namespace DistantStars.Client.Model.Models.Systems
{
    public class MenuInfoModel : BindableBase
    {
        public int MenuId { get; set; }

        public string MenuHeader { get; set; }

        public string TargetView { get; set; }

        public int ParentId { get; set; }

        public MenuType MenuType { get; set; }

        public string StateName { get; set; }

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

        #region string MenuIcon 图标
        /// <summary>
        /// 图标字段
        /// </summary>
        private string _MenuIcon;
        /// <summary>
        /// 图标属性
        /// </summary>
        public string MenuIcon
        {
            get => _MenuIcon;
            set => SetProperty(ref _MenuIcon, value);
        }
        #endregion


        #region bool Checked 选中
        /// <summary>
        /// 选中字段
        /// </summary>
        private bool _Checked;
        /// <summary>
        /// 选中属性
        /// </summary>
        public bool Checked
        {
            get => _Checked;
            set
            {
                if (_Checked == value) return;
                SetProperty(ref _Checked, value);
                foreach (var child in Children)
                {
                    child.Checked = Checked;
                }

            }
        }

        #endregion

        public void Clear()
        {
            ClearChildren(this);
        }

        void ClearChildren(MenuInfoModel model)
        {
            foreach (var child in model.Children)
            {
                ClearChildren(child);
            }
            model.Children.Clear();
        }
        public ObservableCollection<MenuInfoModel> Children { get; set; } =
            new ObservableCollection<MenuInfoModel>();





    }
}
