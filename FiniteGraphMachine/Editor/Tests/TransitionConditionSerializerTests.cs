using DT;
using NUnit.Framework;
using NSubstitute;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public class TransitionConditionSerializerTests {
    [TestCase("Key", 5, true)]
    [TestCase("Key", 0, false)]
    [TestCase("NotKey", 5, false)]
    [TestCase("NotKey", 0, false)]
    [TestCase("NotKey", -1, false)]
    public void IntTransitionCondition_MatchesOtherTests_AfterSerializingAndDeserializing(string key, int targetValue, bool expectedEntered) {
      Graph graph = new Graph();

      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();

      Transition transition = new Transition(waitForManualExit: false);
      IntTransitionCondition condition = new IntTransitionCondition(key, targetValue);
      // NEW CODE
      string serialized = TransitionConditionSerializer.Serialize(condition);
      ITransitionCondition deserialized = TransitionConditionSerializer.Deserialize(serialized);
      transition.AddTransitionCondition(deserialized);
      // END NEW CODE

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
      Assert.AreEqual(expectedEntered, entered);
    }

    [TestCase("Key", true, true)]
    [TestCase("Key", false, false)]
    [TestCase("NotKey", true, false)]
    [TestCase("NotKey", false, false)]
    public void BoolTransitionCondition_MatchesOtherTests_AfterSerializingAndDeserializing(string key, bool targetValue, bool expectedEntered) {
      Graph graph = new Graph();

      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();

      Transition transition = new Transition(waitForManualExit: false);

      BoolTransitionCondition condition = new BoolTransitionCondition(key, targetValue);
      // NEW CODE
      string serialized = TransitionConditionSerializer.Serialize(condition);
      ITransitionCondition deserialized = TransitionConditionSerializer.Deserialize(serialized);
      transition.AddTransitionCondition(deserialized);
      // END NEW CODE

      NodeTransition nodeTransition = new NodeTransition { targets = new NodeId[] { nodeB.Id }, transition = transition };
      graph.AddOutgoingTransitionForNode(nodeA, nodeTransition);

      IGraphContext stubContext = Substitute.For<IGraphContext>();
      stubContext.HasBoolParameterKey(Arg.Is("Key")).Returns(true);
      stubContext.GetBool(Arg.Is("Key")).Returns(true);

      IGraphContextFactory stubFactory = Substitute.For<IGraphContextFactory>();
      stubFactory.MakeContext().Returns(stubContext);
      GraphContextFactoryLocator.Provide(stubFactory);

      bool entered = false;
      nodeB.OnEnter += () => { entered = true; };

      graph.Start();
      Assert.AreEqual(expectedEntered, entered);
    }

    [TestCase("Key", true)]
    [TestCase("NotKey", false)]
    public void TriggerTransitionCondition_MatchesOtherTests_AfterSerializingAndDeserializing(string key, bool expectedEntered) {
      Graph graph = new Graph();

      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();

      Transition transition = new Transition(waitForManualExit: false);
      TriggerTransitionCondition condition = new TriggerTransitionCondition(key);
      // NEW CODE
      string serialized = TransitionConditionSerializer.Serialize(condition);
      ITransitionCondition deserialized = TransitionConditionSerializer.Deserialize(serialized);
      transition.AddTransitionCondition(deserialized);
      // END NEW CODE

      NodeTransition nodeTransition = new NodeTransition { targets = new NodeId[] { nodeB.Id }, transition = transition };
      graph.AddOutgoingTransitionForNode(nodeA, nodeTransition);

      IGraphContext stubContext = Substitute.For<IGraphContext>();
      stubContext.HasTriggerParameterKey(Arg.Is("Key")).Returns(true);
      stubContext.HasTrigger(Arg.Is("Key")).Returns(true);

      IGraphContextFactory stubFactory = Substitute.For<IGraphContextFactory>();
      stubFactory.MakeContext().Returns(stubContext);
      GraphContextFactoryLocator.Provide(stubFactory);

      bool entered = false;
      nodeB.OnEnter += () => { entered = true; };

      graph.Start();
      Assert.AreEqual(expectedEntered, entered);
    }
  }
}