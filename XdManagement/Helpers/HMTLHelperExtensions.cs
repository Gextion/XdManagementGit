using System;
using System.Web.Mvc;

namespace EficienciaEnergetica
{
    public static class HMTLHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string controller2 = null, string controller3 = null, string controller4 = null,
                string controller5 = null, string controller6 = null, string controller7 = null, string controller8 = null,
                string action = null, string cssClass = null)
        {

            if (String.IsNullOrEmpty(cssClass)) 
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = string.Empty;

            if (String.IsNullOrEmpty(controller2))
                controller2 = string.Empty;

            if (String.IsNullOrEmpty(controller3))
                controller3 = string.Empty;

            if (String.IsNullOrEmpty(controller4))
                controller4 = string.Empty;

            if (String.IsNullOrEmpty(controller5))
                controller5 = string.Empty;

            if (String.IsNullOrEmpty(controller6))
                controller6 = string.Empty;

            if (String.IsNullOrEmpty(controller7))
                controller7 = string.Empty;

            if (String.IsNullOrEmpty(controller8))
                controller8 = string.Empty;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return (controller  == currentController || controller2 == currentController || controller3 == currentController || controller4 == currentController ||
                    controller5 == currentController || controller6 == currentController || controller7 == currentController || controller8 == currentController
                ) && action == currentAction ? cssClass : String.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

	}
}
