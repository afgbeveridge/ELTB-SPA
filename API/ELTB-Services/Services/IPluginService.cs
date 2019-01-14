using System.Collections.Generic;
using SharedObjects.Esoterica;

namespace ELTB.Services {

    public interface IPluginService {
        IEsotericInterpreter InterpreterFor(string language);
        IEnumerable<IEsotericInterpreter> RegisteredInterpreters { get; }
    }
}