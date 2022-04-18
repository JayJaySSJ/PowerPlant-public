using Owin;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PowerPlant.WebApi.Server
{
    internal class StartUp
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.EnableCors(new EnableCorsAttribute("*", "*", "*", "X-Custom-Header"));
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}");

            appBuilder.UseWebApi(config);
        }
    }
}