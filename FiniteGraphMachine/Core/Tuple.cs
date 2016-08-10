using System;
using System.Collections;
using System.Collections.Generic;

namespace DTFiniteGraphMachine {
  public static class Tuple {
    public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2) {
      return new Tuple<T1, T2>(item1, item2);
    }
  }

  public class Tuple<T1, T2> {
    public T1 Item1 {
      get; private set;
    }

    public T2 Item2 {
      get; private set;
    }

    public Tuple(T1 item1, T2 item2) {
      this.Item1 = item1;
      this.Item2 = item2;
    }
  }
}