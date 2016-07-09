using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class Node : INode, ISerializationCallbackReceiver, IComparable<Node>{
    // PRAGMA MARK - Static Public Interface
    public static bool operator ==(Node a, Node b) {
      // If both are null, or both are same instance, return true.
      if (System.Object.ReferenceEquals(a, b)) {
        return true;
      }

      // If one is null, but not both, return false.
      if (((object)a == null) || ((object)b == null)) {
        return false;
      }

      return a.Equals(b);
    }

    public static bool operator !=(Node a, Node b) {
      return !(a == b);
    }

    public static Node DeepClone(Node n) {
      Node clone = new Node(n.Id);
      clone._nodeDelegates = n._nodeDelegates.Select(d => d.DeepClone()).ToList();

      return clone;
    }


    // PRAGMA MARK - Public Interface
    public event Action OnEnter = delegate {};
    public event Action OnExit = delegate {};
    public event Action<Node> OnManualExit = delegate {};

    public Node(NodeId id) {
      this._id = id;
    }

    public void AddNodeDelegate(INodeDelegate nodeDelegate) {
      this._nodeDelegates.Add(nodeDelegate);
    }

    public void RemoveNodeDelegate(INodeDelegate nodeDelegate) {
      bool successful = this._nodeDelegates.Remove(nodeDelegate);
      if (!successful) {
        Debug.LogError("Node - RemoveNodeDelegate called with node delegate not found!");
      }
    }

    public IList<INodeDelegate> GetNodeDelegates() {
      return this._nodeDelegates;
    }

    public override bool Equals(object obj) {
      if (obj == null) {
        return false;
      }

      Node otherNode = obj as Node;
      if (otherNode == null) {
        return false;
      }

      return otherNode.Id == this.Id;
    }

    public bool Equals(Node otherNode) {
      if (otherNode == null) {
        return false;
      }

      return otherNode.Id == this.Id;
    }

    public override int GetHashCode() {
      return base.GetHashCode() ^ this.Id;
    }


    // PRAGMA MARK - INode Implementation
    public NodeId Id {
      get { return this._id; }
    }

    public bool IsManuallyExited {
      get; private set;
    }

    public void HandleEnter() {
      this.IsManuallyExited = false;
      foreach (INodeDelegate nodeDelegate in this._nodeDelegates) {
        nodeDelegate.HandleEnter();
      }
      this.OnEnter.Invoke();
    }

    public void HandleExit() {
      this.IsManuallyExited = false;
      foreach (INodeDelegate nodeDelegate in this._nodeDelegates) {
        nodeDelegate.HandleExit();
      }
      this.OnExit.Invoke();
    }

    public void TriggerManualExit() {
      this.IsManuallyExited = true;
      this.OnManualExit.Invoke(this);
    }


    // PRAGMA MARK - ISerializationCallbackReceiver Implementation
    public void OnAfterDeserialize() {
      if (this._nodeDelegates == null) {
        this._nodeDelegates = new List<INodeDelegate>();
      } else {
        this._nodeDelegates.Clear();
      }

      foreach (string serializedNodeDelegate in this._serializedNodeDelegates) {
        INodeDelegate nodeDelegate = JsonSerialization.DeserializeGeneric<INodeDelegate>(serializedNodeDelegate);
        if (nodeDelegate != null) {
          this._nodeDelegates.Add(nodeDelegate);
        }
      }
    }

    public void OnBeforeSerialize() {
      if (this._serializedNodeDelegates == null) {
        this._serializedNodeDelegates = new List<string>();
      } else {
        this._serializedNodeDelegates.Clear();
      }

      if (this._nodeDelegates == null) {
        return;
      }

      foreach (INodeDelegate nodeDelegate in this._nodeDelegates) {
        string serializedNodeDelegate = JsonSerialization.SerializeGeneric(nodeDelegate);
        if (serializedNodeDelegate != null) {
          this._serializedNodeDelegates.Add(serializedNodeDelegate);
        }
      }
    }


    // PRAGMA MARK - IComparable<Node> Implementation
    public int CompareTo(Node other) {
      return this.Id.CompareTo(other.Id);
    }


    // PRAGMA MARK - Internal
    [SerializeField] private NodeId _id;
    [SerializeField] private List<string> _serializedNodeDelegates = new List<string>();

    private List<INodeDelegate> _nodeDelegates = new List<INodeDelegate>();
  }
}