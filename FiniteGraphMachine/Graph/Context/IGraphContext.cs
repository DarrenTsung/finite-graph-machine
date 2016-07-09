using DT;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DT.GameEngine {
  public interface IGraphContext {
    event Action OnContextUpdated;

    bool HasIntParameterKey(string key);
    int GetInt(string key);
    void SetInt(string key, int val);

    bool HasBoolParameterKey(string key);
    bool GetBool(string key);
    void SetBool(string key, bool val);

    bool HasTriggerParameterKey(string key);
    bool HasTrigger(string key);
    void SetTrigger(string key);
    void ResetTrigger(string key);

    void PopulateStartingContextParameters(IList<GraphContextParameter> contextParameters);
  }
}