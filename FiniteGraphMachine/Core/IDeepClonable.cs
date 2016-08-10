using System.Collections;

namespace DTFiniteGraphMachine {
  public interface IDeepClonable<T> {
    T DeepClone();
  }
}
