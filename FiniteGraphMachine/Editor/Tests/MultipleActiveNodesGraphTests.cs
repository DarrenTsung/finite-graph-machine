using DT;
using NUnit.Framework;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public class MultipleActiveNodesGraphTests {
    [Test]
    public void MultipleStartingNodes_EnteredCorrectly() {
      Graph graph = new Graph();
      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();
      Node nodeC = graph.MakeNode();

      graph.SetStartingNodes(new Node[] { nodeA, nodeB, nodeC });

      bool aEntered = false;
      nodeA.OnEnter += () => { aEntered = true; };
      bool bEntered = false;
      nodeB.OnEnter += () => { bEntered = true; };
      bool cEntered = false;
      nodeC.OnEnter += () => { cEntered = true; };

      graph.Start();
      Assert.IsTrue(aEntered);
      Assert.IsTrue(bEntered);
      Assert.IsTrue(cEntered);
    }

    [Test]
    public void Entering_AlreadyActiveNode_WillNotEnter() {
      Graph graph = new Graph();

      // A --
      // B --\-- C
      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();
      Node nodeC = graph.MakeNode();

      NodeTransition acNodeTransition = new NodeTransition { targets = new NodeId[] { nodeC.Id }, transition = new Transition(waitForManualExit: false) };
      graph.AddOutgoingTransitionForNode(nodeA, acNodeTransition);

      NodeTransition bcNodeTransition = new NodeTransition { targets = new NodeId[] { nodeC.Id }, transition = new Transition(waitForManualExit: false) };
      graph.AddOutgoingTransitionForNode(nodeB, bcNodeTransition);

      graph.SetStartingNodes(new Node[] { nodeA, nodeB, nodeC });

      int cEnteredCount = 0;
      nodeC.OnEnter += () => { cEnteredCount++; };

      graph.Start();
      Assert.AreEqual(1, cEnteredCount);
    }

    [Test]
    public void MultipleActiveNodes_BothTransition() {
      Graph graph = new Graph();

      // A --> B
      // C --> D
      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();
      Node nodeC = graph.MakeNode();
      Node nodeD = graph.MakeNode();

      NodeTransition abNodeTransition = new NodeTransition { targets = new NodeId[] { nodeB.Id }, transition = new Transition(waitForManualExit: false) };
      graph.AddOutgoingTransitionForNode(nodeA, abNodeTransition);

      NodeTransition cdNodeTransition = new NodeTransition { targets = new NodeId[] { nodeD.Id }, transition = new Transition(waitForManualExit: false) };
      graph.AddOutgoingTransitionForNode(nodeC, cdNodeTransition);

      graph.SetStartingNodes(new Node[] { nodeA, nodeC });

      bool aEntered = false;
      nodeA.OnEnter += () => { aEntered = true; };
      bool bEntered = false;
      nodeB.OnEnter += () => { bEntered = true; };
      bool cEntered = false;
      nodeC.OnEnter += () => { cEntered = true; };
      bool dEntered = false;
      nodeD.OnEnter += () => { dEntered = true; };

      graph.Start();
      Assert.IsTrue(aEntered);
      Assert.IsTrue(bEntered);
      Assert.IsTrue(cEntered);
      Assert.IsTrue(dEntered);
    }
  }
}