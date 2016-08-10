using System.Collections;

namespace DTFiniteGraphMachine {
  public interface INodeDelegate : IDeepClonable<INodeDelegate> {
    void HandleEnter();
    void HandleExit();
  }
}
