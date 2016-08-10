
using System.Collections;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public class Grid : IGrid {
    // PRAGMA MARK - Public Interface
    public Grid(int lineSpacing) {
      this.LineSpacing = lineSpacing;
    }

    public int LineSpacing {
      get; private set;
    }


    // PRAGMA MARK - IGrid Implementation
    public Vector2 SnapToGrid(Vector2 point) {
      float newX = Mathf.Round(point.x / this.LineSpacing) * this.LineSpacing;
      float newY = Mathf.Round(point.y / this.LineSpacing) * this.LineSpacing;
      return new Vector2(newX, newY);
    }
  }
}