
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Internal
    private IEditorWindowState _currentState;

    private void CleanupCurrentState() {
      if (this._currentState != null) {
        this._currentState.Cleanup();
        this._currentState = null;
      }
    }

    private void SetCurrentState(IEditorWindowState newState) {
      this._currentState = newState;
      this._currentState.Context = this;
    }
  }
}