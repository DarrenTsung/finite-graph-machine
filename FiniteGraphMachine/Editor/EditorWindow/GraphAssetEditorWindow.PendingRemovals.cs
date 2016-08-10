
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Internal
    private List<Tuple<Node, NodeTransition>> _pendingNodeTransitionRemovals = new List<Tuple<Node, NodeTransition>>();

    private void AddPendingNodeTransitionRemoval(Node node, NodeTransition nodeTransition) {
      Tuple<Node, NodeTransition> tuple = Tuple.Create(node, nodeTransition);
      this._pendingNodeTransitionRemovals.Add(tuple);
    }

    private void FlushPendingRemovals() {
      for (int i = this._pendingNodeTransitionRemovals.Count - 1; i >= 0; i--) {
        Tuple<Node, NodeTransition> tuple = this._pendingNodeTransitionRemovals[i];
        Node node = tuple.Item1;
        NodeTransition nodeTransition = tuple.Item2;

        this.TargetGraph.RemoveOutgoingTransitionForNode(node, nodeTransition);
        this.SetTargetDirty();
      }
      this._pendingNodeTransitionRemovals.Clear();
    }
  }
}