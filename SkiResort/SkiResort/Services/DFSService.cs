namespace SkiResort.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using SkiResortRoute;

    public class DFSService : ISkiDFSPathService
    {
        /// <summary>
        /// The rows count
        /// </summary>
        public int RowsCount { get; set; }

        /// <summary>
        /// The columns count
        /// </summary>
        public int ColumnsCount { get; set; }

        /// <summary>
        /// DFSOs the peration.
        /// </summary>
        /// <returns>The peration.</returns>
        /// <param name="i">The index.</param>
        /// <param name="j">J.</param>
        /// <param name="geoMap">Geo map.</param>
        public Navigation DFSOperation(int i, int j, int[,] geoMap)
        {
            Navigation pathAndDrop = new Navigation
            {
                PathLenght = 0,
                NavCoords = new NavigationCoordinates { xCoord = i, yCoord = j }
            };

            Navigation currentPathAndDrop = new Navigation
            {
                PathLenght = 0,
                NavCoords = new NavigationCoordinates()
            };

            // Searching UP ▲ Direction
            if (j > 0 && geoMap[i, j] > geoMap[i, j - 1])
            {
                currentPathAndDrop = DFSOperation(i, j - 1, geoMap: geoMap);

                // If current path value is larger then Update path full length
                if (currentPathAndDrop.PathLenght > pathAndDrop.PathLenght) pathAndDrop = currentPathAndDrop;
            }

            // Searching DOWN ▼ Direction
            if (j < (ColumnsCount - 1) && geoMap[i, j] > geoMap[i, j + 1])
            {
                currentPathAndDrop = DFSOperation(i, j + 1, geoMap: geoMap);

                if (currentPathAndDrop.PathLenght > pathAndDrop.PathLenght) pathAndDrop = currentPathAndDrop;
            }

            // Searching LEFT ◄ Direction
            if (i > 0 && geoMap[i, j] > geoMap[i - 1, j])
            {
                currentPathAndDrop = DFSOperation(i - 1, j, geoMap: geoMap);

                if (currentPathAndDrop.PathLenght > pathAndDrop.PathLenght) pathAndDrop = currentPathAndDrop;
            }

            // Searching RIGHT ► Direction
            if (i < (RowsCount - 1) && geoMap[i, j] > geoMap[i + 1, j])
            {
                currentPathAndDrop = DFSOperation(i + 1, j, geoMap: geoMap);

                if (currentPathAndDrop.PathLenght > pathAndDrop.PathLenght) pathAndDrop = currentPathAndDrop;
            }

            // For each recursion set a new Path Full Length
            pathAndDrop.PathLenght++;

            return pathAndDrop;
        }

        /// <summary>
        /// DFSFs the length of the or max path.
        /// </summary>
        /// <returns>The or max path length.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="GeoMap">Geo map.</param>
        public List<int> DFSForMaxPathLength(int x, int y, int[,] GeoMap)
        {
            List<int> list = new List<int>();
            List<int> currentPathList = new List<int>();

            // Searching UP ▲ Direction
            if (y > 0 && GeoMap[x, y] > GeoMap[x, y - 1])
            {
                currentPathList = DFSForMaxPathLength(x, y - 1, GeoMap);
                if (currentPathList.Count > list.Count)
                    list = currentPathList;
            }

            // Searching DOWN ▼ Direction
            if (y < (ColumnsCount - 1) && GeoMap[x, y] > GeoMap[x, y + 1])
            {
                currentPathList = DFSForMaxPathLength(x, y + 1, GeoMap);
                if (currentPathList.Count > list.Count)
                    list = currentPathList;
            }

            // Searching LEFT ◄ Direction
            if (x > 0 && GeoMap[x, y] > GeoMap[x - 1, y])
            {
                currentPathList = DFSForMaxPathLength(x - 1, y, GeoMap);
                if (currentPathList.Count > list.Count)
                    list = currentPathList;
            }

            // Searching RIGHT ► Direction
            if (x < (RowsCount - 1) && GeoMap[x, y] > GeoMap[x + 1, y])
            {
                currentPathList = DFSForMaxPathLength(x + 1, y, GeoMap);
                if (currentPathList.Count > list.Count)
                    list = currentPathList;
            }

            list.Add(y); list.Add(x);
            return list;
        }

        /// <summary>
        /// Gets the max path.
        /// </summary>
        /// <param name="Path">Path.</param>
        /// <param name="Drop">Drop.</param>
        /// <param name="maxPathCoordXY">Max path coordinate xy.</param>
        /// <param name="maxPath">Max path.</param>
        /// <param name="maxDrop">Max drop.</param>
        public bool GetMaxPath(int[,] Path, int[,] Drop, int[] maxPathCoordXY, ref int maxPath, ref int maxDrop)
        {
            try
            {
                for (int i = 0; i < RowsCount; i++)
                {
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        if (Path[i, j] > maxPath)
                        {   // if path[i][j] > maxPath, update maxPath and maxDrop
                            maxPath = Path[i, j];
                            maxDrop = Drop[i, j];

                            // Update coordinates too by the max point
                            maxPathCoordXY[0] = i;
                            maxPathCoordXY[1] = j;
                        }
                        if (Path[i, j] == maxPath)
                        {   // IF maxPaths are equals, compare the maxDrop
                            if (Drop[i, j] > maxDrop)
                            {
                                // if drop[i][j] > maxDrop, update maxDrop
                                maxDrop = Drop[i, j];

                                // Update coordinates too by the max point
                                maxPathCoordXY[0] = i;
                                maxPathCoordXY[1] = j;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Processes the DFSA sync.
        /// </summary>
        /// <returns>The DFSA sync.</returns>
        /// <param name="fileStream">File stream.</param>
        public virtual Task<bool> ProcessDFSAsync(Stream fileStream)
        {
            return Task.FromResult(false);
        }
    }
}
