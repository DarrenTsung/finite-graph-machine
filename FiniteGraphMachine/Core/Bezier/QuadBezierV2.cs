using System.Collections;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public struct QuadBezierV2 {
    public Vector2 start;
    public Vector2 control;
    public Vector2 end;

    public QuadBezierV2(Vector2 start, Vector2 control, Vector2 end) {
      this.start = start;
      this.control = control;
      this.end = end;
    }

    public QuadBezierV2(Vector2 start, Vector2 end, Axis? axis = null, float tangentMidpointMultiplier = 0.7f) {
      this.start = start;
      this.end = end;

      Vector2 offset = this.end - this.start;
      // ex. A ---> B    ==    Direction.RIGHT
      Direction offsetDirection = DirectionUtil.ConvertVector2(offset);
      if (axis != null) {
        // override offsetDirection with axis if passed in
        // ex. offset = (1.0f, 2.0f) and axis is Axis.HORIZONTAL then axisOffset is (1.0f, 0.0f)
        Vector2 axisOffset = Vector2.Scale(axis.Value.Vector2Value(), offset);
        offsetDirection = DirectionUtil.ConvertVector2(axisOffset);
      }

      // ex. (Direction.RIGHT).Vector2Value()   ==   Vector2(1.0f, 0.0f)
      this.control = this.start + Vector2.Scale(offsetDirection.Vector2Value(), Vector2Util.Abs(offset));

      Vector2 midpoint = (this.start + this.end) / 2.0f;
      this.control = midpoint + (this.control - midpoint) * tangentMidpointMultiplier;
    }
  }

  public static class QuadBezierV2Extensions {
    public static Vector2 Evaluate(this QuadBezierV2 q, float t) {
      return BezierUtil.Quad(q.start, q.control, q.end, t);
    }
  }
}