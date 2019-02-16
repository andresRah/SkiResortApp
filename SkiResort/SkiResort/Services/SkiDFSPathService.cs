namespace SkiResort.Services
{
    using System;
    using System.IO;
    using System.Linq;

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


        public void Process(Stream fileStream) 
        {
            #region 1. Read and define GeoMap array from file and the differents Paths and Drops into arrays.
            Tuple<int, int, int[,], bool> fileRead = ReadAndDecodeFile(fileStream);

            if (fileRead.Item4 == false) { Console.WriteLine("File malformed"); return; }

            rowsCount = fileRead.Item1;
            columnsCount = fileRead.Item2;

            int[,] GeoMap = fileRead.Item3;
            int[,] Path = new int[rowsCount, columnsCount];
            int[,] Drop = new int[rowsCount, columnsCount];
            #endregion



        }

        private static Tuple<int, int, int[,], bool> ReadAndDecodeFile(Stream fileStream)
        {
            var result = Tuple.Create(0, 0, new int[,] { }, false);

            using (StreamReader skiingFile = new StreamReader(fileStream))
            {
                string[] firstLine = skiingFile.ReadLine().Split(' ');

                if (firstLine.Count() != 2)
                    return result;

                int rowsNumber = Convert.ToInt32(firstLine.First());
                int columnsNumber = Convert.ToInt32(firstLine.Last());
                int dimensions = (rowsNumber * columnsNumber);

                int[,] contentFile = ReadFile(skiingFile, rowsNumber, columnsNumber);

                if (contentFile?.Length == 0)
                    return result;

                bool convertStatus = (contentFile?.LongLength == dimensions) ? true : false;

                result = Tuple.Create(rowsNumber, columnsNumber, contentFile, convertStatus);
            }

            return result;
        }

        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <param name="skiingFile">The skiing file.</param>
        /// <param name="rowsNumber">The rows number.</param>
        /// <param name="columnsNumber">The columns number.</param>
        /// <returns></returns>
        private static int[,] ReadFile(StreamReader skiingFile, int rowsNumber, int columnsNumber)
        {
            int[,] contentFile = null;

            try
            {
                string line = string.Empty;
                contentFile = new int[rowsNumber, columnsNumber];
                int a = 0;

                while ((line = skiingFile.ReadLine()) != null)
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
