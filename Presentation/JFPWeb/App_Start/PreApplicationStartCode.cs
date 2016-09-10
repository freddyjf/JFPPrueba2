using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace Precedente.App_Start
{
    public class PreApplicationStartCode
    {
        private static bool _isStarting;

        public static void PreStart()
        {
            if (_isStarting) return;

            _isStarting = true;

            DynamicModuleUtility.RegisterModule(typeof(Prerender.io.PrerenderModule));
        }
    }
}