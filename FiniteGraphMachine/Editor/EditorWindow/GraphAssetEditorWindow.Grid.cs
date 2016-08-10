
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Static
    private const int kGridLineSpacing = 13;        // number of pixels between grid line
    private const int kSecondaryGridSpacing = 5;    // number of grid lines between secondary grid line

    private static readonly Color kLightLineColor = new Color(0.0f, 0.0f, 0.0f, 0.15f);
    private static readonly Color kDarkLineColor = new Color(0.0f, 0.0f, 0.0f, 0.25f);

    private static readonly Color kGridBackgroundColor = ColorUtil.HexStringToColor("#323232");
    private static Texture2D _kGridBackgroundTexture;
    private static Texture2D kGridBackgroundTexture {
      get {
        if (_kGridBackgroundTexture == null) {
          _kGridBackgroundTexture = Texture2DUtil.CreateTextureWithColor(kGridBackgroundColor);
        }
        return _kGridBackgroundTexture;
      }
    }


    // PRAGMA MARK - Internal
    private Grid _grid = new Grid(kGridLineSpacing);

		private void DrawGrid(Rect rect) {
      Vector2 offset = this._panner.Position;

			GUI.Box(rect, "", GUIStyleUtil.StyleWithTexture(kGridBackgroundTexture));

      Color oldColor = Handles.color;

      int horizontalLinesOffset = Mathf.FloorToInt(offset.x / this._grid.LineSpacing);
      int verticalLinesOffset = Mathf.FloorToInt(offset.y / this._grid.LineSpacing);

      offset = new Vector2(Mathf.Repeat(offset.x, this._grid.LineSpacing),
                           Mathf.Repeat(offset.y, this._grid.LineSpacing));

      int numberOfVerticalLines = Mathf.CeilToInt(rect.width / this._grid.LineSpacing) + 1;
      int numberOfHorizontalLines = Mathf.CeilToInt(rect.height / this._grid.LineSpacing) + 1;

      for (float i = 0; i < numberOfVerticalLines; i++) {
        float x = i * this._grid.LineSpacing;
        x += offset.x - this._grid.LineSpacing;

        Handles.color = ((i - horizontalLinesOffset) % kSecondaryGridSpacing == 0) ? kDarkLineColor : kLightLineColor;
        Handles.DrawLine(new Vector3(x, 0.0f, 0.0f), new Vector3(x, rect.height, 0.0f));
      }

      for (float i = 0; i < numberOfHorizontalLines; i++) {
        float y = i * this._grid.LineSpacing;
        y += offset.y - this._grid.LineSpacing;

        Handles.color = ((i - verticalLinesOffset) % kSecondaryGridSpacing == 0) ? kDarkLineColor : kLightLineColor;
        Handles.DrawLine(new Vector3(0.0f, y, 0.0f), new Vector3(rect.width, y, 0.0f));
      }

			Handles.color = oldColor;
		}
  }
}