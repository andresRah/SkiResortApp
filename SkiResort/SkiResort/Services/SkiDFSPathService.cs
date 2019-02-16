namespace SkiResort.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using SkiResort.Models;
    using SkiResortRoute;
    using Xamarin.Forms;

    /// <summary>
    /// Ski DFSP ath service.
    /// </summary>
    public class SkiDFSPathService : DFSService, ISkiDFSPathService
    {
        /// <summary>
        /// Processes the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="fileStream">File stream.</param>
        public override async Task<Tuple<SkiDFSResponse, bool>> ProcessDFSAsync(Stream fileStream)
        {
            bool resultTask = false;
            SkiDFSResponse response = null;

            try
            {
                #region 1. Read and define GeoMap array from file and the differents Paths and Drops into arrays.
                Tuple<int, int, int[,], bool> fileRead = await ReadAndDecodeFileAsync(fileStream);

                if (fileRead.Item4 == false) { await Application.Current.MainPage.DisplayAlert("Challenge", "Malformed file", "Ok"); await Task.FromResult(resultTask); }

                RowsCount = fileRead.Item1;
                ColumnsCount = fileRead.Item2;

                int[,] GeoMap = fileRead.Item3;
                int[,] Path = new int[RowsCount, ColumnsCount];
                int[,] Drop = new int[RowsCount, ColumnsCount];
                #endregion

                #region 2. Apply DFS algorithm
                for (int i = 0; i < RowsCount; i++)
                {
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        Navigation PathAux = DFSOperation(i, j, GeoMap);
                        Path[i, j] = PathAux.PathLenght;
                        Drop[i, j] = GeoMap[i, j] - GeoMap[PathAux.NavCoords.xCoord, PathAux.NavCoords.yCoord];
                    }
                }
                #endregion

                #region 3. Get Coordinates of point with max path and drop in the Path[] and Drop[]

                int[] maxPathCoordXY = new int[2]; // Coordinate X,Y of point with max path and drop
                int maxPath = -1;                  // Have the longest path
                int maxDrop = -1;                  // Have the longest Drop

                // 3.1 Find maxPath and maxDrop 
                resultTask = GetMaxPath(Path, Drop, maxPathCoordXY, ref maxPath, ref maxDrop);

                if (resultTask == false)
                {
                    await Application.Current.MainPage.DisplayAlert("Challenge", "Malformed file imposible get maximun path", "Ok");
                    return await Task.FromResult(Tuple.Create(response, false));
                }
                #endregion

                response = FormatResultSkiPath(fileRead?.Item3, maxPathCoordXY, maxPath, maxDrop);
            }
            catch (Exception ex) 
            {
                resultTask = false;
            }

            return Tuple.Create(response, resultTask);
        }

        private SkiDFSResponse FormatResultSkiPath(int[,] fileRead, int[] maxPathCoordXY, int maxPath, int maxDrop)
        {
            #region 4. Print Maximum Path and Drop
            SkiDFSResponse response = new SkiDFSResponse
            {
                MaxPath = maxPath,
                MaxDrop = maxDrop,
                ResultPointsList = new List<PointsSkiDFSResponse>()
            };
            //Console.WriteLine($"Maximal Path is: {maxPath} \nMaximal Drop is: {maxDrop}");
            #endregion

            #region 5. Print all Path points from the maximum to the minimum Point (Skiing down)

            // Load respective CoordX and CoordY values
            List<int> listPath = DFSForMaxPathLength(maxPathCoordXY[0], maxPathCoordXY[1], fileRead);
            listPath.Reverse();

            int[] CoordXY = new int[2];
            int index = 0;
            int temp = 0;

            foreach (var aux in listPath)
            {
                CoordXY[index % 2] = aux;
                index++;

                if (index % 2 == 0)
                {
                    response.ResultPointsList.Add(new PointsSkiDFSResponse
                    {
                        XCoord = temp,
                        YCoord = aux,
                        Altitude = fileRead[CoordXY[0], CoordXY[1]]
                    });
                }

                temp = aux;
            }
            #endregion
            return response;
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
