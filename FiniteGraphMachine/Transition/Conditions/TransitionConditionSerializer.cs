using DT;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DT.GameEngine {
  public static class TransitionConditionSerializer {
    public static string Serialize(ITransitionCondition condition) {
      if (condition == null) {
        Debug.LogError("TransitionConditionSerializer - Serialize was passed in null condition!");
        return "";
      }

      Type type = condition.GetType();

      MethodInfo genericMethod = typeof(TransitionConditionSerializer).GetMethod("SerializeCondition", BindingFlags.Static | BindingFlags.NonPublic);
      MethodInfo method = genericMethod.MakeGenericMethod(type);

      string serializedCondition = (string)method.Invoke(null, new object[] { condition });

      var serializedConditionWrapper = new SerializedClassWrapper(type, serializedCondition);
      return JsonUtility.ToJson(serializedConditionWrapper, prettyPrint: true);
    }

    private static string SerializeCondition<T>(T condition) {
      return JsonUtility.ToJson(condition, prettyPrint: true);
    }

    public static ITransitionCondition Deserialize(string serializedClassWrapper) {
      SerializedClassWrapper serializedConditionWrapper = JsonUtility.FromJson<SerializedClassWrapper>(serializedClassWrapper);
      if (serializedConditionWrapper == null) {
        Debug.LogError("TransitionConditionSerializer - Deserialize failed to deserialize class wrapper!");
        return null;
      }

      Type type = Type.GetType(serializedConditionWrapper.typeName);

      MethodInfo genericMethod = typeof(TransitionConditionSerializer).GetMethod("DeserializeCondition", BindingFlags.Static | BindingFlags.NonPublic);
      MethodInfo method = genericMethod.MakeGenericMethod(type);

      return method.Invoke(null, new object[] { serializedConditionWrapper.serializedClass }) as ITransitionCondition;
    }

    private static ITransitionCondition DeserializeCondition<T>(string serializedCondition) {
      T condition = JsonUtility.FromJson<T>(serializedCondition);
      return condition as ITransitionCondition;
    }
  }

  [Serializable]
  public class SerializedClassWrapper {
    public string typeName;
    public string serializedClass;

    public SerializedClassWrapper(Type type, string serializedClass) {
      this.typeName = type.AssemblyQualifiedName;
      this.serializedClass = serializedClass;
    }
  }
}
