using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTFiniteGraphMachine {
  [Serializable]
  public class TransitionViewData {
    // PRAGMA MARK - Public Interface
    public Transition transition;
    public Color color;

    public TransitionViewData(Transition transition) {
      this.transition = transition;
    }
  }
}