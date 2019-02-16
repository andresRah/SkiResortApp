namespace SkiResort.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Plugin.FilePicker;
    using Plugin.FilePicker.Abstractions;
    using SkiResort.Models;
    using SkiResort.Services;
    using Xamarin.Forms;

    /// <summary>
    /// Test ski view model.
    /// </summary>
    public class TestSkiViewModel : BaseViewModel
    {
        /// <summary>
        /// The result path and drop.
        /// </summary>
        private SkiDFSResponse _resultPathAndDrop;

        /// <summary>
        /// The result path and drop.
        /// </summary>
        public SkiDFSResponse ResultPathAndDrop 
        { 
           get => _resultPathAndDrop; 
           set 
           {
                _resultPathAndDrop = value;

                if (_resultPathAndDrop.ResultPointsList.Count <= 0)
                {
                   IsVisiblePointList = false;
                }

                IsVisiblePointList = true;
                OnPropertyChanged();
                OnPropertyChanged("IsVisiblePointList");
            } 
        }

        /// <summary>
        /// The is visible point list.
        /// </summary>
        private bool _isVisiblePointList;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:SkiResort.ViewModels.TestSkiViewModel"/> is
        /// visible point list.
        /// </summary>
        /// <value><c>true</c> if is visible point list; otherwise, <c>false</c>.</value>
        public bool IsVisiblePointList
        {
            get => _isVisiblePointList;
            set
            {
                _isVisiblePointList = value;
                OnPropertyChanged();
            }
        }

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
            _isVisiblePointList = false;

            RegisterCommands();

            _resultPathAndDrop = new SkiDFSResponse
            {
                MaxDrop = 0,
                MaxPath = 0,
                ResultPointsList = new List<PointsSkiDFSResponse>()
            };
        }

        /// <summary>
        /// Registers the commands.
        /// </summary>
        private void RegisterCommands() 
        {
            CommandLoadFile = new Command(async () => await SelectFileAsync());
        }

        /// <summary>
        /// Loads the file async.
        /// </summary>
        /// <returns>The file async.</returns>
        private async Task SelectFileAsync()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(50);

                if (Device.OS != TargetPlatform.iOS)
                {
                    FileData fileSelected = await CrossFilePicker.Current.PickFile();

                    string extension = Path.GetExtension(fileSelected.FileName);

                    if (!allowedFilesTypesExts.Any(x => x.Equals(extension)))
                    {
                        await Application.Current.MainPage.DisplayAlert("Challenge", "Not compatible file!!!", "Ok");
                        return;
                    }

                    var resultDFSProcess = await ProcessFileAsync(fileSelected);

                    if (!resultDFSProcess.Item2)
                    {
                        await Application.Current.MainPage.DisplayAlert("Challenge", "Get maximum path and drop failed!!!", "Ok");
                        return;
                    }

                    ResultPathAndDrop = resultDFSProcess?.Item1;
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

        /// <summary>
        /// Processes the file async.
        /// </summary>
        /// <returns>The file async.</returns>
        /// <param name="fileSelected">File selected.</param>
        private async Task<Tuple<SkiDFSResponse, bool>> ProcessFileAsync(FileData fileSelected)
        {
            Stream fileStream = fileSelected.GetStream();
            ISkiDFSPathService SkiDFSPath = new SkiDFSPathService();

            return await SkiDFSPath.ProcessDFSAsync(fileStream);
        }
    }
}