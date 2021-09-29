using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace DistantStars.Client.Common.ViewModels
{
    public class ViewModelBase : BindableBase
    {
        /// <summary>
        /// 绑定视图界面
        /// </summary>
        protected FrameworkElement _View;

        #region LoadedCommand 加载命令
        /// <summary>
        /// 加载命令
        /// </summary>
        public ICommand LoadedCommand => new DelegateCommand<object>(Loaded);

        private void Loaded(object obj)
        {
            if (obj is FrameworkElement view)
            {
                _View = view;
                LoadedContinue();
            }
        }
        public virtual void LoadedContinue() { }
        #endregion
    }
}
