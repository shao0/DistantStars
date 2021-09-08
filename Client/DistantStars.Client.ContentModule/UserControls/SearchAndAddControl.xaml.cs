using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DistantStars.Client.ContentModule.UserControls
{
    /// <summary>
    /// SearchAndAddControl.xaml 的交互逻辑
    /// </summary>
    public partial class SearchAndAddControl : UserControl
    {
        public SearchAndAddControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register(
            "SearchText", typeof(string), typeof(SearchAndAddControl), new PropertyMetadata(default(string)));

        public string SearchText
        {
            get { return (string) GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty QueryCommandProperty = DependencyProperty.Register(
            "QueryCommand", typeof(ICommand), typeof(SearchAndAddControl), new PropertyMetadata(default(ICommand)));

        public ICommand QueryCommand
        {
            get { return (ICommand) GetValue(QueryCommandProperty); }
            set { SetValue(QueryCommandProperty, value); }
        }


        public static readonly DependencyProperty AddCommandProperty = DependencyProperty.Register(
            "AddCommand", typeof(ICommand), typeof(SearchAndAddControl), new PropertyMetadata(default(ICommand)));

        public ICommand AddCommand
        {
            get { return (ICommand) GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }
    }
}
