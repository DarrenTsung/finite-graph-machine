using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Static
    private static void ConfigureWindow(GraphComponent graphComponent) {
      if (graphComponent == null) {
        return;
      }

      GraphAssetEditorWindow.ConfigureWindow(graphComponent.graphAsset);
    }

    private static void ConfigureWindow(GraphAsset graphAsset) {
      if (graphAsset == null) {
        return;
      }

      GraphAssetEditorWindow window = EditorWindow.GetWindow(typeof(GraphAssetEditorWindow)) as GraphAssetEditorWindow;
      window.ConfigureWithGraphAsset(graphAsset);
      window.Show();
    }


    // PRAGMA MARK - Internal
		private void OnSelectionChange() {
			var oldWindow = EditorWindow.focusedWindow;
      UnityEngine.Object activeObject = Selection.activeObject;

			if (activeObject is GraphComponent) {
				GraphAssetEditorWindow.ConfigureWindow((GraphComponent)activeObject);
			} else if (activeObject is GraphAsset) {
				GraphAssetEditorWindow.ConfigureWindow((GraphAsset)activeObject);
      } else if (Selection.activeGameObject != null) {
				GraphComponent graphComponent = Selection.activeGameObject.GetComponent<GraphComponent>();
				if (graphComponent != null && graphComponent.graphAsset != null){
					GraphAssetEditorWindow.ConfigureWindow(graphComponent);
				}
			} else {
        return;
      }

			if (oldWindow) {
        oldWindow.Focus();
      }
		}
  }
}
