using DT;
using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  public interface IGrid {
    Vector2 SnapToGrid(Vector2 point);
  }
}