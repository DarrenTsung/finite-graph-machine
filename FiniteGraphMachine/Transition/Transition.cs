using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DTFiniteGraphMachine {
  [Serializable]
  public class Transition : ITransition, ISerializationCallbackReceiver {
    // PRAGMA MARK - Static Public Interface
    public static Transition DeepClone(Transition t) {
      Transition clone = new Transition();
      clone._waitForManualExit = t._waitForManualExit;
      clone._conditions = t._conditions.Select(c => c.DeepClone()).ToList();

      return clone;
    }


    // PRAGMA MARK - ITransition Implementation
    public void ConfigureWithContext(TransitionContext context) {
      this._context = context;
    }

    public bool HasContext() {
      return this._context != null;
    }

    public bool AreConditionsMet() {
      if (!this.CheckConfigured()) {
        return false;
      }

      if (this._waitForManualExit && !this._context.nodeContext.IsManuallyExited) {
        return false;
      }

      foreach (ITransitionCondition condition in this._conditions) {
        if (!condition.IsConditionMet(this._context)) {
          return false;
        }
      }

      return true;
    }

    public void HandleTransitionUsed() {
      if (!this.CheckConfigured()) {
        return;
      }

      foreach (ITransitionCondition condition in this._conditions) {
        condition.HandleTransitionUsed(this._context);
      }
    }

    public void AddTransitionCondition(ITransitionCondition condition) {
      this._conditions.Add(condition);
    }

    public IList<ITransitionCondition> GetTransitionConditions() {
      return this._conditions;
    }

    public Transition(bool waitForManualExit = true) {
      this._waitForManualExit = waitForManualExit;
    }

    public bool WaitForManualExit {
      get { return this._waitForManualExit; }
      set { this._waitForManualExit = value; }
    }


    // PRAGMA MARK - ISerializationCallbackReceiver Implementation
    public void OnAfterDeserialize() {
      this._conditions = new List<ITransitionCondition>();
      foreach (string serializedCondition in this._serializedConditions) {
        ITransitionCondition condition = JsonSerialization.DeserializeGeneric<ITransitionCondition>(serializedCondition);
        if (condition != null) {
          this._conditions.Add(condition);
        }
      }
    }

    public void OnBeforeSerialize() {
      if (this._serializedConditions == null) {
        this._serializedConditions = new List<string>();
      } else {
        this._serializedConditions.Clear();
      }

      if (this._conditions == null) {
        return;
      }

      foreach (ITransitionCondition condition in this._conditions) {
        string serializedCondition = JsonSerialization.SerializeGeneric(condition);
        if (serializedCondition != null) {
          this._serializedConditions.Add(serializedCondition);
        }
      }
    }


    // PRAGMA MARK - Internal
    [SerializeField] private List<string> _serializedConditions = new List<string>();
    [SerializeField] private bool _waitForManualExit = false;

    private List<ITransitionCondition> _conditions = new List<ITransitionCondition>();

    private TransitionContext _context;

    private bool CheckConfigured() {
      if (this._context == null) {
        Debug.LogError("Transition - not configured, null context!");
        return false;
      }

      return true;
    }
  }
}