using System.Collections;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public class NodeDragger : IDragDelegate {
    public NodeDragger(NodeViewData nodeViewData, IGrid grid) {
      this._nodeViewData = nodeViewData;

      this._grid = grid;
    }

    // PRAGMA MARK - IDragDelegate Implementation
    public void HandleDragStarted(Vector2 canvasPosition) {
      this._startDragCanvasPosition = canvasPosition;
      this._startDragNodePosition = this._nodeViewData.position;
    }

    public void HandleDragUpdated(Vector2 canvasPosition) {
      Vector2 dragOffset = canvasPosition - this._startDragCanvasPosition;
      this._nodeViewData.position = this._startDragNodePosition + dragOffset;
      this._nodeViewData.position = this._grid.SnapToGrid(this._nodeViewData.position);
    }

    public void HandleDragFinished() {
      // do nothing for now
    }


    // PRAGMA MARK - Internal
    private Vector2 _startDragCanvasPosition;
    private Vector2 _startDragNodePosition;

    private NodeViewData _nodeViewData;

    private IGrid _grid;
  }
}