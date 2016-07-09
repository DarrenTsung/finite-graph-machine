using DT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Internal
    private EditNodeTransitionState _editNodeTransitionState = new EditNodeTransitionState();

    private void StartEditingNodeTransition(Node node, NodeTransition nodeTransition) {
      this.SelectNode(node);
      this.SelectNodeTransition(nodeTransition);

      this._editNodeTransitionState.Configure(node, nodeTransition);
      this.CleanupCurrentState();
      this.SetCurrentState(this._editNodeTransitionState);
    }

    private class EditNodeTransitionState : IEditorWindowState {
      // PRAGMA MARK - Public Interface
      public void Configure(Node node, NodeTransition nodeTransition) {
        this._node = node;
        this._nodeTransition = nodeTransition;
      }


      // PRAGMA MARK - IEditorWindowState Implementation
      public GraphAssetEditorWindow Context { get; set; }

      public void Cleanup() {
        if (this._nodeTransition.targets.Length <= 0) {
          this.Context.TargetGraph.RemoveOutgoingTransitionForNode(this._node, this._nodeTransition);
          this.Context.SetTargetDirty();
        }

        this._node = null;
        this._nodeTransition = null;
      }

      public void Render() {
        this.Context.DrawGrid(this.Context.CanvasRect);
        this.Context.DrawTransitions();

        this.DrawEditingTransitionToMousePoint();

        this.Context.DrawNodes();
      }

      public void HandleEvent(Event currentEvent) {
        if (!currentEvent.isMouse) {
          return;
        }

        Vector2 mousePosition = currentEvent.mousePosition;

        bool leftMouseButton = currentEvent.button == 0;
        bool leftMouseButtonDown = leftMouseButton && currentEvent.type == EventType.MouseDown;

        bool rightMouseButton = currentEvent.button == 1;
        bool rightMouseButtonDown = rightMouseButton && currentEvent.type == EventType.MouseDown;

  			if (leftMouseButtonDown) {
          if (this.Context.CanvasRect.Contains(mousePosition)) {
            Node node = this.Context.FindNodeContainingPoint(mousePosition);
            if (node != null) {
              this.HandleNodeClicked(node);
            }
            currentEvent.Use();
          }
          return;
        }

        if (rightMouseButtonDown) {
          this.Context.GoToNormalState();
          currentEvent.Use();
        }
      }


      // PRAGMA MARK - Internal
      private Node _node;
      private NodeTransition _nodeTransition;

      private void HandleNodeClicked(Node node) {
        if (this._nodeTransition == null) {
          Debug.LogError("HandleNodeClicked - node transition is null!");
          return;
        }

        List<NodeId> targetIds = new List<NodeId>(this._nodeTransition.targets);
        if (targetIds.Contains(node.Id)) {
          targetIds.Remove(node.Id);
        } else {
          targetIds.Add(node.Id);
        }

        this._nodeTransition.targets = targetIds.ToArray();
        this.Context.SetTargetDirty();
      }

      private void DrawEditingTransitionToMousePoint() {
        if (this._node == null) {
          Debug.LogError("DrawEditingTransitionToMousePoint - node is null!");
          return;
        }

        Vector2 mousePosition = Event.current.mousePosition;
        NodeViewData nodeViewData = this.Context.GetViewDataForNode(this._node);
        TransitionViewData transitionViewData = nodeViewData.GetViewDataForTransition(this._nodeTransition.transition);
        this.Context.DrawTransitionFromPointToPoint(transitionViewData,
                                                    nodeViewData.position + this.Context._panner.Position,
                                                    mousePosition,
                                                    TransitionViewStyle.HIGHLIGHTED);
      }
    }
  }
}