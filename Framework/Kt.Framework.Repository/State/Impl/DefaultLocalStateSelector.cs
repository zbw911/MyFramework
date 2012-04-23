 

using Kt.Framework.Repository.Context;

namespace Kt.Framework.Repository.State.Impl
{
    /// <summary>
    /// Default implementation of <see cref="ILocalStateSelector"/>.
    /// </summary>
    public class DefaultLocalStateSelector : ILocalStateSelector
    {
        readonly IContext _context;

        /// <summary>
        /// Default Constructor.
        /// Creates an instance of <see cref="DefaultLocalStateSelector"/> class.
        /// </summary>
        /// <param name="context">An instance of <see cref="IContext"/>.</param>
        public DefaultLocalStateSelector(IContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the <see cref="ILocalState"/> instance to use.
        /// </summary>
        /// <returns></returns>
        public ILocalState Get()
        {
            if (_context.IsWcfApplication)
                return new WcfLocalState(_context);
            if (_context.IsWebApplication)
                return new HttpLocalState(_context);
            return new ThreadLocalState();
        }
    }
}