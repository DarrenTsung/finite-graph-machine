
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public class ComplexGraphAfterCloningTests {
    [Test]
    public void MultipleTransitions_HappenCorrectly() {
      Graph graph = new Graph();

      // A -> B -> C
      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();
      Node nodeC = graph.MakeNode();

      Transition bTransition = new Transition(waitForManualExit: false);
      bTransition.AddTransitionCondition(new IntTransitionCondition("Key", 5));
      NodeTransition bNodeTransition = new NodeTransition { targets = new NodeId[] { nodeB.Id }, transition = bTransition };
      graph.AddOutgoingTransitionForNode(nodeA, bNodeTransition);

      Transition cTransition = new Transition(waitForManualExit: false);
      cTransition.AddTransitionCondition(new IntTransitionCondition("Key", 5));
      NodeTransition cNodeTransition = new NodeTransition { targets = new NodeId[] { nodeC.Id }, transition = cTransition };
      graph.AddOutgoingTransitionForNode(nodeB, cNodeTransition);

      // CLONING CODE
      graph = Graph.DeepClone(graph);

      nodeA = graph.LoadNodeById(nodeA.Id);
      nodeB = graph.LoadNodeById(nodeB.Id);
      nodeC = graph.LoadNodeById(nodeC.Id);
      // END CLONING CODE

      IGraphContext stubContext = Substitute.For<IGraphContext>();
      stubContext.HasIntParameterKey(Arg.Is("Key")).Returns(true);
      stubContext.GetInt(Arg.Is("Key")).Returns(5);

      IGraphContextFactory stubFactory = Substitute.For<IGraphContextFactory>();
      stubFactory.MakeContext().Returns(stubContext);
      GraphContextFactoryLocator.Provide(stubFactory);

      bool bEntered = false;
      nodeB.OnEnter += () => { bEntered = true; };
      bool cEntered = false;
      nodeC.OnEnter += () => { cEntered = true; };

      graph.Start();
      Assert.IsTrue(bEntered);
      Assert.IsTrue(cEntered);
    }

    [Test]
    public void MoreComplexGraph_RunsCorrectly() {
      Graph graph = new Graph();

      // A --- B --
      //  \--- C --\- D
      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();
      Node nodeC = graph.MakeNode();
      Node nodeD = graph.MakeNode();

      Transition bTransition = new Transition(waitForManualExit: false);
      bTransition.AddTransitionCondition(new IntTransitionCondition("Key", 5));
      NodeTransition bNodeTransition = new NodeTransition { targets = new NodeId[] { nodeB.Id }, transition = bTransition };
      graph.AddOutgoingTransitionForNode(nodeA, bNodeTransition);

      Transition cTransition = new Transition(waitForManualExit: false);
      cTransition.AddTransitionCondition(new IntTransitionCondition("Key", 3));
      NodeTransition cNodeTransition = new NodeTransition { targets = new NodeId[] { nodeC.Id }, transition = cTransition };
      graph.AddOutgoingTransitionForNode(nodeA, cNodeTransition);

      Transition cdTransition = new Transition(waitForManualExit: false);
      cdTransition.AddTransitionCondition(new IntTransitionCondition("Key", 5));
      NodeTransition cdNodeTransition = new NodeTransition { targets = new NodeId[] { nodeD.Id }, transition = cdTransition };
      graph.AddOutgoingTransitionForNode(nodeC, cdNodeTransition);

      Transition bdTransition = new Transition(waitForManualExit: true);
      bdTransition.AddTransitionCondition(new IntTransitionCondition("Key", 5));
      NodeTransition bdNodeTransition = new NodeTransition { targets = new NodeId[] { nodeD.Id }, transition = bdTransition };
      graph.AddOutgoingTransitionForNode(nodeB, bdNodeTransition);

      // CLONING CODE
      Graph clonedGraph = Graph.DeepClone(graph);

      Node clonedNodeA = clonedGraph.LoadNodeById(nodeA.Id);
      Node clonedNodeB = clonedGraph.LoadNodeById(nodeB.Id);
      Node clonedNodeC = clonedGraph.LoadNodeById(nodeC.Id);
      Node clonedNodeD = clonedGraph.LoadNodeById(nodeD.Id);
      // END CLONING CODE

      IGraphContext stubContext = Substitute.For<IGraphContext>();
      stubContext.HasIntParameterKey(Arg.Is("Key")).Returns(true);
      stubContext.GetInt(Arg.Is("Key")).Returns(5);

      IGraphContextFactory stubFactory = Substitute.For<IGraphContextFactory>();
      stubFactory.MakeContext().Returns(stubContext);
      GraphContextFactoryLocator.Provide(stubFactory);

      bool aEntered = false;
      nodeA.OnEnter += () => { aEntered = true; };
      bool bEntered = false;
      nodeB.OnEnter += () => { bEntered = true; };

      bool clonedAEntered = false;
      clonedNodeA.OnEnter += () => { clonedAEntered = true; };
      bool clonedBEntered = false;
      clonedNodeB.OnEnter += () => { clonedBEntered = true; };
      bool clonedCEntered = false;
      clonedNodeC.OnEnter += () => { clonedCEntered = true; };
      bool clonedDEntered = false;
      clonedNodeD.OnEnter += () => { clonedDEntered = true; };

      clonedGraph.Start();
      Assert.IsTrue(clonedAEntered);
      Assert.IsTrue(clonedBEntered);
      Assert.IsFalse(clonedCEntered);
      Assert.IsFalse(clonedDEntered);

      // check to make sure the original nodes are not entered
      Assert.IsFalse(aEntered);
      Assert.IsFalse(bEntered);

      clonedAEntered = false;
      clonedBEntered = false;
      clonedCEntered = false;
      clonedDEntered = false;

      clonedNodeB.TriggerManualExit();
      Assert.IsFalse(clonedAEntered);
      Assert.IsFalse(clonedBEntered);
      Assert.IsFalse(clonedCEntered);
      Assert.IsTrue(clonedDEntered);
    }

    [Test]
    public void Entering_TheSameNode_MultipleTimesWithCascade_Works() {
      Graph graph = new Graph();

      // A -> B <-> C
      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();
      Node nodeC = graph.MakeNode();

      Transition abTransition = new Transition(waitForManualExit: false);
      NodeTransition abNodeTransition = new NodeTransition { targets = new NodeId[] { nodeB.Id }, transition = abTransition };
      graph.AddOutgoingTransitionForNode(nodeA, abNodeTransition);

      Transition bcTransition = new Transition(waitForManualExit: false);
      bcTransition.AddTransitionCondition(new TriggerTransitionCondition("BGoToC"));
      NodeTransition bcNodeTransition = new NodeTransition { targets = new NodeId[] { nodeC.Id }, transition = bcTransition };
      graph.AddOutgoingTransitionForNode(nodeB, bcNodeTransition);

      Transition cbTransition = new Transition(waitForManualExit: false);
      cbTransition.AddTransitionCondition(new TriggerTransitionCondition("CGoToB"));
      NodeTransition cbNodeTransition = new NodeTransition { targets = new NodeId[] { nodeB.Id }, transition = cbTransition };
      graph.AddOutgoingTransitionForNode(nodeC, cbNodeTransition);

      // CLONING CODE
      graph = Graph.DeepClone(graph);

      nodeA = graph.LoadNodeById(nodeA.Id);
      nodeB = graph.LoadNodeById(nodeB.Id);
      nodeC = graph.LoadNodeById(nodeC.Id);
      // END CLONING CODE

      IGraphContext stubContext = Substitute.For<IGraphContext>();
      stubContext.HasTriggerParameterKey(Arg.Is("BGoToC")).Returns(true);
      stubContext.HasTriggerParameterKey(Arg.Is("CGoToB")).Returns(true);
      // returns true then false
      stubContext.HasTrigger(Arg.Is("BGoToC")).Returns(true, false);
      // returns true then false
      stubContext.HasTrigger(Arg.Is("CGoToB")).Returns(true, false);

      IGraphContextFactory stubFactory = Substitute.For<IGraphContextFactory>();
      stubFactory.MakeContext().Returns(stubContext);
      GraphContextFactoryLocator.Provide(stubFactory);

      int bEnteredCount = 0;
      nodeB.OnEnter += () => { bEnteredCount++; };
      int cEnteredCount = 0;
      nodeC.OnEnter += () => { cEnteredCount++; };

      graph.Start();

      Assert.AreEqual(2, bEnteredCount);
      Assert.AreEqual(1, cEnteredCount);
    }
  }
}