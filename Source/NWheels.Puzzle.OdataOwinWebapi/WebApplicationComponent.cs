﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin.Hosting;
using NWheels.Hosting;
using NWheels.Logging;
using NWheels.UI;
using NWheels.UI.Endpoints;
using Owin;

namespace NWheels.Puzzle.OdataOwinWebapi
{
    internal class WebApplicationComponent : LifecycleEventListenerBase
    {
        private readonly IUiApplication _app;
        private readonly ILogger _logger;
        private readonly ILifetimeScope _container;
        private IDisposable _host = null;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public WebApplicationComponent(IWebAppEndpoint endpoint, Auto<ILogger> logger, IComponentContext componentContext)
        {
            _app = endpoint.App;
            _logger = logger.Instance;
            _container = (ILifetimeScope)componentContext;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public override void Activate()
        {
            const string url = "http://localhost:9000/";

            try
            {
                _host = WebApp.Start(url, ConfigureWebApplication);
                _logger.WebApplicationStarted(_app.GetType().Name, url);
            }
            catch ( Exception e )
            {
                _logger.WebApplicationFailedToStart(_app.GetType().Name, e);
                throw;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public override void Deactivate()
        {
            try
            {
                _host.Dispose();
                _logger.WebApplicationStopped(_app.GetType().Name);
            }
            catch ( Exception e )
            {
                _logger.WebApplicationFailedToStop(_app.GetType().Name, e);
                throw;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public void ConfigureWebApplication(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.DependencyResolver = new AutofacWebApiDependencyResolver(_container);

            app.UseAutofacMiddleware(_container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public interface ILogger : IApplicationEventLogger
        {
            [LogInfo]
            void WebApplicationStarted(string appName, string Url);
            [LogError]
            void WebApplicationFailedToStart(string appName, Exception e);
            [LogInfo]
            void WebApplicationStopped(string appName);
            [LogError]
            void WebApplicationFailedToStop(string appName, Exception e);
        }
    }
}
