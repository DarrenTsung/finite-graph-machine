using System;
using System.Collections;
using System.Collections.Generic;

namespace DTFiniteGraphMachine {
  [Serializable]
  public class NodeTransition {
    public static NodeTransition DeepClone(NodeTransition nt) {
      NodeTransition clone = new NodeTransition();
      clone.targets = (NodeId[])nt.targets.Clone();
      clone.transition = Transition.DeepClone(nt.transition);

      return clone;
    }

    public NodeId[] targets = new NodeId[0];
    public Transition transition = new Transition();
  }
}