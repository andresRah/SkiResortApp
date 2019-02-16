namespace SkiResort.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using SkiResortRoute;

    /// <summary>
    /// Ski DFSP ath service.
    /// </summary>
    public interface ISkiDFSPathService
    {
        /// <summary>
        /// DFSOs the peration.
        /// </summary>
        /// <returns>The peration.</returns>
        /// <param name="i">The index.</param>
        /// <param name="j">J.</param>
        /// <param name="geoMap">Geo map.</param>
        Navigation DFSOperation(int i, int j, int[,] geoMap);

        /// <summary>
        /// DFSFs the length of the or max path.
        /// </summary>
        /// <returns>The or max path length.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="GeoMap">Geo map.</param>
        List<int> DFSForMaxPathLength(int x, int y, int[,] GeoMap);

        /// <summary>
        /// Gets the max path.
        /// </summary>
        /// <returns><c>true</c>, if max path was gotten, <c>false</c> otherwise.</returns>
        /// <param name="Path">Path.</param>
        /// <param name="Drop">Drop.</param>
        /// <param name="maxPathCoordXY">Max path coordinate xy.</param>
        /// <param name="maxPath">Max path.</param>
        /// <param name="maxDrop">Max drop.</param>
        bool GetMaxPath(int[,] Path, int[,] Drop, int[] maxPathCoordXY, ref int maxPath, ref int maxDrop);

        /// <summary>
        /// Processes the DFSA sync.
        /// </summary>
        /// <returns>The DFSA sync.</returns>
        /// <param name="fileStream">File stream.</param>
        Task<bool> ProcessDFSAsync(Stream fileStream);
    }
}