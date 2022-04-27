using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(ARJointGroup))]
public class ARJointGroupPropertyDrawer : PropertyDrawer
{
    public static string treePath = "Assets/RagdollExperiments/AREntity/Editor/ARJointGroupDrawer.uxml";
    public override VisualElement CreatePropertyGUI(SerializedProperty property) {
        VisualElement toReturn = new VisualElement();
        VisualTreeAsset tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(treePath);
        tree.CloneTree(toReturn);


        return toReturn;
    }
}
