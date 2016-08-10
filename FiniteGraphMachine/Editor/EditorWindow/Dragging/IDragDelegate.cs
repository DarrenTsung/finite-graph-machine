using System.Collections;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public interface IDragDelegate {
    void HandleDragStarted(Vector2 canvasPosition);
    void HandleDragUpdated(Vector2 canvasPosition);
    void HandleDragFinished();
  }
}