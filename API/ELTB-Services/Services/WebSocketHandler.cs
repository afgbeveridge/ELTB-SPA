using System;
using System.Text;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using SharedObjects.Esoterica;
using System.Composition;

namespace ELTB.Services {
  public class WebSocketHandler : IWebSocketHandler {

    private WebSocket Channel { get; set; }
    private CancellationTokenSource LocalToken { get; set; }
    private IPluginService PluginHandler { get; set; }
    private string Language { get; set; }

    public WebSocketHandler(IPluginService pluginService) {
      PluginHandler = pluginService;
    }

    public Task<IWebSocketHandler> Host(WebSocket socket) {
      Channel = socket;
      LocalToken = new CancellationTokenSource();
      CancellationToken token = LocalToken.Token;
      return Task.FromResult((IWebSocketHandler)this);
    }

    public async Task Process() {
      LocalIOWrapper wrapper = new LocalIOWrapper(Channel, LocalToken);
      var bundle = await GetLanguageAndSource(wrapper);
      var processor = LocateProcessor(bundle.Language);
      await Interpret(processor, wrapper, bundle.Source);
      await Channel.CloseAsync(WebSocketCloseStatus.NormalClosure, "Execution complete", LocalToken.Token);
    }

    private async Task Interpret(IEsotericInterpreter interp, LocalIOWrapper wrapper, string src) {
      try {
        interp.Interpret(wrapper, new[] { src });
      }
      catch (Exception ex) {
        await wrapper.Write(ex.ToString());
      }
    }

    private async Task<SourceBundle> GetLanguageAndSource(LocalIOWrapper wrapper) {
      var src = await wrapper.Receive(false);
      // TODO: Assert
      var end = src.IndexOf(src.First(), 1);
      return new SourceBundle {
        Language = src.Substring(1, end - 1),
        Source = src.Substring(end + 1)
      };
    }

    private IEsotericInterpreter LocateProcessor(string language) {
      return PluginHandler.InterpreterFor(language);
    }

    private class SourceBundle {
      internal string Language { get; set; }
      internal string Source { get; set; }
    }

  }

}
