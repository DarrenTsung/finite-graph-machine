using System;
using System.Collections;

namespace DTFiniteGraphMachine {
  public class GraphNodeContext : IGraphNodeContext {
    // PRAGMA MARK - IGraphNodeContext Implementation
    public bool IsActive {
      get { return this._graph.IsNodeActive(this._node); }
    }

    public bool IsManuallyExited {
      get { return this._node.IsManuallyExited; }
    }

    public GraphNodeContext(Graph graph, Node node) {
      this._node = node;
      this._graph = graph;
    }


    // PRAGMA MARK - Internal
    private Graph _graph;
    private Node _node;
  }
}