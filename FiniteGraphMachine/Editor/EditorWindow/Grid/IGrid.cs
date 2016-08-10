
using System.Collections;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public interface IGrid {
    Vector2 SnapToGrid(Vector2 point);
  }
}