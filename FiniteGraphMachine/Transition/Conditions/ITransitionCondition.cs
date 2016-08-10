using System;
using System.Collections;
using System.Linq;

namespace DTFiniteGraphMachine {
  public interface ITransitionCondition : IDeepClonable<ITransitionCondition> {
    bool IsConditionMet(TransitionContext context);
    void HandleTransitionUsed(TransitionContext context);
  }
}
