using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  [CreateAssetMenu]
  public class GraphAsset : ScriptableObject {
    public Graph graph = new Graph();
    public GraphViewData graphViewData = new GraphViewData();
  }
}