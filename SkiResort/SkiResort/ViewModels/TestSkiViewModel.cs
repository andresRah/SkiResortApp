namespace SkiResort.ViewModels
{
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class TestSkiViewModel : BaseViewModel
    {
        public TestSkiViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}
