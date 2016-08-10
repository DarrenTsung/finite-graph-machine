#if UNITY_EDITOR
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public static class EditorGUILayoutUtil {
    public static void DynamicField(FieldInfo fieldInfo, object obj) {
      string fieldName = fieldInfo.Name;
      fieldName = Regex.Replace(fieldName, "_([a-z])", m => m.Groups[1].ToString().ToUpper());
      fieldName = Regex.Replace(fieldName, "(\\B[A-Z])", " $1");
      fieldName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(fieldName);

      if (fieldInfo.FieldType == typeof(string)) {
        fieldInfo.SetValue(obj, EditorGUILayout.TextField(fieldName, (string)fieldInfo.GetValue(obj)));
      } else if (fieldInfo.FieldType == typeof(int)) {
        fieldInfo.SetValue(obj, EditorGUILayout.IntField(fieldName, (int)fieldInfo.GetValue(obj)));
      } else if (fieldInfo.FieldType == typeof(float)) {
        fieldInfo.SetValue(obj, EditorGUILayout.FloatField(fieldName, (float)fieldInfo.GetValue(obj)));
      } else if (fieldInfo.FieldType == typeof(bool)) {
        fieldInfo.SetValue(obj, EditorGUILayout.Toggle(fieldName, (bool)fieldInfo.GetValue(obj)));
      } else if (fieldInfo.FieldType.IsClass) {
        object childFieldObj = fieldInfo.GetValue(obj);
        EditorGUILayout.LabelField(fieldInfo.Name + ": ");

        EditorGUI.indentLevel++;
          FieldInfo[] childFields = TypeUtil.GetInspectorFields(fieldInfo.FieldType);
          foreach (FieldInfo childField in childFields) {
            EditorGUILayoutUtil.DynamicField(childField, childFieldObj);
          }
        EditorGUI.indentLevel--;
      }
    }
  }
}
#endif