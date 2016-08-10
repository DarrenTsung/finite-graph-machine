
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Internal
    private bool _dragging = false;
    private IDragDelegate _currentDragDelegate;

    private void StartDraggingNode(Node node, Vector2 startCanvasPosition) {
      if (this._dragging) {
        Debug.LogWarning("StartDraggingNode - already dragging!");
        return;
      }

      if (node == null) {
        Debug.LogWarning("StartDraggingNode - node is null!");
        return;
      }

      this._currentDragDelegate = new NodeDragger(this.GetViewDataForNode(node), this._grid);

      this.StartDragging(startCanvasPosition);
    }

    private void StartDraggingPanner(Vector2 startCanvasPosition) {
      this._currentDragDelegate = this._panner;
      this.StartDragging(startCanvasPosition);
    }


    private void StartDragging(Vector2 startCanvasPosition) {
      this._dragging = true;
      this._currentDragDelegate.HandleDragStarted(startCanvasPosition);
    }

    private void UpdateDragging(Vector2 updatedCanvasPosition) {
      if (!this._dragging) {
        return;
      }

      this._currentDragDelegate.HandleDragUpdated(updatedCanvasPosition);
    }

    private void StopDragging() {
      if (!this._dragging) {
        return;
      }

      this._currentDragDelegate.HandleDragFinished();
      this.SetTargetDirty();

      this._dragging = false;
    }
  }
}