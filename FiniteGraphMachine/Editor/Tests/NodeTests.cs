
using NUnit.Framework;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public class NodeTests {
    [Test]
    public void Id_IsSameAsConstructed() {
      Node node = new Node(1);
      Assert.AreEqual(1, node.Id);
    }

    [Test]
    public void OnEnter_Called_WhenHandlingEnter() {
      Node node = new Node(1);

      bool onEnterCalled = false;
      node.OnEnter += () => { onEnterCalled = true; };

      node.HandleEnter();

      Assert.IsTrue(onEnterCalled);
    }

    [Test]
    public void OnExit_Called_WhenHandlingExit() {
      Node node = new Node(1);

      bool onExitCalled = false;
      node.OnExit += () => { onExitCalled = true; };

      node.HandleExit();

      Assert.IsTrue(onExitCalled);
    }

    [Test]
    public void OnManualExit_Called_WhenTriggeringManualExit() {
      Node node = new Node(1);

      bool onManualExitCalled = false;
      node.OnManualExit += (Node sameNode) => { onManualExitCalled = true; };

      node.TriggerManualExit();

      Assert.IsTrue(onManualExitCalled);
    }

    [Test]
    public void IsManuallyExited_True_AfterTriggeringManualExit() {
      Node node = new Node(1);

      Assert.IsFalse(node.IsManuallyExited);
      node.TriggerManualExit();
      Assert.IsTrue(node.IsManuallyExited);
    }

    [Test]
    public void IsManuallyExited_False_AfterExitingOrEnteringNode() {
      Node node = new Node(1);

      node.TriggerManualExit();
      Assert.IsTrue(node.IsManuallyExited);
      node.HandleEnter();
      Assert.IsFalse(node.IsManuallyExited);

      node.TriggerManualExit();
      Assert.IsTrue(node.IsManuallyExited);
      node.HandleExit();
      Assert.IsFalse(node.IsManuallyExited);
    }
  }
}