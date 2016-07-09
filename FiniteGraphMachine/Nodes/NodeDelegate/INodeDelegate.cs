using DT;
using System.Collections;

namespace DT.GameEngine {
  public interface INodeDelegate : IDeepClonable<INodeDelegate> {
    void HandleEnter();
    void HandleExit();
  }
}
