using System.Collections.Generic;

namespace SkiResort.Models
{
    /// <summary>
    /// Ski DFSR esponse.
    /// </summary>
    public class SkiDFSResponse
    {
        /// <summary>
        /// Gets or sets the max path.
        /// </summary>
        /// <value>The max path.</value>
        public int MaxPath { get; set; }

        /// <summary>
        /// Gets or sets the max drop.
        /// </summary>
        /// <value>The max drop.</value>
        public int MaxDrop { get; set; }

        /// <summary>
        /// Gets or sets the result points list.
        /// </summary>
        /// <value>The result points list.</value>
        public List<PointsSkiDFSResponse> ResultPointsList { get; set; }
    }

    /// <summary>
    /// Glosslist.
    /// </summary>
    public class PointsSkiDFSResponse
    {
        /// <summary>
        /// Gets or sets the XC oord.
        /// </summary>
        /// <value>The XC oord.</value>
        public int XCoord { get; set; }

        /// <summary>
        /// Gets or sets the YC oord.
        /// </summary>
        /// <value>The YC oord.</value>
        public int YCoord { get; set; }

        /// <summary>
        /// Gets or sets the altitude.
        /// </summary>
        /// <value>The altitude.</value>
        public int Altitude { get; set; }
    }
}
