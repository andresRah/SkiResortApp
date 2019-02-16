namespace SkiResort.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class SkiDFSPathService : ISkiDFSPathService
    {
        /// <summary>
        /// The rows count
        /// </summary>
        private int rowsCount { get; set; }

        /// <summary>
        /// The columns count
        /// </summary>
        private int columnsCount { get; set; }


        /// <summary>
        /// Processes the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="fileStream">File stream.</param>
        public async Task ProcessAsync(Stream fileStream) 
        {
            #region 1. Read and define GeoMap array from file and the differents Paths and Drops into arrays.
            Tuple<int, int, int[,], bool> fileRead = await ReadAndDecodeFileAsync(fileStream);

            if (fileRead.Item4 == false) { await Application.Current.MainPage.DisplayAlert("Challenge", "Malformed file", "Ok"); return; }

            rowsCount = fileRead.Item1;
            columnsCount = fileRead.Item2;

            int[,] GeoMap = fileRead.Item3;
            int[,] Path = new int[rowsCount, columnsCount];
            int[,] Drop = new int[rowsCount, columnsCount];
            #endregion



        }

        /// <summary>
        /// Reads the and decode file.
        /// </summary>
        /// <returns>The and decode file.</returns>
        /// <param name="fileStream">File stream.</param>
        private async Task<Tuple<int, int, int[,], bool>> ReadAndDecodeFileAsync(Stream fileStream)
        {
            var result = Tuple.Create(0, 0, new int[,] { }, false);

            using (StreamReader skiingFile = new StreamReader(fileStream))
            {
                var line = await skiingFile.ReadLineAsync();

                string[] firstLine = line.Split(' ');

                if (firstLine.Count() != 2)
                    return result;

                int rowsNumber = Convert.ToInt32(firstLine.First());
                int columnsNumber = Convert.ToInt32(firstLine.Last());
                int dimensions = (rowsNumber * columnsNumber);

                int[,] contentFile = await ReadFileAsync(skiingFile, rowsNumber, columnsNumber);

                if (contentFile?.Length == 0)
                    return result;

                bool convertStatus = (contentFile?.LongLength == dimensions) ? true : false;

                result = Tuple.Create(rowsNumber, columnsNumber, contentFile, convertStatus);
            }

            return result;
        }

        /// <summary>
        /// Reads the file async.
        /// </summary>
        /// <returns>The file async.</returns>
        /// <param name="skiingFile">Skiing file.</param>
        /// <param name="rowsNumber">Rows number.</param>
        /// <param name="columnsNumber">Columns number.</param>
        private async Task<int[,]> ReadFileAsync(StreamReader skiingFile, int rowsNumber, int columnsNumber)
        {
            int[,] contentFile = null;

            try
            {
                string line = string.Empty;
                contentFile = new int[rowsNumber, columnsNumber];
                int a = 0;

                while ((line = await skiingFile.ReadLineAsync()) != null)
                {
                    var row = line.Split(' ').ToList();

                    int b = 0;
                    row.ForEach(x =>
                    {
                        contentFile[a, b++] = int.Parse(x);
                    });
                    a++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return contentFile;
        }
    }
}
