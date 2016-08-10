
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Internal
    private NormalState _normalState = new NormalState();

    private void GoToNormalState() {
      this.CleanupCurrentState();
      this.SetCurrentState(this._normalState);
    }

    private class NormalState : IEditorWindowState {
      // PRAGMA MARK - IEditorWindowState Implementation
      public GraphAssetEditorWindow Context { get; set; }

      public void Cleanup() {
      }

      public void Render() {
        this.Context.DrawGrid(this.Context.CanvasRect);
        this.Context.DrawTransitions();
        this.Context.DrawNodes();

        this.Context.DrawInspectorWindow();
      }

      public void HandleEvent(Event currentEvent) {
        if (!currentEvent.isMouse) {
          return;
        }

        Vector2 mousePosition = currentEvent.mousePosition;

        bool leftMouseButton = currentEvent.button == 0;
        bool rightMouseButton = currentEvent.button == 1;

        bool rightMouseButtonMouseDown = rightMouseButton && currentEvent.type == EventType.MouseDown;

        if (currentEvent.type == EventType.ContextClick || rightMouseButtonMouseDown) {
          if (this.Context.CanvasRect.Contains(mousePosition)) {
            Node node = this.Context.FindNodeContainingPoint(mousePosition);
            if (node == null) {
              this.Context.OpenNodeCreationMenu(currentEvent);
            } else {
              this.Context.OpenNodeContextMenu(currentEvent, node);
            }
            currentEvent.Use();
          }
          return;
        }

        if (leftMouseButton) {
          bool mouseInCanvas = this.Context.CanvasRect.Contains(mousePosition);
          if (mouseInCanvas) {
            this.HandleLeftMouseButtonEventInCanvas(currentEvent);
            currentEvent.Use();
          }
          return;
        }
      }

      private void HandleLeftMouseButtonEventInCanvas(Event currentEvent) {
        Vector2 mousePosition = currentEvent.mousePosition;

        Node node = this.Context.FindNodeContainingPoint(mousePosition);
        if (currentEvent.type == EventType.MouseDown) {
          if (node != null) {
            this.Context.SelectNode(node);
            this.Context.StartDraggingNode(node, mousePosition);
          } else {
            if (currentEvent.modifiers == EventModifiers.Alt) {
              this.Context.StartDraggingPanner(mousePosition);
            }
            this.Context.DeselectCurrentNode();
          }
        } else if (currentEvent.type == EventType.MouseDrag) {
          this.Context.UpdateDragging(mousePosition);
        } else if (currentEvent.type == EventType.MouseUp) {
          this.Context.StopDragging();
        }
      }
    }
  }
}