using DT;
using System;
using System.Collections;

namespace DT.GameEngine {
  public interface IGraphNodeContext {
    bool IsActive { get; }
    bool IsManuallyExited { get; }
  }
}