using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTFiniteGraphMachine {
  [Serializable]
  [CreateAssetMenu]
  public class GraphAsset : ScriptableObject {
    public Graph graph = new Graph();
    public GraphViewData graphViewData = new GraphViewData();
  }
}