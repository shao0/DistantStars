using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DistantStars.Client.Common;
using DistantStars.Client.IBLL.Systems;
using DistantStars.Client.Start.Models;
using DistantStars.Common.DTO.Enums;
using DistantStars.Common.DTO.Parameters;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace DistantStars.Client.Start.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly IUserBLL _userBll;
        private readonly IFileBLL _file;
        private readonly IEventAggregator _ea;
        private FrameworkElement _view;

        public LoginViewModel(IUserBLL userBll, IFileBLL file, IEventAggregator ea)
        {
            _userBll = userBll;
            _file = file;
            _ea = ea;
        }

        #region List<LoginInfoRecord> LoginList 记录登录信息
        /// <summary>
        /// 记录登录信息字段
        /// </summary>
        private List<LoginInfoRecord> _LoginList;
        /// <summary>
        /// 记录登录信息属性
        /// </summary>
        public List<LoginInfoRecord> LoginList
        {
            get => _LoginList;
            set => SetProperty(ref _LoginList, value);
        }
        #endregion


        #region LoginInfoRecord Record 登录类
        /// <summary>
        /// 登录类字段
        /// </summary>
        private LoginInfoRecord _record;
        /// <summary>
        /// 登录类属性
        /// </summary>
        public LoginInfoRecord Record
        {
            get => _record;
            set
            {
                if (value == null) value = new LoginInfoRecord();
                SetProperty(ref _record, value);
            }
        }

        #endregion

        #region string ErrorMessage 错误信息
        /// <summary>
        /// 错误信息字段
        /// </summary>
        private string _ErrorMessage;
        /// <summary>
        /// 错误信息属性
        /// </summary>
        public string ErrorMessage
        {
            get => _ErrorMessage;
            set => SetProperty(ref _ErrorMessage, value);
        }
        #endregion

        private string _DocumentPath;
        string DocumentPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DocumentPath))
                {
                    _DocumentPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DistantStars");
                    if (!Directory.Exists(_DocumentPath))
                    {
                        Directory.CreateDirectory(_DocumentPath);
                    }
                }
                return _DocumentPath;
            }
        }

        private string JsonPath;

        #region LoadedCommand 加载命令
        /// <summary>
        /// 加载命令
        /// </summary>
        public ICommand LoadedCommand => new DelegateCommand<object>(Loaded);

        private void Loaded(object obj)
        {
            if (obj is FrameworkElement view)
            {
                _view = view;
                JsonPath = Path.Combine(DocumentPath, "UserLogin.json");
                if (File.Exists(JsonPath))
                {
                    var json = File.ReadAllText(JsonPath);
                    LoginList = JsonConvert.DeserializeObject<List<LoginInfoRecord>>(json);
                    Record = LoginList.FirstOrDefault();
                }
                else
                {
                    LoginList = new List<LoginInfoRecord>();
                }
                if (Record == null)
                {
                    Record = new LoginInfoRecord();
                }
                else if (Record.AutoLogin)
                {
                    Login(_view);
                }
            }
        }

        #endregion


        #region LoginCommand 登录命令
        /// <summary>
        /// 登录命令
        /// </summary>
        public ICommand LoginCommand => new DelegateCommand<object>(Login);

        private async void Login(object obj)
        {
            //IMessage message = null;
            try
            {
                if (string.IsNullOrWhiteSpace(Record.UserAccount))
                {
                    throw new Exception("用户不能为空!");
                }

                if (string.IsNullOrWhiteSpace(Record.Password))
                {
                    throw new Exception("密码不能为空!");
                }

                if (obj is Window login)
                {
                    //message = _view.Show("登录中...", ShowEnum.ShowLoading);

                    if (await _userBll.LoginAsync(Record.UserAccount, Record.Password))
                    {
                        //message.Message = "登录成功";
                      await  LocalRecord();
                        login.DialogResult = true;
                    }
                    else
                    {
                        throw new Exception("用户或密码错误!");
                    }
                }
            }
            catch (Exception e)
            {
                //_view.Show(e.Message);
            }
            //finally
            //{
            //    message?.Close();
            //}
        }

        async Task LocalRecord()
        {
            var accountDocument = Path.Combine(DocumentPath, Record.UserAccount);
            if (!Directory.Exists(accountDocument))
            {
                Directory.CreateDirectory(accountDocument);
            }
            var imagePath = Path.Combine(accountDocument, $"{Record.UserAccount}.jpg");
            Global.CurrentUserInfo.UserIconPath = Record.UserIconPath;
            if (Record.UserIconMd5 != Global.CurrentUserInfo.UserIcon)
            {
                await DownloadHeadPortrait(imagePath, Record);
            }
            LoginList.Remove(Record);
            if (!Record.RememberPassword)
            {
                Record.Password = string.Empty;
            }
            LoginList.Insert(0, Record);
            var json = JsonConvert.SerializeObject(LoginList, Formatting.Indented);
            using (var sw = File.CreateText(JsonPath))
            {
                await sw.WriteAsync(json);
            }
            //_ea.GetEvent<CurrentUserUpdateEvent>().Publish(Global.CurrentUserInfo);
        }
        private async Task DownloadHeadPortrait(string imagePath, LoginInfoRecord loginCurrent)
        {
            var bytes = await _file.DownloadFileAsync(new FileParameter
            { FileType = FileType.Image, MD5 = Global.CurrentUserInfo.UserIcon });
            if (bytes.Length > 0)
            {
                using (var ms = new MemoryStream(bytes))
                {
                    using (var fs = new FileStream(imagePath, FileMode.Create))
                    {
                        ms.WriteTo(fs);
                    }
                }
                loginCurrent.UserIconMd5 = Global.CurrentUserInfo.UserIcon;
                Global.CurrentUserInfo.UserIconPath = loginCurrent.UserIconPath = imagePath;
            }
        }

        #endregion

    }
}
