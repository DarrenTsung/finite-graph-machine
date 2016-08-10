using System;
using System.Collections;

namespace DTFiniteGraphMachine {
  public interface ITransition {
    void ConfigureWithContext(TransitionContext context);
    bool HasContext();

    bool AreConditionsMet();
    void HandleTransitionUsed();

    void AddTransitionCondition(ITransitionCondition condition);
  }
}