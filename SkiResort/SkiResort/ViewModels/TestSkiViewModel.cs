using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.FilePicker;
using Xamarin.Forms;

namespace SkiResort.ViewModels
{
    /// <summary>
    /// Test ski view model.
    /// </summary>
    public class TestSkiViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the command load file.
        /// </summary>
        /// <value>The command load file.</value>
        public ICommand CommandLoadFile { get; set; }

        /// <summary>
        /// The allowed files types exts
        /// </summary>
        private readonly string[] allowedFilesTypesExts = {".txt"};

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SkiResort.ViewModels.TestSkiViewModel"/> class.
        /// </summary>
        public TestSkiViewModel()
        {
            Title = "TestSki";
            RegisterCommands();
        }

        /// <summary>
        /// Registers the commands.
        /// </summary>
        private void RegisterCommands() 
        {
            //CommandLoadFile = new Command(async (e) => await LoadFileAsync());
            CommandLoadFile = new Command(async () => await LoadFileAsync());
        }

        /// <summary>
        /// Loads the file async.
        /// </summary>
        /// <returns>The file async.</returns>
        private async Task LoadFileAsync()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(50);

                if (Device.OS == TargetPlatform.Android)
                {
                    var fileSelected = await CrossFilePicker.Current.PickFile();

                    string extension = Path.GetExtension(fileSelected.FilePath);

                    if (!allowedFilesTypesExts.Any(x => x.Equals(extension)))
                    {
                        await Application.Current.MainPage.DisplayAlert("Challenge", "Not compatible file", "Ok");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Challenge", "Load files error", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
