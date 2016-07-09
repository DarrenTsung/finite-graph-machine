using DT;
using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  public interface IDragDelegate {
    void HandleDragStarted(Vector2 canvasPosition);
    void HandleDragUpdated(Vector2 canvasPosition);
    void HandleDragFinished();
  }
}