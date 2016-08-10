using UnityEngine;

namespace DTFiniteGraphMachine {
  public static class Vector2Util {
    public static Vector2 Abs(Vector2 v) {
      return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
    }

    public static Vector2 InverseScale(Vector2 a, Vector2 b) {
      float newX = b.x == 0 ? float.MaxValue : (a.x / b.x);
      float newY = b.y == 0 ? float.MaxValue : (a.y / b.y);
      return new Vector2(newX, newY);
    }
  }
}