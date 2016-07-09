using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Public Interface
    public void OnGUI() {
			if (EditorApplication.isCompiling) {
				this.ShowNotice("Compiling, please wait..");
				return;
			}

      if (this.TargetGraph == null) {
        this.ShowNotice("Select a graph asset to edit!");
        return;
      }

      if (EditorWindow.mouseOverWindow == this) {
        this._willRepaint = true;
      }

			GUI.skin = this._skin;
      this.RecomputeCanvasRect();

			GUI.BeginGroup(this.CanvasRect);
        this._currentState.Render();
			GUI.EndGroup();

      this._currentState.HandleEvent(Event.current);

      if (this._willRepaint) {
        this.Repaint();
        this._willRepaint = false;
      }
      this.FlushPendingRemovals();
      this.SaveChanges();
    }


    // PRAGMA MARK - Internal
    private bool _willRepaint;

    private Rect _canvasRect;
    private Rect CanvasRect {
      get { return this._canvasRect; }
    }

    private void RecomputeCanvasRect() {
      this._canvasRect = new Rect(0.0f, 0.0f, this.position.width, this.position.height);
    }

    private void ShowNotice(string noticeText) {
      this.ShowNotification(new GUIContent(noticeText));
    }
  }
}