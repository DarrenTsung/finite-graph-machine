
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    private interface IEditorWindowState {
      // PRAGMA MARK - GraphAssetEditor
      GraphAssetEditorWindow Context {
        get; set;
      }

      // PRAGMA MARK - State
      void Cleanup();

      // PRAGMA MARK - Editor Window
      void Render();
      void HandleEvent(Event currentEvent);
    }
  }
}