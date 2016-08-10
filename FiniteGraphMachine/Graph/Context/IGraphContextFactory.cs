using System;
using System.Collections;

namespace DTFiniteGraphMachine {
  public interface IGraphContextFactory {
    IGraphContext MakeContext();
  }
}