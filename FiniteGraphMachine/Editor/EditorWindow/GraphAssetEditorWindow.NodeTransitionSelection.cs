using DT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Internal
    private NodeTransition _selectedNodeTransition = null;

    private NodeTransition GetSelectedNodeTransition() {
      return this._selectedNodeTransition;
    }

    private void SelectNodeTransition(NodeTransition nodeTransition) {
      this._selectedNodeTransition = nodeTransition;
    }

    private bool IsNodeTransitionSelected(NodeTransition nodeTransition) {
      return this._selectedNodeTransition == nodeTransition;
    }

    private void DeselectCurrentNodeTransition() {
      this._selectedNodeTransition = null;
    }
  }
}