using DT;
using System;
using System.Collections;

namespace DT.GameEngine {
  public interface IGraphContextFactory {
    IGraphContext MakeContext();
  }
}