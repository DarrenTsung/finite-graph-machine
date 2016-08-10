
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Internal
    private Node _selectedNode = null;

    private Node GetSelectedNode() {
      return this._selectedNode;
    }

    private void SelectNode(Node node) {
      this._selectedNode = node;
    }

    private bool IsNodeSelected(Node node) {
      return this._selectedNode == node;
    }

    private void DeselectCurrentNode() {
      this._selectedNode = null;
      this.DeselectCurrentNodeTransition();
    }
  }
}