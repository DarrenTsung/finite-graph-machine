#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public static class GUIStyleUtil {
    private static readonly GUIStyle kEmptyGUIStyle = new GUIStyle();
    private static Dictionary<Texture2D, GUIStyle> _cachedTextureStyles = new Dictionary<Texture2D, GUIStyle>();

    public static GUIStyle StyleWithTexture(Texture2D texture) {
      return GUIStyleUtil.StyleWithTexture(kEmptyGUIStyle, texture);
    }

    public static GUIStyle StyleWithTexture(GUIStyle baseStyle, Texture2D texture) {
      GUIStyle style = GUIStyleUtil._cachedTextureStyles.SafeGet(texture);
      if (style == null) {
        style = new GUIStyle(baseStyle);
        style.normal.background = texture;
      }

      return style;
    }
  }
}
#endif