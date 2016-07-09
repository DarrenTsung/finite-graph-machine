using DT;
using System;
using System.Collections;

namespace DT.GameEngine {
  [Serializable]
  public abstract class TransitionCondition : ITransitionCondition {
    // PRAGMA MARK - ITransitionCondition Implementation
    public abstract bool IsConditionMet(TransitionContext context);
    public virtual void HandleTransitionUsed(TransitionContext context) {}


    // PRAGMA MARK - ITransitionCondition.IDeepClonable<ITransitionCondition> Implementation
    public abstract ITransitionCondition DeepClone();
  }
}