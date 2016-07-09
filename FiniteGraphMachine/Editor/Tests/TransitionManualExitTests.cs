using DT;
using NUnit.Framework;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public class TransitionManualExitTests {
    [Test]
    public void NoTransitionCondition_WithWaitForManualExit_TakenAfterManualExitTriggered() {
      Graph graph = new Graph();
      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();

      Transition transition = new Transition(waitForManualExit: true);
      NodeTransition nodeTransition = new NodeTransition { targets = new NodeId[] { nodeB.Id }, transition = transition };
      graph.AddOutgoingTransitionForNode(nodeA, nodeTransition);

      bool entered = false;
      nodeB.OnEnter += () => { entered = true; };

      graph.Start();
      Assert.IsFalse(entered);

      nodeA.TriggerManualExit();
      Assert.IsTrue(entered);
    }

    [TestCase("Key", 5, true)]
    [TestCase("Key", 3, false)]
    [TestCase("NotKey", 5, false)]
    [TestCase("NotKey", 3, false)]
    public void IntTransitionCondition_WithWaitForManualExit_TakenAfterManualExitTriggered(string key, int targetValue, bool expectedEntered) {
      Graph graph = new Graph();
      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();

      Transition transition = new Transition(waitForManualExit: true);
      transition.AddTransitionCondition(new IntTransitionCondition(key, targetValue));
      NodeTransition nodeTransition = new NodeTransition { targets = new NodeId[] { nodeB.Id }, transition = transition };
      graph.AddOutgoingTransitionForNode(nodeA, nodeTransition);

      IGraphContext stubContext = Substitute.For<IGraphContext>();
      stubContext.HasIntParameterKey(Arg.Is("Key")).Returns(true);
      stubContext.GetInt(Arg.Is("Key")).Returns(5);

      IGraphContextFactory stubFactory = Substitute.For<IGraphContextFactory>();
      stubFactory.MakeContext().Returns(stubContext);
      GraphContextFactoryLocator.Provide(stubFactory);

      bool entered = false;
      nodeB.OnEnter += () => { entered = true; };

      graph.Start();
      Assert.IsFalse(entered);

      nodeA.TriggerManualExit();
      Assert.AreEqual(expectedEntered, entered);
    }
  }
}