using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DTFiniteGraphMachine {
  [Serializable]
  public class GraphData : ISerializationCallbackReceiver {
    // PRAGMA MARK - Static Public Interface
    public static GraphData DeepClone(GraphData data) {
      GraphData clone = new GraphData();
      clone._startingNodeIds = (NodeId[])data._startingNodeIds.Clone();
      clone._nodeDatas = data._nodeDatas.Select(nodeData => NodeData.DeepClone(nodeData)).ToList();

      return clone;
    }


    // PRAGMA MARK - Static Internal
    private static readonly NodeTransition[] kEmptyNodeTransitionArray = new NodeTransition[0];


    // PRAGMA MARK - Public Interface
    public Node LoadNodeById(NodeId id) {
      if (!this.HasNodeWithId(id)) {
        Debug.LogWarning("GraphData - LoadNodeById has no node with id: " + id + "!");
        return null;
      }
      return this.CachedNodeDataMapping[id].node;
    }

    public IList<Node> GetAllNodes() {
      return this.CachedAllNodes;
    }

    public Node[] GetStartingNodes() {
      if (this._startingNodeIds == null) {
        return null;
      }
      return (from id in this._startingNodeIds select this.LoadNodeById(id)).ToArray();
    }

    public void SetStartingNodes(IList<Node> nodes) {
      this._startingNodeIds = nodes.Select(node => node.Id).ToArray();
    }

    public Node MakeNode() {
      Node node = new Node(this.CachedHighestNodeId + 1);
      this._nodeDatas.Add(new NodeData(node));

      // If no starting nodes, set starting node to first node created
      if (this._startingNodeIds == null) {
        this._startingNodeIds = new NodeId[] { node.Id };
      }

      this.ClearCached();
      return node;
    }

    public void RemoveNode(Node node) {
      if (!this.HasNodeWithId(node.Id)) {
        Debug.LogWarning("GraphData - RemoveNode has no node with id: " + node.Id + "!");
        return;
      }

      NodeData nodeData = this.CachedNodeDataMapping[node.Id];
      bool successful = this._nodeDatas.Remove(nodeData);
      if (!successful) {
        Debug.LogWarning("GraphData - RemoveNode called with node not contained in the graph!");
        return;
      }

      // remove all transitions to this node
      foreach (NodeData otherNodeData in this._nodeDatas) {
        List<NodeTransition> nodeTransitionsToRemove = new List<NodeTransition>();

        foreach (NodeTransition transition in otherNodeData.outgoingTransitions) {
          if (!transition.targets.Contains(node.Id)) {
            continue;
          }

          transition.targets = transition.targets.Where(id => id != node.Id).ToArray();
          if (transition.targets.Length == 0) {
            nodeTransitionsToRemove.Add(transition);
          }
        }

        otherNodeData.outgoingTransitions.RemoveRange(nodeTransitionsToRemove);
      }

      this.ClearCached();
    }

    public IList<NodeTransition> GetOutgoingTransitionsForNode(Node node) {
      if (!this.HasNodeWithId(node.Id)) {
        Debug.LogWarning("GraphData - GetOutgoingTransitionsForNode has no node with id: " + node.Id + "!");
        return kEmptyNodeTransitionArray;
      }

      return this.CachedNodeDataMapping[node.Id].outgoingTransitions;
    }

    public void AddOutgoingTransitionForNode(Node node, NodeTransition nodeTransition) {
      if (!this.HasNodeWithId(node.Id)) {
        Debug.LogWarning("GraphData - AddOutgoingTransitionForNode has no node with id: " + node.Id + "!");
        return;
      }

      this.CachedNodeDataMapping[node.Id].outgoingTransitions.Add(nodeTransition);
    }

    public void RemoveOutgoingTransitionForNode(Node node, NodeTransition nodeTransition) {
      if (!this.HasNodeWithId(node.Id)) {
        Debug.LogWarning("GraphData - RemoveOutgoingTransitionForNode has no node with id: " + node.Id + "!");
        return;
      }

      this.CachedNodeDataMapping[node.Id].outgoingTransitions.Remove(nodeTransition);
    }


    // PRAGMA MARK - ISerializationCallbackReceiver Implementation
    public void OnAfterDeserialize() {
      this.ClearCached();
    }

    public void OnBeforeSerialize() {
      this.ClearCached();
    }


    // PRAGMA MARK - Internal
    [SerializeField] List<NodeData> _nodeDatas = new List<NodeData>();
    [SerializeField] NodeId[] _startingNodeIds;

    private int? _cachedHighestNodeId;
    private int CachedHighestNodeId {
      get {
        if (this._cachedHighestNodeId == null) {
          if (this._nodeDatas.IsNullOrEmpty()) {
            this._cachedHighestNodeId = 0;
          } else {
            this._cachedHighestNodeId = this._nodeDatas.Select(data => data.node.Id).Max();
          }
        }

        return this._cachedHighestNodeId.Value;
      }
    }

    private Dictionary<NodeId, NodeData> _cachedNodeDataMapping;
    private Dictionary<NodeId, NodeData> CachedNodeDataMapping {
      get {
        if (this._cachedNodeDataMapping == null) {
          this._cachedNodeDataMapping = new Dictionary<NodeId, NodeData>();
          foreach (NodeData nodeData in this._nodeDatas) {
            this._cachedNodeDataMapping[nodeData.node.Id] = nodeData;
          }
        }

        return this._cachedNodeDataMapping;
      }
    }

    private List<Node> _cachedAllNodes;
    private List<Node> CachedAllNodes {
      get {
        if (this._cachedAllNodes == null) {
          this._cachedAllNodes = this._nodeDatas.Select(data => data.node).ToList();
        }
        return this._cachedAllNodes;
      }
    }

    private void ClearCached() {
      this._cachedNodeDataMapping = null;
      this._cachedHighestNodeId = null;
      this._cachedAllNodes = null;
    }

    private bool HasNodeWithId(NodeId id) {
      return this.CachedNodeDataMapping.ContainsKey(id);
    }
  }
}
