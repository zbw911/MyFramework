 

using Kt.Framework.Repository.Context;

namespace Kt.Framework.Repository.State.Impl
{
    /// <summary>
    /// Default implementation of <see cref="ISessionStateSelector"/>.
    /// </summary>
    public class DefaultSessionStateSelector : ISessionStateSelector
    {
        readonly IContext _context;

        /// <summary>
        /// Default Constructor.
        /// Creates a new instance of <see cref="DefaultLocalStateSelector"/> class.
        /// </summary>
        /// <param name="context">An instance of <see cref="IContext"/>.</param>
        public DefaultSessionStateSelector(IContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the implementation of <see cref="ISessionState"/> to use.
        /// </summary>
        /// <returns></returns>
        public ISessionState Get()
        {
            if (_context.IsWcfApplication)
            {
                if (_context.IsAspNetCompatEnabled)
                    return new HttpSessionState(_context);
                return new WcfSessionState(_context);
            }
            if (_context.IsWebApplication)
                return new HttpSessionState(_context);
            return null;
        }
    }
}