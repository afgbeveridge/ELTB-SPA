using ELTB.Models;
using ELTB.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ELTB.Controllers {

  [Produces("application/json")]
  [Route("api/EsotericLanguage")]
  public class EsotericLanguageController : Controller {

    private IPluginService Service { get; set; }

    public EsotericLanguageController(IPluginService svc) {
      Service = svc;
    }

    [HttpGet("SupportedLanguages")]
    public IEnumerable<LanguageMetadata> SupportedLanguages() {
      return Service
              .RegisteredInterpreters
              .Select(l => LanguageMetadata.Create(l.Language, l.Summary, l.Url));
    }

  }
}
