
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Static
    private const float kTransitionLineWidth = 4.0f;
    private const float kTransitionTangentMultiplier = 0.75f;


    // PRAGMA MARK - Internal
    private void DrawTransitions() {
      foreach (Node node in this.TargetGraph.GetAllNodes()) {
        this.DrawTransitionsForNode(node);
      }
    }

    private void DrawTransitionsForNode(Node node) {
      NodeViewData nodeViewData = this.GetViewDataForNode(node);
      IList<NodeTransition> nodeTransitions = this.TargetGraph.GetOutgoingTransitionsForNode(node);
      foreach (NodeTransition nodeTransition in nodeTransitions) {
        TransitionViewStyle transitionStyle = this.GetStyleForTransition(node, nodeTransition);
        TransitionViewData transitionViewData = nodeViewData.GetViewDataForTransition(nodeTransition.transition);

        IList<Node> targetNodes = nodeTransition.targets.Select(targetId => this.TargetGraph.LoadNodeById(targetId)).ToList();
        foreach (Node targetNode in targetNodes) {
          this.DrawTransitionFromNodeToNode(transitionViewData, node, targetNode, transitionStyle);
        }
      }
    }

    private TransitionViewStyle GetStyleForTransition(Node node, NodeTransition nodeTransition) {
      if (!this.IsNodeSelected(node)) {
        return TransitionViewStyle.NORMAL;
      }

      if (this.IsNodeTransitionSelected(nodeTransition)) {
        return TransitionViewStyle.HIGHLIGHTED;
      }

      return TransitionViewStyle.SEMI_HIGHLIGHTED;
    }

    private void DrawTransitionFromNodeToNode(TransitionViewData transitionViewData, Node node, Node targetNode, TransitionViewStyle style) {
      NodeViewData nodeViewData = this.GetViewDataForNode(node);
      NodeViewData targetNodeViewData = this.GetViewDataForNode(targetNode);

      this.DrawTransitionFromPointToPoint(transitionViewData,
                                          nodeViewData.position + this._panner.Position,
                                          targetNodeViewData.position + this._panner.Position,
                                          style);
    }

    private void DrawTransitionFromPointToPoint(TransitionViewData transitionViewData, Vector2 point, Vector2 targetPoint, TransitionViewStyle style) {
      Color transitionColor = TransitionViewStyleUtil.GetColor(style);
      GUIStyle transitionArrowStyle = TransitionViewStyleUtil.GetArrowStyle(style);

      CubicBezierV2 bezier = new CubicBezierV2(point, targetPoint, kTransitionTangentMultiplier);
      Handles.DrawBezier(bezier.start,
                         bezier.end,
                         bezier.startTangent,
                         bezier.endTangent,
                         transitionColor,
                         null,
                         kTransitionLineWidth);

      Vector3[] bezierPoints = Handles.MakeBezierPoints(bezier.start,
                                                        bezier.end,
                                                        bezier.startTangent,
                                                        bezier.endTangent,
                                                        division: 40);

      int midPointIndex = Mathf.FloorToInt(bezierPoints.Length / 2.0f);
      Vector2 midPointTangent = bezierPoints[midPointIndex + 1] - bezierPoints[midPointIndex];

      Vector2 midPoint = (point + targetPoint) / 2.0f;
      float rotationAngle = Vector2.Angle(Vector2.right, midPointTangent);
      if (midPointTangent.y < 0.0f) {
        rotationAngle *= -1.0f;
      }

      GUIUtility.RotateAroundPivot(rotationAngle, midPoint);
      GUI.Box(RectUtil.MakeRect(midPoint, new Vector2(10.0f, 10.0f), pivot: new Vector2(0.5f, 0.5f)), "", transitionArrowStyle);
      GUIUtility.RotateAroundPivot(-rotationAngle, midPoint);
    }
  }
}