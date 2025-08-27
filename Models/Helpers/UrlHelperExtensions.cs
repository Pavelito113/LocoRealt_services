using Microsoft.AspNetCore.Mvc;

namespace LocoRealt.Models.Helpers
{
    public static class UrlHelperExtensions
    {
        public static string RemoveQueryParam(this IUrlHelper urlHelper, string paramName)
        {
            var request = urlHelper.ActionContext.HttpContext.Request;
            var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(request.QueryString.Value);

            queryParams.Remove(paramName);

            var newQueryString = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString("", queryParams);
            return $"{request.Path}{newQueryString}";
        }

        public static string RemoveQueryParams(this IUrlHelper urlHelper, params string[] paramNames)
        {
            var request = urlHelper.ActionContext.HttpContext.Request;
            var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(request.QueryString.Value);

            foreach (var paramName in paramNames)
            {
                queryParams.Remove(paramName);
            }

            var newQueryString = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString("", queryParams);
            return $"{request.Path}{newQueryString}";
        }
    }
}
