using System;
using System.Collections;
using System.Collections.Generic;

namespace DTFiniteGraphMachine {
  public class CountMap<T> : Dictionary<T, int> {
    // PRAGMA MARK - Public Interface
    public void Increment(T key, int amount = 1) {
      this[key] = this.GetValue(key) + amount;
    }

    public void Decrement(T key, int amount = 1) {
      this[key] = this.GetValue(key) - amount;
    }

    public int GetValue(T key) {
      return this.SafeGet(key, defaultValue: 0);
    }
  }
}