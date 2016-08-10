using System.Collections;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public struct CubicBezierV3 {
    public Vector3 start;
    public Vector3 startTangent;
    public Vector3 end;
    public Vector3 endTangent;

    public CubicBezierV3(Vector3 start, Vector3 startTangent, Vector3 end, Vector3 endTangent) {
      this.start = start;
      this.startTangent = startTangent;
      this.end = end;
      this.endTangent = endTangent;
    }
  }

  public static class CubicBezierV3Extensions {
    public static Vector3 Evaluate(this CubicBezierV3 q, float t) {
      return BezierUtil.Cubic(q.start, q.startTangent, q.end, q.endTangent, t);
    }
  }
}