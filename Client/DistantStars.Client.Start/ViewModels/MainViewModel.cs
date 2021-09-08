using Prism.Mvvm;

namespace DistantStars.Client.Start.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private string _title = "DistantStars";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public MainViewModel()
        {

        }
    }
}
