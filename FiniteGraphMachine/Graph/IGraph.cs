using System;
using System.Collections;

namespace DTFiniteGraphMachine {
  public interface IGraph {
    void Start();
    void Stop();
    void Reset();
  }
}