using System.Collections;
﻿using UnityEngine;

namespace DTFiniteGraphMachine {
	public enum Axis {
		HORIZONTAL = 1,
		VERTICAL = 2
	}

	public static class AxisExtensions {
    public static Vector2 Vector2Value(this Axis a) {
      return (a == Axis.HORIZONTAL) ? new Vector2(1.0f, 0.0f) : new Vector2(0.0f, 1.0f);
    }

    public static float ApplicableValue(this Axis a, Vector2 v) {
      return (a == Axis.HORIZONTAL) ? v.x : v.y;
    }
	}
}
