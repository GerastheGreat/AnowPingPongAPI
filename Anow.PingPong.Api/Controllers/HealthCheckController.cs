using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Anow.PingPong.Api.Controllers
{
  [Route("api/HealthCheck")]
  public class HealthCheckController : Controller
  {
    private string _version = null;

    /// <summary>
    /// Basic application health endpoint
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public object Get()
    {
      return new { Version = Version };
    }

    private string Version
    {
      get
      {
        if (_version == null)
        {
          var netVersion = typeof(HealthCheckController).GetTypeInfo().Assembly.GetName().Version;

          _version = $"{netVersion.Major}.{netVersion.Minor}.{netVersion.Build}";
          _version += "-" + (netVersion.Revision == 0 ? "ephemeral" : "build" + netVersion.Revision.ToString("D3"));
        }

        return _version;
      }
    }
  }
}
