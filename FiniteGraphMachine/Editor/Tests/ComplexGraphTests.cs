
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public class ComplexGraphTests {
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
      bool cEntered = false;
      nodeC.OnEnter += () => { cEntered = true; };
      bool dEntered = false;
      nodeD.OnEnter += () => { dEntered = true; };

      graph.Start();
      Assert.IsTrue(aEntered);
      Assert.IsTrue(bEntered);
      Assert.IsFalse(cEntered);
      Assert.IsFalse(dEntered);

      aEntered = false;
      bEntered = false;
      cEntered = false;
      dEntered = false;

      nodeB.TriggerManualExit();
      Assert.IsFalse(aEntered);
      Assert.IsFalse(bEntered);
      Assert.IsFalse(cEntered);
      Assert.IsTrue(dEntered);
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