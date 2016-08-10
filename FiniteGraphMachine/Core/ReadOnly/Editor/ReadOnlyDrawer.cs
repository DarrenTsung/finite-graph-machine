using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
	[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
	public class ReadOnlyDrawer : PropertyDrawer {
		public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label) {
      GUI.enabled = false;
      EditorGUI.PropertyField(_position, _property, _label, true);
      GUI.enabled = true;
		}
	}
}