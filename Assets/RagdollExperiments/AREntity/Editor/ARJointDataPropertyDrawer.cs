using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

[CustomPropertyDrawer(typeof(ARJointData))]
public class ARJointDataPropertyDrawer : PropertyDrawer
{
    public string treePath = "Assets/RagdollExperiments/AREntity/Editor/ARJointDataPropertyDrawer.uxml";

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement toReturn = new VisualElement();
        VisualTreeAsset a = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(treePath);
        a.CloneTree(toReturn);
        return toReturn;

    }
}
