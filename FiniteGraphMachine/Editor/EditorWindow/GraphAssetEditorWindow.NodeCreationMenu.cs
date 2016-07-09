using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Internal
    private void OpenNodeCreationMenu(Event currentEvent) {
      GenericMenu nodeCreationMenu = new GenericMenu();
      nodeCreationMenu.AddItem(new GUIContent("New Node"), false, this.MakeNode, currentEvent.mousePosition);
      nodeCreationMenu.ShowAsContext();
    }

    private void MakeNode(object mousePosition) {
      Node newNode = this.TargetGraph.MakeNode();
      NodeViewData nodeViewData = this.TargetGraphViewData.LoadViewDataForNode(newNode);
      nodeViewData.position = this._grid.SnapToGrid((Vector2)mousePosition + this._panner.Position);

      this.SetTargetDirty();
    }
  }
}