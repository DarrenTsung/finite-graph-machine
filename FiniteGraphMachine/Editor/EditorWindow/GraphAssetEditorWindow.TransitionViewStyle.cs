
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    private enum TransitionViewStyle {
      NORMAL = 1,
      SEMI_HIGHLIGHTED = 2,
      HIGHLIGHTED = 3
    }

    private static class TransitionViewStyleUtil {
      public static Color GetColor(TransitionViewStyle style) {
        switch (style) {
          case TransitionViewStyle.SEMI_HIGHLIGHTED:
            return ColorUtil.HexStringToColor("#EFD9A6");
          case TransitionViewStyle.HIGHLIGHTED:
            return ColorUtil.HexStringToColor("#EEBE4D");
          case TransitionViewStyle.NORMAL:
          default:
            return ColorUtil.HexStringToColor("#FFFFFF");
        }
      }

      public static GUIStyle GetArrowStyle(TransitionViewStyle style) {
        switch (style) {
          case TransitionViewStyle.SEMI_HIGHLIGHTED:
            return (GUIStyle)"TransitionArrowSemiHighlighted";
          case TransitionViewStyle.HIGHLIGHTED:
            return (GUIStyle)"TransitionArrowHighlighted";
          case TransitionViewStyle.NORMAL:
          default:
            return (GUIStyle)"TransitionArrowNormal";
        }
      }
    }
  }
}