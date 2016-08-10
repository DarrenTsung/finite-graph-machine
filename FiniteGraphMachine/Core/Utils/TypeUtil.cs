using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public static class TypeUtil {
    // PRAGMA MARK - Static Public Interface
    public static FieldInfo[] GetInspectorFields(Type type) {
      if (!TypeUtil._inspectorFieldMapping.ContainsKey(type)) {
        FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                 .Where((fieldInfo) => {
                                   if (fieldInfo.IsPublic) {
                                     return true;
                                   } else {
                                     return Attribute.IsDefined(fieldInfo, typeof(SerializeField));
                                   }
                                 }).ToArray();
        TypeUtil._inspectorFieldMapping[type] = fields;
      }

      return TypeUtil._inspectorFieldMapping[type];
    }

    public static Type[] GetImplementationTypes(Type inputType) {
      if (!TypeUtil._implementationTypeMapping.ContainsKey(inputType)) {
        TypeUtil._implementationTypeMapping[inputType] =
          (from assembly in AppDomain.CurrentDomain.GetAssemblies()
           from type in assembly.GetTypes()
           where inputType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract && !type.IsGenericType
           select type).ToArray();
      }

      return TypeUtil._implementationTypeMapping[inputType];
    }

    public static string[] GetImplementationTypeNames(Type type) {
      if (!TypeUtil._implementationTypeNameMapping.ContainsKey(type)) {
        TypeUtil._implementationTypeNameMapping[type] = TypeUtil.GetImplementationTypes(type).Select(t => t.Name).ToArray();
      }

      return TypeUtil._implementationTypeNameMapping[type];
    }


    // PRAGMA MARK - Static Internal
    private static Dictionary<Type, FieldInfo[]> _inspectorFieldMapping = new Dictionary<Type, FieldInfo[]>();
    private static Dictionary<Type, Type[]> _implementationTypeMapping = new Dictionary<Type, Type[]>();
    private static Dictionary<Type, string[]> _implementationTypeNameMapping = new Dictionary<Type, string[]>();
  }
}
