
using NUnit.Framework;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public class MultipleTransitionTests {
    [Test]
    public void Transition_WithMultipleTargets_EntersAllSuccessfully() {
      Graph graph = new Graph();
      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();
      Node nodeC = graph.MakeNode();

      NodeTransition nodeTransition = new NodeTransition { targets = new NodeId[] { nodeB.Id, nodeC.Id }, transition = new Transition(waitForManualExit: false) };
      graph.AddOutgoingTransitionForNode(nodeA, nodeTransition);

      bool bEntered = false;
      nodeB.OnEnter += () => { bEntered = true; };
      bool cEntered = false;
      nodeC.OnEnter += () => { cEntered = true; };

      graph.Start();
      Assert.IsTrue(bEntered);
      Assert.IsTrue(cEntered);
    }
  }
}