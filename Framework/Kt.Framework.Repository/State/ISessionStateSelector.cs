 

namespace Kt.Framework.Repository.State
{
    /// <summary>
    /// Interface that is implemented by a custom selector that creates instances of <see cref="ISessionState"/>.
    /// </summary>
    public interface ISessionStateSelector
    {
        /// <summary>
        /// Gets the implementation of <see cref="ISessionState"/> to use.
        /// </summary>
        /// <returns></returns>
        ISessionState Get();
    }
}