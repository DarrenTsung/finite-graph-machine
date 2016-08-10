
using System.Collections;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public class Panner : IDragDelegate {
    public Vector2 Position {
      get { return this._currentPanPosition; }
    }

    // PRAGMA MARK - IDragDelegate Implementation
    public void HandleDragStarted(Vector2 canvasPosition) {
      this._startDragCanvasPosition = canvasPosition;
      this._startDragPanPosition = this._currentPanPosition;
    }

    public void HandleDragUpdated(Vector2 canvasPosition) {
      Vector2 dragOffset = canvasPosition - this._startDragCanvasPosition;
      this._currentPanPosition = this._startDragPanPosition + dragOffset;
    }

    public void HandleDragFinished() {
      // do nothing for now
    }


    // PRAGMA MARK - Internal
    private Vector2 _startDragCanvasPosition;
    private Vector2 _startDragPanPosition;

    private Vector2 _currentPanPosition;
  }
}