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

        //WORKS - Uses the IMGUIContainer I added in the builder to draw the default inspector.
        //This lets me grab property bindings easily as I work!
         /*IMGUIContainer defaultInspect = myInspector.Q<IMGUIContainer>("DefaultContainer");
        if (defaultInspect.enabledInHierarchy)
            defaultInspect.onGUIHandler = () => DrawDefaultInspector();*/

        //Doesn't work but should according to docs?
        // VisualElement defaultFoldout = myInspector.Q("Default_Inspector");
        //InspectorElement.FillDefaultInspector(defaultFoldout, serializedObject, this);

        return myInspector;
    }/**/

    public void HotPantsRomance()
    {


        //return true;
    }
}
