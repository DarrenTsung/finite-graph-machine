using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Static
    [MenuItem("DarrenTsung/Graph Editor")]
    public static void ShowWindow() {
      GraphAssetEditorWindow window = EditorWindow.GetWindow(typeof(GraphAssetEditorWindow)) as GraphAssetEditorWindow;
      window.Show();
    }


    // PRAGMA MARK - Public Interface
    public void ConfigureWithGraphAsset(GraphAsset graphAsset) {
      this._targetGraphAsset = graphAsset;
    }


    // PRAGMA MARK - Internal
    private GraphAsset _targetGraphAsset;
    private Graph TargetGraph {
      get {
        if (this._targetGraphAsset == null) {
          return null;
        }
        return this._targetGraphAsset.graph;
      }
    }

    private GraphViewData TargetGraphViewData {
      get {
        if (this._targetGraphAsset == null) {
          return null;
        }

        return this._targetGraphAsset.graphViewData;
      }
    }

    private GUISkin _skin;

    private void OnEnable() {
      this.DeselectCurrentNode();
      this.DeselectCurrentNodeTransition();

			this._skin = (GUISkin)Resources.Load("GraphAssetEditorWindowSkin");
      this.wantsMouseMove = true;
      this.GoToNormalState();
    }

    private NodeViewData GetViewDataForNode(Node node) {
      return this.TargetGraphViewData.LoadViewDataForNode(node);
    }

    private void SetTargetDirty() {
      if (this._targetGraphAsset == null) {
        return;
      }

			EditorUtility.SetDirty(this._targetGraphAsset);
    }

    private void SaveChanges() {
      AssetDatabase.SaveAssets();
    }
  }
}