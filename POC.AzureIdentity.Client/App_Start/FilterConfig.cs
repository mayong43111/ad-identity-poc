using System.Web;
using System.Web.Mvc;

namespace POC.AzureIdentity.Client
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
