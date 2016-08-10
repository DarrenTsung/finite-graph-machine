using System;
using System.Reflection;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public static class JsonSerialization {
    public static T DeserializeFromTextAsset<T>(TextAsset source) {
      return JsonSerialization.DeserializeFromString<T>(source.text);
    }

    public static T DeserializeFromString<T>(string source) {
      return JsonUtility.FromJson<T>(source);
    }

    public static void OverwriteDeserializeFromTextAsset(TextAsset source, object objectToOverwrite) {
      JsonSerialization.OverwriteDeserializeFromString(source.text, objectToOverwrite);
    }

    public static void OverwriteDeserializeFromString(string source, object objectToOverwrite) {
      JsonUtility.FromJsonOverwrite(source, objectToOverwrite);
    }

    public static void SerializeToTextAsset(object obj, TextAsset source, bool prettyPrint = false) {
      TextAssetUtil.WriteToTextAsset(JsonUtility.ToJson(obj, prettyPrint), source);
    }

    public static void SerializeToTextAssetFilename(object obj, string filename, bool prettyPrint = false) {
      TextAssetUtil.WriteToTextAssetFilename(JsonUtility.ToJson(obj, prettyPrint), filename);
    }

    public static string SerializeGeneric(object obj) {
      if (obj == null) {
        Debug.LogError("JsonSerialization - SerializeGeneric was passed in null!");
        return "";
      }

      Type type = obj.GetType();

      MethodInfo genericMethod = typeof(JsonSerialization).GetMethod("SerializeType", BindingFlags.Static | BindingFlags.NonPublic);
      MethodInfo method = genericMethod.MakeGenericMethod(type);

      string serializedCondition = (string)method.Invoke(null, new object[] { obj });

      var serializedWrapper = new SerializedClassWrapper(type, serializedCondition);
      return JsonUtility.ToJson(serializedWrapper, prettyPrint: true);
    }

    private static string SerializeType<T>(T obj) {
      return JsonUtility.ToJson(obj, prettyPrint: true);
    }

    public static T DeserializeGeneric<T>(string serializedClassWrapper) {
      SerializedClassWrapper serializedWrapper = JsonUtility.FromJson<SerializedClassWrapper>(serializedClassWrapper);
      if (serializedWrapper == null) {
        Debug.LogError("JsonSerialization - DeserializeGeneric failed to deserialize class wrapper!");
        return default(T);
      }

      Type type = Type.GetType(serializedWrapper.typeName);

      MethodInfo genericMethod = typeof(JsonSerialization).GetMethod("DeserializeType", BindingFlags.Static | BindingFlags.NonPublic);
      MethodInfo method = genericMethod.MakeGenericMethod(type);

      object obj = method.Invoke(null, new object[] { serializedWrapper.serializedClass });
      T castedObject = default(T);
      try {
        castedObject = (T)obj;
      } catch (InvalidCastException) {
        Debug.LogError("JsonSerialization - DeserializeGeneric failed to cast deserialized class!");
      }

      return castedObject;
    }

    private static T DeserializeType<T>(string serializedT) {
      return JsonUtility.FromJson<T>(serializedT);
    }

    [Serializable]
    private class SerializedClassWrapper {
      public string typeName;
      public string serializedClass;

      public SerializedClassWrapper(Type type, string serializedClass) {
        this.typeName = type.AssemblyQualifiedName;
        this.serializedClass = serializedClass;
      }
    }
  }
}