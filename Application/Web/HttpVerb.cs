using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Web
{
    /// <summary>
    /// Represents an HTTP verb.
    /// </summary>
    [Flags]
    public enum HttpVerb
    {
        /// <summary>
        /// GET
        /// </summary>
        Get,

        /// <summary>
        /// POST
        /// </summary>
        Post,

        /// <summary>
        /// PUT
        /// </summary>
        Put,

        /// <summary>
        /// DELETE
        /// </summary>
        Delete,

        /// <summary>
        /// OPTIONS
        /// </summary>
        Options,

        /// <summary>
        /// TRACE
        /// </summary>
        Trace,

        /// <summary>
        /// HEAD
        /// </summary>
        Head,

        /// <summary>
        /// PATCH
        /// </summary>
        Patch,
    }
}
