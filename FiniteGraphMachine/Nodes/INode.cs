using System;
using System.Collections;

namespace DTFiniteGraphMachine {
  public interface INode {
    event Action OnEnter;
    event Action OnExit;
    event Action<Node> OnManualExit;

    NodeId Id { get; }

    void HandleEnter();
    void HandleExit();
  }
}