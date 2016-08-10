using System;
using System.Collections;

namespace DTFiniteGraphMachine {
  public interface IGraphNodeContext {
    bool IsActive { get; }
    bool IsManuallyExited { get; }
  }
}