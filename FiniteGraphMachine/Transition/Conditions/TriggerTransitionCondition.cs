using System;
using System.Collections;
using UnityEngine;

namespace DTFiniteGraphMachine {
  [Serializable]
  public class TriggerTransitionCondition : TransitionCondition {
    // PRAGMA MARK - ITransitionCondition Implementation
    public override bool IsConditionMet(TransitionContext context) {
      if (!context.graphContext.HasTriggerParameterKey(this._key)) {
        return false;
      }

      return context.graphContext.HasTrigger(this._key);
    }

    public override void HandleTransitionUsed(TransitionContext context) {
      context.graphContext.ResetTrigger(this._key);
    }

    public TriggerTransitionCondition() {}
    public TriggerTransitionCondition(string key) {
      this._key = key;
    }


    // PRAGMA MARK - ITransitionCondition.IDeepClonable<ITransitionCondition> Implementation
    public override ITransitionCondition DeepClone() {
      return new TriggerTransitionCondition(this._key);
    }


    // PRAGMA MARK - Internal
    [SerializeField] private string _key;
  }
}