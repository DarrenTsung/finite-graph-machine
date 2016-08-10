using System.Collections;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public class DebugNodeDelegate : INodeDelegate {
    // PRAGMA MARK - INodeDelegate Implementation
    public void HandleEnter() {
      Debug.Log("Enter");
    }

    public void HandleExit() {
      Debug.Log("Exit");
    }


    // PRAGMA MARK - INodeDelegate.IDeepClonable<INodeDelegate> Implementation
    public INodeDelegate DeepClone() {
      return new DebugNodeDelegate();
    }
  }
}
