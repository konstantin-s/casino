using Prism.Mvvm;

namespace prism_app.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Жёванное казино";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
