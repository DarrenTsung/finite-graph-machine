
using NUnit.Framework;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public class BasicGraphTests {
    [Test]
    public void AfterStartingGraph_StartingNodeEntered() {
      Graph graph = new Graph();
      Node nodeA = graph.MakeNode();
      graph.SetStartingNodes(new Node[] { nodeA });

      bool entered = false;
      nodeA.OnEnter += () => { entered = true; };

      graph.Start();
      Assert.IsTrue(entered);
    }

    [Test]
    public void StartingNodes_DefaultToFirstNode_Automatically() {
      Graph graph = new Graph();
      Node nodeA = graph.MakeNode();

      bool entered = false;
      nodeA.OnEnter += () => { entered = true; };

      graph.Start();
      Assert.IsTrue(entered);
    }

    [Test]
    public void NoStartingNodes_DoesNotBreakThings() {
      Graph graph = new Graph();
      graph.Start();
      Assert.IsTrue(true);
    }
  }
}