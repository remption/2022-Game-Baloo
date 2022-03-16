using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(ConfigurableJoint))]
public class ConfigurableJointInspector : Editor
{
    public VisualTreeAsset m_InspectorXML;
    public override VisualElement CreateInspectorGUI()
    {
        
  
        VisualElement myInspector = new VisualElement();
        m_InspectorXML.CloneTree(myInspector);

        ConfigurableJoint j;
       // j.targetRotation.eulerAngles
       // Rigidbody body = new Rigidbody();

        //WORKS
         IMGUIContainer defaultInspect = myInspector.Q<IMGUIContainer>("DefaultContainer");
        if (defaultInspect.enabledInHierarchy)
        defaultInspect.onGUIHandler = () => DrawDefaultInspector();

        //Doesn't work but should according to docs!
        // VisualElement defaultFoldout = myInspector.Q("Default_Inspector");
        //InspectorElement.FillDefaultInspector(defaultFoldout, serializedObject, this);


        //test.style.display = DisplayStyle.None;
        // return base.CreateInspectorGUI();


        return myInspector;
    }


   /* public override void OnInspectorGUI()
    {
        DrawPropertiesExcluding(serializedObject, );
        Rigidbody buts;

       SerializedProperty curProp = serializedObject.GetIterator();
        bool stillGoing = curProp != null;
       while(stillGoing)
        {
            EditorGUILayout.PropertyField(curProp);
            //TODO - take care of the children recurisively
            stillGoing = curProp.Next(false);
        }
    }*/
}
