using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(ARJointGroupEntry))]
public class ARGroupEntryPropertyDrawer : PropertyDrawer
{
    public static string treePath = "Assets/RagdollExperiments/AREntity/Editor/ARGroupEntryDrawer.uxml";
    
    public override VisualElement CreatePropertyGUI(SerializedProperty property) {
        VisualElement toReturn = new VisualElement();
        VisualTreeAsset a = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(treePath);
        a.CloneTree(toReturn);
        return toReturn;
    }
}
