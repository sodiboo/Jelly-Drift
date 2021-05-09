using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Transform))]
public class LevelReverse : Editor
{
    Editor defaultEditor;
    private void OnEnable()
    {
        defaultEditor = Editor.CreateEditor(targets, System.Type.GetType("UnityEditor.TransformInspector, UnityEditor"));
    }
    public override void OnInspectorGUI()
    {
        defaultEditor.OnInspectorGUI();
        var targets = this.targets.Cast<Transform>();
        
        if (GUILayout.Button("Reverse Children"))
        {
            foreach (var transform in targets)
            {
                var children = new List<Transform>();
                for (var i = transform.childCount - 1; i >= 0; i--)
                {
                    children.Add(transform.GetChild(i));
                }
                transform.DetachChildren();
                foreach (var child in children)
                {
                    child.SetParent(transform);
                }
            }
        }
        if (GUILayout.Button("Flip Rotation"))
        {
            foreach (var transform in targets)
            transform.Rotate(transform.up, 180);
        }
    }
}
