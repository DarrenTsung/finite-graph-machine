using System;
using System.Collections;
using UnityEngine;

namespace DTFiniteGraphMachine {
  [Serializable]
  public class BoolTransitionCondition : TransitionCondition {
    // PRAGMA MARK - ITransitionCondition Implementation
    public override bool IsConditionMet(TransitionContext context) {
      if (!context.graphContext.HasBoolParameterKey(this._key)) {
        return false;
      }

      return this._targetValue == context.graphContext.GetBool(this._key);
    }

    public BoolTransitionCondition() {}
    public BoolTransitionCondition(string key, bool targetValue) {
      this._key = key;
      this._targetValue = targetValue;
    }


    // PRAGMA MARK - ITransitionCondition.IDeepClonable<ITransitionCondition> Implementation
    public override ITransitionCondition DeepClone() {
      return new BoolTransitionCondition(this._key, this._targetValue);
    }


    // PRAGMA MARK - Internal
    [SerializeField] private string _key;
    [SerializeField] private bool _targetValue;
  }
}