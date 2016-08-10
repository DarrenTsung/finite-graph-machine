using System.Collections;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public struct QuadBezierV3 {
    public Vector3 start;
    public Vector3 control;
    public Vector3 end;

    public QuadBezierV3(Vector3 start, Vector3 control, Vector3 end) {
      this.start = start;
      this.control = control;
      this.end = end;
    }
  }

  public static class QuadBezierV3Extensions {
    public static Vector3 Evaluate(this QuadBezierV3 q, float t) {
      return BezierUtil.Quad(q.start, q.control, q.end, t);
    }
  }
}