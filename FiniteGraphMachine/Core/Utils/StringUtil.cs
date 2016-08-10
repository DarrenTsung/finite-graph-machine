using System.Collections;
using System.Collections.Generic;

namespace DTFiniteGraphMachine {
  public static class StringUtil {
    public static string Join<T>(string seperator, IEnumerable<T> values) {
      string s = null;
      foreach (T val in values) {
        if (s == null) {
          s = val.ToString();
        } else {
          s += ", " + val.ToString();
        }
      }

      return s;
    }
  }
}