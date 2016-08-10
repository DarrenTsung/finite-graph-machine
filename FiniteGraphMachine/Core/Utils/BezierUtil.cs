using System.Collections;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public static class BezierUtil {
    public static Vector2 Quad(Vector2 start, Vector2 control, Vector2 end, float t) {
      return (((1.0f - t) * (1.0f - t)) * start) + (2.0f * t * (1.0f - t) * control) + ((t * t) * end);
    }

    public static Vector3 Quad(Vector3 start, Vector3 control, Vector3 end, float t) {
      return (((1.0f - t) * (1.0f - t)) * start) + (2.0f * t * (1.0f - t) * control) + ((t * t) * end);
    }

    public static Vector2 Cubic(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, float t) {
      return ((((-start + (3 * (startTangent-endTangent)) + end) * t) + (3 * (start + endTangent) - (6 * startTangent))) * t + (3 * (startTangent - start))) * t + start;
    }

    public static Vector3 Cubic(Vector3 start, Vector3 startTangent, Vector3 end, Vector3 endTangent, float t) {
      return ((((-start + (3 * (startTangent-endTangent)) + end) * t) + (3 * (start + endTangent) - (6 * startTangent))) * t + (3 * (startTangent - start))) * t + start;
    }
  }
}