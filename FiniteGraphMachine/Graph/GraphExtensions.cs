using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public static class GraphExtensions {
    public static bool IsStartingNode(this Graph g, Node node) {
      return g.GetStartingNodes().Contains(node);
    }
  }
}