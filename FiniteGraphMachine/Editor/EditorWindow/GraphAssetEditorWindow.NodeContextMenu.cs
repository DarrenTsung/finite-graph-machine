
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTFiniteGraphMachine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Internal
    private void OpenNodeContextMenu(Event currentEvent, Node node) {
      GenericMenu nodeCreationMenu = new GenericMenu();
      nodeCreationMenu.AddItem(new GUIContent("Make Transition"), false, this.MakeTransition, node);
      nodeCreationMenu.AddSeparator("");
      if (this.TargetGraph.IsStartingNode(node)) {
        nodeCreationMenu.AddItem(new GUIContent("Unset Starting"), false, this.RemoveFromStartingNodes, node);
      } else {
        nodeCreationMenu.AddItem(new GUIContent("Set Starting"), false, this.AddToStartingNodes, node);
      }
      nodeCreationMenu.AddItem(new GUIContent("Remove"), false, this.RemoveNode, node);
      nodeCreationMenu.ShowAsContext();
    }

    private void MakeTransition(object nodeAsObject) {
      this.MakeTransition(nodeAsObject as Node);
    }

    private void MakeTransition(Node node) {
      NodeTransition nodeTransition = new NodeTransition();
      this.TargetGraph.AddOutgoingTransitionForNode(node, nodeTransition);
      this.StartEditingNodeTransition(node, nodeTransition);
    }

    private void AddToStartingNodes(object nodeAsObject) {
      Node node = nodeAsObject as Node;
      List<Node> startingNodes = this.TargetGraph.GetStartingNodes().ToList();
      startingNodes.Add(node);

      this.TargetGraph.SetStartingNodes(startingNodes);
      this.SetTargetDirty();
    }

    private void RemoveFromStartingNodes(object nodeAsObject) {
      Node node = nodeAsObject as Node;
      List<Node> startingNodes = this.TargetGraph.GetStartingNodes().ToList();
      bool successful = startingNodes.Remove(node);
      if (!successful) {
        Debug.LogError("RemoveFromStartingNodes - failed to remove node from starting nodes!");
        return;
      }

      this.TargetGraph.SetStartingNodes(startingNodes);
      this.SetTargetDirty();
    }

    private void RemoveNode(object nodeAsObject) {
      this.RemoveNode(nodeAsObject as Node);
    }

    private void RemoveNode(Node node) {
      this.TargetGraph.RemoveNode(node);
      this.SetTargetDirty();
    }
  }
}