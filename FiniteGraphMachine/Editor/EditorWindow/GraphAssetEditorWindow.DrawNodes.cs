using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Static
    private const float kNodeSelectedLineWeight = 6.0f;
    private const float kNodeShadowOffset = 3.0f;

    private static readonly Vector2 kNodeSize = new Vector2(8 * kGridLineSpacing, 4 * kGridLineSpacing);
    private static readonly Vector2 kNodePivot = new Vector2(0.5f, 0.5f);
    private static readonly Color kNodeColor = ColorUtil.HexStringToColor("#909090");
    private static readonly Color kNodeSelectedColor = ColorUtil.HexStringToColor("#e486ea");

    private static Texture2D _kNodeTexture;
    private static Texture2D kNodeTexture {
      get {
        if (_kNodeTexture == null) {
          _kNodeTexture = Texture2DUtil.CreateTextureWithColor(kNodeColor);
        }
        return _kNodeTexture;
      }
    }

    private static Texture2D _kNodeSelectedTexture;
    private static Texture2D kNodeSelectedTexture {
      get {
        if (_kNodeSelectedTexture == null) {
          _kNodeSelectedTexture = Texture2DUtil.CreateTextureWithColor(kNodeSelectedColor);
        }
        return _kNodeSelectedTexture;
      }
    }


    // PRAGMA MARK - Internal
    private void DrawNodes() {
      foreach (Node node in this.TargetGraph.GetAllNodes()) {
        this.DrawNode(node);
      }
    }

    private void DrawNode(Node node) {
      NodeViewData viewData = this.GetViewDataForNode(node);
      Rect nodeRect = this.GetNodeRect(node);

      if (this.IsNodeSelected(node)) {
        Rect expandedNodeRect = RectUtil.Expand(nodeRect, kNodeSelectedLineWeight);
        GUIStyle nodeSelectedStyle = GUIStyleUtil.StyleWithTexture(GUI.skin.box, kNodeSelectedTexture);
        GUI.Box(expandedNodeRect, "", nodeSelectedStyle);
      }

      Rect nodeShadowRect = nodeRect;
      nodeShadowRect.position = nodeShadowRect.position + new Vector2(kNodeShadowOffset, kNodeShadowOffset);
      GUI.Box(nodeShadowRect, "", (GUIStyle)"NodeShadow");

      GUIStyle nodeStyle = (GUIStyle)"Node";
      if (this.TargetGraph.IsStartingNode(node)) {
        nodeStyle = (GUIStyle)"StartingNode";
      }

      GUI.Box(nodeRect, viewData.name, nodeStyle);
    }

    private Rect GetNodeRect(Node node) {
      NodeViewData viewData = this.GetViewDataForNode(node);
      return RectUtil.MakeRect(viewData.position + this._panner.Position, kNodeSize, kNodePivot);
    }
  }
}