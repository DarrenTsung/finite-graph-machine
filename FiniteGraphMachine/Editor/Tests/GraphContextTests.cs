using DT;
using NUnit.Framework;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public class GraphContextTests {
    [Test]
    public void HasParameterKey_ReturnsTrue_WhenInStartingContextParameters_ElseFalse() {
      GraphContext graphContext = new GraphContext();

      GraphContextParameter[] startingParameters = new GraphContextParameter[] {
        new GraphContextParameter { type = GraphContextParameterType.Int, key = "IntKey" },
        new GraphContextParameter { type = GraphContextParameterType.Bool, key = "BoolKey" },
        new GraphContextParameter { type = GraphContextParameterType.Trigger, key = "TriggerKey" },
      };
      graphContext.PopulateStartingContextParameters(startingParameters);

      Assert.IsTrue(graphContext.HasIntParameterKey("IntKey"));
      Assert.IsFalse(graphContext.HasIntParameterKey("NotIntKey"));
      Assert.IsTrue(graphContext.HasBoolParameterKey("BoolKey"));
      Assert.IsFalse(graphContext.HasBoolParameterKey("NotBoolKey"));
      Assert.IsTrue(graphContext.HasTriggerParameterKey("TriggerKey"));
      Assert.IsFalse(graphContext.HasTriggerParameterKey("NotTriggerKey"));
    }

    [Test]
    public void GettersAndSettersForInt_WorkAsExpected() {
      GraphContext graphContext = new GraphContext();

      GraphContextParameter[] startingParameters = new GraphContextParameter[] {
        new GraphContextParameter { type = GraphContextParameterType.Int, key = "IntKey" },
      };
      graphContext.PopulateStartingContextParameters(startingParameters);

      Assert.AreEqual(0, graphContext.GetInt("IntKey"));

      graphContext.SetInt("IntKey", 5);
      Assert.AreEqual(5, graphContext.GetInt("IntKey"));
    }

    [Test]
    public void GettersAndSettersForBool_WorkAsExpected() {
      GraphContext graphContext = new GraphContext();

      GraphContextParameter[] startingParameters = new GraphContextParameter[] {
        new GraphContextParameter { type = GraphContextParameterType.Bool, key = "BoolKey" },
      };
      graphContext.PopulateStartingContextParameters(startingParameters);

      Assert.AreEqual(false, graphContext.GetBool("BoolKey"));

      graphContext.SetBool("BoolKey", true);
      Assert.AreEqual(true, graphContext.GetBool("BoolKey"));
    }

    [Test]
    public void GettersAndSettersForTrigger_WorkAsExpected() {
      GraphContext graphContext = new GraphContext();

      GraphContextParameter[] startingParameters = new GraphContextParameter[] {
        new GraphContextParameter { type = GraphContextParameterType.Trigger, key = "TriggerKey" },
      };
      graphContext.PopulateStartingContextParameters(startingParameters);

      Assert.AreEqual(false, graphContext.HasTrigger("TriggerKey"));

      graphContext.SetTrigger("TriggerKey");
      Assert.AreEqual(true, graphContext.HasTrigger("TriggerKey"));

      graphContext.ResetTrigger("TriggerKey");
      Assert.AreEqual(false, graphContext.HasTrigger("TriggerKey"));
    }

    [Test]
    public void OnContextUpdated_Called_WhenContextValuesChange() {
      GraphContext graphContext = new GraphContext();

      GraphContextParameter[] startingParameters = new GraphContextParameter[] {
        new GraphContextParameter { type = GraphContextParameterType.Int, key = "IntKey" },
        new GraphContextParameter { type = GraphContextParameterType.Bool, key = "BoolKey" },
        new GraphContextParameter { type = GraphContextParameterType.Trigger, key = "TriggerKey" },
      };
      graphContext.PopulateStartingContextParameters(startingParameters);

      bool contextUpdated = false;
      graphContext.OnContextUpdated += () => { contextUpdated = true; };

      graphContext.SetInt("IntKey", 10);
      Assert.IsTrue(contextUpdated);
      contextUpdated = false;

      graphContext.SetBool("BoolKey", true);
      Assert.IsTrue(contextUpdated);
      contextUpdated = false;

      graphContext.SetTrigger("TriggerKey");
      Assert.IsTrue(contextUpdated);
      contextUpdated = false;
      graphContext.ResetTrigger("TriggerKey");
      Assert.IsTrue(contextUpdated);
      contextUpdated = false;
    }
  }
}