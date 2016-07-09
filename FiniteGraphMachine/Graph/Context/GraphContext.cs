using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  public class GraphContext : IGraphContext {
    // PRAGMA MARK - Public Interface
    public event Action OnContextUpdated = delegate {};

    public bool HasIntParameterKey(string key) {
      return this._intValues.ContainsKey(key);
    }

    public int GetInt(string key) {
      if (!this.HasIntParameterKey(key)) {
        Debug.LogError("GraphContext - GetInt called with invalid parameter key: " + key);
        return 0;
      }

      return this._intValues[key];
    }

    public void SetInt(string key, int val) {
      if (!this.HasIntParameterKey(key)) {
        Debug.LogError("GraphContext - SetInt called with invalid parameter key: " + key);
        return;
      }

      this._intValues[key] = val;
      this.OnContextUpdated.Invoke();
    }

    public bool HasBoolParameterKey(string key) {
      return this._boolValues.ContainsKey(key);
    }

    public bool GetBool(string key) {
      if (!this.HasBoolParameterKey(key)) {
        Debug.LogError("GraphContext - GetBool called with invalid parameter key: " + key);
        return false;
      }

      return this._boolValues[key];
    }

    public void SetBool(string key, bool val) {
      if (!this.HasBoolParameterKey(key)) {
        Debug.LogError("GraphContext - SetBool called with invalid parameter key: " + key);
        return;
      }

      this._boolValues[key] = val;
      this.OnContextUpdated.Invoke();
    }

    public bool HasTriggerParameterKey(string key) {
      return this._triggerValues.ContainsKey(key);
    }

    public bool HasTrigger(string key) {
      if (!this.HasTriggerParameterKey(key)) {
        Debug.LogError("GraphContext - HasTrigger called with invalid parameter key: " + key);
        return false;
      }

      return this._triggerValues[key];
    }

    public void SetTrigger(string key) {
      if (!this.HasTriggerParameterKey(key)) {
        Debug.LogError("GraphContext - SetTrigger called with invalid parameter key: " + key);
        return;
      }

      this._triggerValues[key] = true;
      this.OnContextUpdated.Invoke();
    }

    public void ResetTrigger(string key) {
      if (!this.HasTriggerParameterKey(key)) {
        Debug.LogError("GraphContext - ResetTrigger called with invalid parameter key: " + key);
        return;
      }

      this._triggerValues[key] = false;
      this.OnContextUpdated.Invoke();
    }

    public void PopulateStartingContextParameters(IList<GraphContextParameter> contextParameters) {
      foreach (GraphContextParameter contextParameter in contextParameters) {
        switch (contextParameter.type) {
          case GraphContextParameterType.Int:
            this._intValues[contextParameter.key] = 0;
            break;
          case GraphContextParameterType.Bool:
            this._boolValues[contextParameter.key] = false;
            break;
          case GraphContextParameterType.Trigger:
            this._triggerValues[contextParameter.key] = false;
            break;
        }
      }
    }


    // PRAGMA MARK - Internal
    private Dictionary<string, int> _intValues = new Dictionary<string, int>();
    private Dictionary<string, bool> _boolValues = new Dictionary<string, bool>();
    private Dictionary<string, bool> _triggerValues = new Dictionary<string, bool>();
  }
}