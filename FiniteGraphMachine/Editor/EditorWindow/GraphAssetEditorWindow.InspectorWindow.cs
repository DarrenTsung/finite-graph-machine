using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Static
    private const float kInspectorWindowWidth = 270.0f;
    private static readonly Vector2 kInspectorWindowPosition = new Vector2(10.0f, 20.0f);

    private const float kInspectorHideButtonHeight = 25.0f;
    private const float kInspectorViewHeight = 250.0f;

    // PRAGMA MARK - Internal
    private bool _inspectorCollapsed = false;
    private Vector2 _inspectorScrollPos;

    private void DrawInspectorWindow() {
      Node selectedNode = this.GetSelectedNode();
      if (selectedNode == null) {
        return;
      }

      NodeViewData selectedNodeViewData = this.GetViewDataForNode(selectedNode);

      float heightSoFar = 0.0f;

      Rect inspectorHideButtonRect = new Rect(kInspectorWindowPosition, new Vector2(kInspectorWindowWidth, kInspectorHideButtonHeight));
      EditorGUIUtility.AddCursorRect(inspectorHideButtonRect, MouseCursor.Link);
      if (GUI.Button(inspectorHideButtonRect, "")) {
        this._inspectorCollapsed = !this._inspectorCollapsed;
      }
      heightSoFar += kInspectorHideButtonHeight;

      if (!this._inspectorCollapsed) {
        Vector2 inspectorRectPosition = kInspectorWindowPosition + new Vector2(0.0f, heightSoFar);

        Rect inspectorRect = new Rect(inspectorRectPosition, new Vector2(kInspectorWindowWidth, kInspectorViewHeight));
				GUILayout.BeginArea(inspectorRect, "", (GUIStyle)"InspectorWindow");
        // Scroll View
  				this._inspectorScrollPos = EditorGUILayout.BeginScrollView(this._inspectorScrollPos, GUILayout.Height(kInspectorViewHeight));
            this.DrawNodeInspector(selectedNode, selectedNodeViewData);
  				EditorGUILayout.EndScrollView();
        // End Scroll View
				GUILayout.EndArea();
      }
    }

    private void DrawNodeInspector(Node node, NodeViewData nodeViewData) {
			nodeViewData.name = EditorGUILayout.TextField(nodeViewData.name);

      // Node Delegate Button + Fields
      EditorGUILayout.BeginVertical((GUIStyle)"InspectorBigBox");
        EditorGUILayout.LabelField("Node Delegates:");
        foreach (INodeDelegate nodeDelegate in node.GetNodeDelegates()) {
          Type nodeDelegateType = nodeDelegate.GetType();
          EditorGUILayout.BeginVertical((GUIStyle)"InspectorBox");
            EditorGUILayout.LabelField(nodeDelegateType.Name, EditorStyles.boldLabel);

            FieldInfo[] fields = TypeUtil.GetInspectorFields(nodeDelegateType);
            foreach (FieldInfo field in fields) {
              EditorGUILayoutUtil.DynamicField(field, nodeDelegate);
            }
          EditorGUILayout.EndVertical();
          EditorGUILayout.Space();
        }

        if (GUILayout.Button("", (GUIStyle)"AddButton", GUILayout.Width(20.0f), GUILayout.Height(20.0f))) {
          GenericMenu nodeDelegateMenu = new GenericMenu();
          foreach (Type nodeDelegateType in INodeDelegateUtil.ImplementationTypes) {
            nodeDelegateMenu.AddItem(new GUIContent(nodeDelegateType.Name), false, this.AddNodeDelegateToNode, Tuple.Create(node, nodeDelegateType));
          }
          nodeDelegateMenu.ShowAsContext();
        }
      EditorGUILayout.EndVertical();

      EditorGUILayout.Space();

      // Node Transitions
      EditorGUILayout.BeginVertical((GUIStyle)"InspectorBigBox");
        EditorGUILayout.LabelField("Transitions:");
        IList<NodeTransition> nodeTransitions = this.TargetGraph.GetOutgoingTransitionsForNode(node);
        foreach (NodeTransition nodeTransition in nodeTransitions) {
          GUIStyle transitionStyle = this.IsNodeTransitionSelected(nodeTransition) ? (GUIStyle)"SelectedInspectorBox" : (GUIStyle)"InspectorBox";
          Rect transitionRect = EditorGUILayout.BeginVertical(transitionStyle, GUILayout.MinHeight(30.0f));
            string targetText = "";
            targetText += (nodeTransition.targets.Length > 1) ? "Targets: " : "Target: ";
            targetText += StringUtil.Join(", ", nodeTransition.targets.Select(id => this.GetViewDataForNode(this.TargetGraph.LoadNodeById(id)).name));

            EditorGUILayout.LabelField(targetText, GUILayout.Height(30.0f));

            Rect editButtonRect = new Rect(new Vector2(transitionRect.x + transitionRect.width - 25.0f,
                                                       transitionRect.y + 5.0f),
                                           new Vector2(20.0f, 20.0f));
            if (GUI.Button(editButtonRect, "", (GUIStyle)"EditButton")) {
              this.StartEditingNodeTransition(node, nodeTransition);
            }

            Rect removeButtonRect = new Rect(new Vector2(transitionRect.x + transitionRect.width - 50.0f,
                                                       transitionRect.y + 5.0f),
                                             new Vector2(20.0f, 20.0f));
            if (GUI.Button(removeButtonRect, "", (GUIStyle)"RemoveButton")) {
              this.AddPendingNodeTransitionRemoval(node, nodeTransition);
            }

            EditorGUILayout.LabelField("Conditions: ");
            Transition transition = nodeTransition.transition;
            transition.WaitForManualExit = EditorGUILayout.Toggle("WaitForManualExit: ", transition.WaitForManualExit);
            foreach (ITransitionCondition transitionCondition in transition.GetTransitionConditions()) {
              EditorGUILayout.BeginVertical(transitionStyle);
                Type transitionConditionType = transitionCondition.GetType();
                EditorGUILayout.LabelField(transitionConditionType.Name, EditorStyles.boldLabel);

                FieldInfo[] fields = TypeUtil.GetInspectorFields(transitionConditionType);
                foreach (FieldInfo field in fields) {
                  EditorGUILayoutUtil.DynamicField(field, transitionCondition);
                }
              EditorGUILayout.EndVertical();
              EditorGUILayout.Space();
            }

            if (GUILayout.Button("", (GUIStyle)"AddButton", GUILayout.Width(20.0f), GUILayout.Height(20.0f))) {
              GenericMenu nodeDelegateMenu = new GenericMenu();
              foreach (Type transitionConditionType in TypeUtil.GetImplementationTypes(typeof(ITransitionCondition))) {
                nodeDelegateMenu.AddItem(new GUIContent(transitionConditionType.Name), false, this.AddTransitionCondition, Tuple.Create(nodeTransition, transitionConditionType));
              }
              nodeDelegateMenu.ShowAsContext();
            }
          EditorGUILayout.EndVertical();
          EditorGUILayout.Space();
        }

        if (GUILayout.Button("", (GUIStyle)"AddButton", GUILayout.Width(20.0f), GUILayout.Height(20.0f))) {
          this.MakeTransition(node);
        }
      EditorGUILayout.EndVertical();
      EditorGUILayout.Space();
    }

    private void AddNodeDelegateToNode(object tupleAsObject) {
      Tuple<Node, Type> data = tupleAsObject as Tuple<Node, Type>;
      Node node = data.Item1;
      Type type = data.Item2;

      INodeDelegate nodeDelegate = Activator.CreateInstance(type) as INodeDelegate;
      if (nodeDelegate == null) {
        Debug.LogError("AddNodeDelegateToNode - Failed to cast created type as INodeDelgate!");
        return;
      }

      node.AddNodeDelegate(nodeDelegate);
      this.SetTargetDirty();
    }

    private void AddTransitionCondition(object tupleAsObject) {
      Tuple<NodeTransition, Type> data = tupleAsObject as Tuple<NodeTransition, Type>;
      NodeTransition nodeTransition = data.Item1;
      Type type = data.Item2;

      ITransitionCondition transitionCondition = Activator.CreateInstance(type) as ITransitionCondition;
      if (transitionCondition == null) {
        Debug.LogError("AddTransitionCondition - Failed to cast created type as ITransitionCondition!");
        return;
      }

      nodeTransition.transition.AddTransitionCondition(transitionCondition);
      this.SetTargetDirty();
    }
  }
}