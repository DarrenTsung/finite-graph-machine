
using NUnit.Framework;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public class TransitionPriorityTests {
    [Test]
    public void WhenMultipleMatchingTransitions_TheFirstIsTaken() {
      Graph graph = new Graph();

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
      graph.AddOutgoingTransitionForNode(nodeA, cNodeTransition);

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
      Assert.IsFalse(cEntered);
    }
  }
}