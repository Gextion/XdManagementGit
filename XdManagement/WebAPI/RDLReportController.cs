using System.Web.Http;

namespace EficienciaEnergetica.WebAPI
{
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class RDLReportController : ApiController
    {   
    }
}