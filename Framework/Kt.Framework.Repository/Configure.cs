 

using System;
using Kt.Framework.Repository.Configuration;

namespace Kt.Framework.Repository
{
    /// <summary>
    /// Static configuration class that allows configuration of Kt.Framework.Repository services.
    /// </summary>
    public static class Configure
    {
        /// <summary>
        /// Entry point to Kt.Framework.Repository configuration.
        /// </summary>
        /// <param name="containerAdapter">The <see cref="IContainerAdapter"/> instance to use
        /// for component registration.</param>
        /// <returns>An instance of <see cref="INCommonConfig"/> that can be used to configure
        /// Kt.Framework.Repository configuration.</returns> 
        public static INCommonConfig Using(IContainerAdapter containerAdapter)
        {
            Guard.Against<ArgumentNullException>(containerAdapter == null,
                                                 "Expected a non-null IContainerAdapter implementation.");
            return new NCommonConfig(containerAdapter);
        }
    }
}