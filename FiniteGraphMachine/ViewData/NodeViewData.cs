using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTFiniteGraphMachine {
  [Serializable]
  public class NodeViewData {
    // PRAGMA MARK - Public Interface
    public NodeId nodeId;
    public string name;
    public Vector2 position;

    public NodeViewData(Node node) {
      this.nodeId = node.Id;
      this.name = string.Format("Node {0}", node.Id);
      this.position = Vector2.zero;
    }

    public TransitionViewData GetViewDataForTransition(Transition transition) {
      TransitionViewData viewData = this.CachedViewDataMapping.SafeGet(transition);
      if (viewData == null) {
        viewData = this.MakeNewTransitionViewData(transition);
      }
      return viewData;
    }


    // PRAGMA MARK - Internal
    [SerializeField] private List<TransitionViewData> _transitionViewDatas = new List<TransitionViewData>();

    private Dictionary<Transition, TransitionViewData> _cachedViewDataMapping;
    private Dictionary<Transition, TransitionViewData> CachedViewDataMapping {
      get {
        if (this._cachedViewDataMapping == null) {
          this._cachedViewDataMapping = this._transitionViewDatas.ToMapWithKeys(viewData => viewData.transition);
        }
        return this._cachedViewDataMapping;
      }
    }

    private void ClearCached() {
      this._cachedViewDataMapping = null;
    }

    private TransitionViewData MakeNewTransitionViewData(Transition transition) {
      TransitionViewData viewData = new TransitionViewData(transition);
      this._transitionViewDatas.Add(viewData);
      this.ClearCached();
      return viewData;
    }
  }
}