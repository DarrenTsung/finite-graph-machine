
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Internal
    private Node FindNodeContainingPoint(Vector2 canvasPoint) {
      foreach (Node node in this.TargetGraph.GetAllNodes().Reverse()) {
        Rect nodeRect = this.GetNodeRect(node);
        if (nodeRect.Contains(canvasPoint)) {
          return node;
        }
      }

      return null;
    }
  }
}