using System.Collections;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public struct CubicBezierV2 {
    public Vector2 start;
    public Vector2 startTangent;
    public Vector2 end;
    public Vector2 endTangent;

    public CubicBezierV2(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent) {
      this.start = start;
      this.startTangent = startTangent;
      this.end = end;
      this.endTangent = endTangent;
    }

    public CubicBezierV2(Vector2 start, Vector2 end, float tangentMultiplier = 0.7f) {
      this.start = start;
      this.end = end;

      Vector2 offset = end - start;
      // ex. A ---> B    ==    Direction.RIGHT
      Direction offsetDirection = DirectionUtil.ConvertVector2(offset);

      // ex. (Direction.RIGHT).Vector2Value()   ==   Vector2(1.0f, 0.0f)
      Vector2 startTangentOffset = Vector2.Scale(offsetDirection.Vector2Value(), Vector2Util.Abs(offset) * tangentMultiplier);
      Vector2 endTangentOffset = -startTangentOffset;

      this.startTangent = this.start + startTangentOffset;
      this.endTangent = this.end + endTangentOffset;
    }
  }

  public static class CubicBezierV2Extensions {
    public static Vector2 Evaluate(this CubicBezierV2 q, float t) {
      return BezierUtil.Cubic(q.start, q.startTangent, q.end, q.endTangent, t);
    }
  }
}