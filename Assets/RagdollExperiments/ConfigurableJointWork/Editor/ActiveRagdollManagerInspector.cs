using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Linq;
using System;
using System.Reflection;

[CustomEditor(typeof(ActiveRagdollJointManager))]
public class ActiveRagdollManagerInspector : Editor
{
    public VisualTreeAsset _myInspectorXML;
    public VisualTreeAsset _jointMiniViewXML;
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement myInspector = new VisualElement();
        _myInspectorXML.CloneTree(myInspector);

        IMGUIContainer defaultInspect = myInspector.Q<IMGUIContainer>("DefaultInpspectorContainer");
        if (defaultInspect.enabledInHierarchy)
           defaultInspect.onGUIHandler = () => DrawDefaultInspector();/**/

        IMGUIContainer jointPreviewContainer = myInspector.Q<IMGUIContainer>("JointPreviewContainer");
        if (jointPreviewContainer != null) jointPreviewContainer.onGUIHandler += DrawJointPreview;

        Button button = myInspector.Q<Button>("FindJointsButton");
        button.clickable.clicked += CollectJoints;

        button = myInspector.Q<Button>("SmartTagButton");
        button.clickable.clicked += SmartTag;


        return myInspector;
    }

    void CollectJoints()
    {
        ActiveRagdollJointManager jointMan = (ActiveRagdollJointManager)target;
        jointMan.CollectJoints(jointMan.gameObject);
    }
    void SmartTag()
    {
        ActiveRagdollJointManager jointMan = (ActiveRagdollJointManager)target;
        jointMan.SmartTagJoints();
    }


    void DrawJointPreview()
    {
        ActiveRagdollJointManager jman = (ActiveRagdollJointManager)target;
        if (jman._joints == null || jman._joints.Count == 0) ;
        else
        {
            for (int i = 0; i < jman._joints.Count; i++)
            {
                DrawSingleJointPreview(jman._joints[i],jman);
            }

        }
    }

    void DrawSingleJointPreview(ConfigurableJoint toDraw, ActiveRagdollJointManager manager)
    {
        SerializedProperty prop = serializedObject.FindProperty("");

        EditorGUILayout.BeginVertical();
        GUIContent gc = new GUIContent(toDraw.gameObject.name);
        EditorGUILayout.ObjectField(gc,toDraw, typeof(ConfigurableJoint));
        JointCopyMotion jcm = toDraw.GetComponent<JointCopyMotion>();
        if(jcm != null) { 
        

            

        }

        EditorGUILayout.EndVertical();
    }
}
