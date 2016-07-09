using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
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