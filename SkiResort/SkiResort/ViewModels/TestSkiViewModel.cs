﻿namespace SkiResort.ViewModels
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
        public SkiDFSResponse ResultPathAndDrop { get; set; }

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

            ResultPathAndDrop = new SkiDFSResponse
            {
                MaxDrop = 56,
                MaxPath = 45,
                ResultPointsList = new List<PointsSkiDFSResponse>()
                {
                   new PointsSkiDFSResponse { XCoord = 5, YCoord = 9, Altitude = 12},
                   new PointsSkiDFSResponse { XCoord = 7, YCoord = 6, Altitude = 55},
                   new PointsSkiDFSResponse { XCoord = 8, YCoord = 3, Altitude = 78},
                   new PointsSkiDFSResponse { XCoord = 13, YCoord = 2, Altitude = 44},
                   new PointsSkiDFSResponse { XCoord = 56, YCoord = 34, Altitude = 45}
                }
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

                if (Device.OS == TargetPlatform.Android)
                {
                    FileData fileSelected = await CrossFilePicker.Current.PickFile();

                    string extension = Path.GetExtension(fileSelected.FileName);

                    if (!allowedFilesTypesExts.Any(x => x.Equals(extension)))
                    {
                        await Application.Current.MainPage.DisplayAlert("Challenge", "Not compatible file!!!", "Ok");
                        return;
                    }

                    bool resultDFSProcess = await ProcessFileAsync(fileSelected);

                    if (!resultDFSProcess)
                    {
                        await Application.Current.MainPage.DisplayAlert("Challenge", "Get maximum path and drop failed!!!", "Ok");
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

        /// <summary>
        /// Processes the file async.
        /// </summary>
        /// <returns>The file async.</returns>
        /// <param name="fileSelected">File selected.</param>
        private async Task<bool> ProcessFileAsync(FileData fileSelected)
        {
            Stream fileStream = fileSelected.GetStream();
            ISkiDFSPathService SkiDFSPath = new SkiDFSPathService();

            return await SkiDFSPath.ProcessDFSAsync(fileStream);
        }
    }
}