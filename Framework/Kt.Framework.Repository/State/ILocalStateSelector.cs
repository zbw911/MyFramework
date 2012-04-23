 

namespace Kt.Framework.Repository.State
{
    /// <summary>
    /// Interface that is implemented by a custom selector that creates instances of <see cref="ILocalState"/>.
    /// </summary>
    public interface ILocalStateSelector
    {
        /// <summary>
        /// Gets the implementation of <see cref="ILocalState"/> to use.
        /// </summary>
        /// <returns></returns>
        ILocalState Get();
    }
}