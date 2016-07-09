using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DT.GameEngine {
  [Serializable]
  public class NodeData {
    // PRAGMA MARK - Static Public Interface
    public static NodeData DeepClone(NodeData nodeData) {
      NodeData clone = new NodeData();
      clone.node = Node.DeepClone(nodeData.node);
      clone.outgoingTransitions = nodeData.outgoingTransitions.Select(nt => NodeTransition.DeepClone(nt)).ToList();

      return clone;
    }


    // PRAGMA MARK - Public Interface
    public Node node;
    public List<NodeTransition> outgoingTransitions;

    public NodeData() {}
    public NodeData(Node node) {
      this.node = node;
      this.outgoingTransitions = new List<NodeTransition>();
    }
  }
}