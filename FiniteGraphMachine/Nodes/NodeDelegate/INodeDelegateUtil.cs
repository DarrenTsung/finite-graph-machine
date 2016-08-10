using System;
using System.Collections;
using System.Linq;

namespace DTFiniteGraphMachine {
  public static class INodeDelegateUtil {
    // PRAGMA MARK - Static Public Interface
    public static Type[] ImplementationTypes {
      get { return INodeDelegateUtil._implementationTypes; }
    }


    // PRAGMA MARK - Static Internal
    private static Type[] _implementationTypes;

    static INodeDelegateUtil() {
      INodeDelegateUtil._implementationTypes =
          (from assembly in AppDomain.CurrentDomain.GetAssemblies()
           from type in assembly.GetTypes()
           where typeof(INodeDelegate).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract
           select type).ToArray();
    }
  }
}
