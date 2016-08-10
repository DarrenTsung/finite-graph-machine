using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DTFiniteGraphMachine {
  [Serializable]
  public class Graph : IGraph {
    // PRAGMA MARK - Static Public Interface
    public static Graph DeepClone(Graph g) {
      Graph clone = new Graph();
      clone._startingContextParameters = new List<GraphContextParameter>(g._startingContextParameters);
      clone._graphData = GraphData.DeepClone(g._graphData);

      return clone;
    }


    // PRAGMA MARK - IGraph Implementation
    public void Start() {
      if (this._isActive) {
        Debug.LogWarning("Graph - Start called when already active!");
        return;
      }

      this._isActive = true;
      this.ResetContext();
      foreach (Node node in this.GetAllNodes()) {
        node.OnManualExit += this.HandleNodeManualExitTriggered;
      }

      Node[] startingNodes = this._graphData.GetStartingNodes();
      if (startingNodes == null) {
        return;
      }

      foreach (Node node in startingNodes) {
        this.AddActiveNode(node);
      }
      this.CommitActiveNodeChanges();

      this.CheckActiveNodesTransitions();
    }

    public void Stop() {
      if (!this._isActive) {
        Debug.LogWarning("Graph - Stop called when not active!");
        return;
      }

      this._isActive = false;
      this.Reset();
    }

    public void Reset() {
      foreach (Node node in this._activeNodes) {
        this.RemoveActiveNode(node);
      }
      this.CommitActiveNodeChanges();

      this._isActive = false;
      this.ResetContext();
      foreach (Node node in this.GetAllNodes()) {
        node.OnManualExit -= this.HandleNodeManualExitTriggered;
      }
    }


    public Node LoadNodeById(NodeId id) {
      return this._graphData.LoadNodeById(id);
    }

    public IList<Node> GetAllNodes() {
      return this._graphData.GetAllNodes();
    }

    public void SetStartingNodes(IList<Node> nodes) {
      this._graphData.SetStartingNodes(nodes);
    }

    public IList<Node> GetStartingNodes() {
      return this._graphData.GetStartingNodes();
    }

    public void AddStartingContextParameter(GraphContextParameter contextParameter) {
      this._startingContextParameters.Add(contextParameter);
    }

    public Node MakeNode() {
      return this._graphData.MakeNode();
    }

    public void RemoveNode(Node node) {
      this._graphData.RemoveNode(node);
    }

    public void AddOutgoingTransitionForNode(Node node, NodeTransition nodeTransition) {
      this._graphData.AddOutgoingTransitionForNode(node, nodeTransition);
    }

    public void RemoveOutgoingTransitionForNode(Node node, NodeTransition nodeTransition) {
      this._graphData.RemoveOutgoingTransitionForNode(node, nodeTransition);
    }

    public IList<NodeTransition> GetOutgoingTransitionsForNode(Node node) {
      IList<NodeTransition> nodeTransitions = this._graphData.GetOutgoingTransitionsForNode(node);

      foreach (NodeTransition nodeTransition in nodeTransitions) {
        Transition transition = nodeTransition.transition;
        if (!transition.HasContext()) {
          TransitionContext transitionContext = new TransitionContext {
            graphContext = this._context,
            nodeContext = new GraphNodeContext(this, node)
          };

          transition.ConfigureWithContext(transitionContext);
        }
      }

      return nodeTransitions;
    }


    public bool IsNodeActive(Node node) {
      int changeValue = this._activeNodeChangeMap.GetValue(node);
      bool shouldBeAdded = changeValue > 0;
      bool shouldBeRemoved = changeValue < 0;

      if (shouldBeAdded) {
        return true;
      }

      if (shouldBeRemoved) {
        return false;
      }

      if (this._activeNodes.Contains(node)) {
        return true;
      }

      return false;
    }


    // PRAGMA MARK - Internal
    [SerializeField] private GraphData _graphData = new GraphData();
    [SerializeField] private List<GraphContextParameter> _startingContextParameters = new List<GraphContextParameter>();

    private IGraphContext _context;
    private bool _isActive = false;

    private HashSet<Node> _activeNodes = new HashSet<Node>();
    private CountMap<Node> _activeNodeChangeMap = new CountMap<Node>();

    private void HandleNodeManualExitTriggered(Node node) {
      if (!this.IsNodeActive(node)) {
        Debug.LogWarning("HandleNodeManualExitTriggered - node that is not active manually exited!");
        return;
      }

      this.CheckTransitions(node);
    }

    private void CheckActiveNodesTransitions() {
      foreach (Node node in this._activeNodes) {
        this.CheckTransitions(node);
      }
      this.CommitActiveNodeChanges();
    }

    private void CheckTransitions(Node node) {
      IList<NodeTransition> nodeTransitions = this.GetOutgoingTransitionsForNode(node);
      foreach (NodeTransition nodeTransition in nodeTransitions) {
        if (!nodeTransition.transition.AreConditionsMet()) {
          continue;
        }

        nodeTransition.transition.HandleTransitionUsed();
        this.RemoveActiveNode(node);
        foreach (Node targetNode in nodeTransition.targets.Select(targetId => this._graphData.LoadNodeById(targetId))) {
          if (targetNode == null) {
            continue;
          }

          this.AddActiveNode(targetNode);
          this.CheckTransitions(targetNode);
        }

        break;
      }
    }

    private void RemoveActiveNode(Node node) {
      if (!this.IsNodeActive(node)) {
        Debug.LogError("Graph - RemoveActiveNode node was not active!");
        return;
      }

      this._activeNodeChangeMap.Decrement(node);
      node.HandleExit();
    }

    private void AddActiveNode(Node node) {
      if (node == null) {
        Debug.LogError("Graph - AddActiveNode node passed was null, stopping graph!");
        return;
      }

      if (this.IsNodeActive(node)) {
        return;
      }

      this._activeNodeChangeMap.Increment(node);
      node.HandleEnter();
    }

    private void CommitActiveNodeChanges() {
      foreach (KeyValuePair<Node, int> entry in this._activeNodeChangeMap) {
        Node node = entry.Key;

        bool shouldRemove = entry.Value < 0;
        bool shouldAdd = entry.Value > 0;

        if (shouldRemove) {
          this._activeNodes.Remove(node);
        }

        if (shouldAdd) {
          this._activeNodes.Add(node);
        }
      }

      this._activeNodeChangeMap.Clear();
    }

    private void ResetContext() {
      if (this._context != null) {
        this._context.OnContextUpdated -= this.CheckActiveNodesTransitions;
      }
      this._context = GraphContextFactoryLocator.MakeContext();
      this._context.OnContextUpdated += this.CheckActiveNodesTransitions;
      this._context.PopulateStartingContextParameters(this._startingContextParameters);
    }
  }
}