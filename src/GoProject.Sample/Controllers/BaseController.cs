using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace GoProject.Sample.Controllers
{
    public class BaseController : Controller
    {
        public string GetUrlCulture(HttpRequestBase request, bool getDefultCultureIfNotExist = true)
        {
            var path = request.Path;
            if (path.IndexOf("/", StringComparison.Ordinal) != 0 || path.Length < 3)
                return getDefultCultureIfNotExist ? "en" : null;

            return path.Substring(1, 2);
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = GetUrlCulture(Request); // read route culture?

            CultureInfo culture;
            try
            {
                culture = new CultureInfo(cultureName);
            }
            catch (CultureNotFoundException)
            {
                culture = new CultureInfo("en");
            }

            // Modify current thread's cultures  
            CultureInfo.DefaultThreadCurrentUICulture =
                CultureInfo.DefaultThreadCurrentCulture =
                    Thread.CurrentThread.CurrentCulture =
                        Thread.CurrentThread.CurrentUICulture = culture;

            RouteData.Values["culture"] = cultureName;  // set culture

            return base.BeginExecuteCore(callback, state);
        }
    }
}