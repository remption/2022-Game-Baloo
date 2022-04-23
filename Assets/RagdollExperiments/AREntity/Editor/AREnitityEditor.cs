using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(AREntity))]
public class AREnitityEditor : Editor
{
    public VisualTreeAsset m_InspectorXML;
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement myInspector = new VisualElement();
        m_InspectorXML.CloneTree(myInspector);

        Button button = myInspector.Q<Button>("GatherJointsButton");
        button.clickable.clicked += GatherJointsIntoARJointDataObjects;
        
        button = myInspector.Q<Button>("SmartAssignMotionSources");
        button.clickable.clicked += GatherJointsIntoARJointDataObjects;

        ObjectField obF = myInspector.Q<ObjectField>("MotionSourceField");
        obF.RegisterValueChangedCallback(evt => {
            
            HideShowAssignMotionButton(button,obF); });

        button.SetEnabled ( obF.value != null); // only show the "smartAssign" button if there is a source object given to draw from
        
        return myInspector;
    }
    
    void HideShowAssignMotionButton(Button button, ObjectField objF)
    {
        button.SetEnabled(objF.value != null);
    }



    #region jointGathering
    /// <summary>
    /// Gets all of the ConfigurableJoints in our GameObject's hierarchy.
    /// Then, checks each joint against the entity's existing joints list.
    /// If a joint isn't in the list, it is added via its own new ARJointData.
    /// </summary>
    void GatherJointsIntoARJointDataObjects()
    {
        AREntity entity = (AREntity)target;
        Transform t = entity.transform;
        if(entity.joints ==null) entity.joints = new List<ARJointData>();

        //Gather joints, and then check each. For each one that is not in the entity.joints list, make a new ARJointData and add it! 
        ConfigurableJoint[] potentialJoints =  entity.GetComponentsInChildren<ConfigurableJoint>();
        for(int i = 0; i < potentialJoints.Length; i++) {
            if (!ListContainsConfigurableJoint(potentialJoints[i], entity.joints)) {
                ARJointData ajd = new ARJointData();
                ajd.joint = potentialJoints[i];
                entity.joints.Add(ajd);
            }
        }
    }

    /// <summary>
    /// For each joint in the AREntity's "joint" list, search and see if there is an identically named 
    /// tranform in the "motionSourceRoot" heirarchy. If so, assign that identically named transform 
    /// to be the joint's motion source.  
    /// 
    /// This method may assign incorrect objects if there are identicaly named objects in the motion source heirarchy,
    /// as it doesn't take into consideration an object's position in the hierarchy. So, unique naming is important.
    /// </summary>
    void SmartAssignMotionSources() {
        AREntity entity = (AREntity)target;
        if (entity.motionSourceRoot == null || entity.joints == null || entity.joints.Count == 0) return;

        ARJointData cur;
        for (int i = 0; i < entity.joints.Count; i++) {
            //get joint and make sure it doesn't have derailing null values
            cur = entity.joints[i];
            if(cur == null || cur.joint == null) continue;
            // if there is a child in the motion source with a matching name, make it the source!
            cur.motionSource = entity.motionSourceRoot.transform.FindDeepChild(cur.joint.name);
        }
    }

    


    #endregion

    /// <summary>
    /// True if the ARJointData list contains an entry for the given joint already
    /// </summary>
    /// <param name="j"></param>
    /// <param name="_arDatas"></param>
    /// <returns></returns>
    bool ListContainsConfigurableJoint(ConfigurableJoint j, List<ARJointData> _arDatas)
    {
        if (_arDatas == null) return false;
        for (int i = 0;i < _arDatas.Count; i++)
        {
            if(_arDatas[i].joint == j)return true;
        }

        return false;
    }

}
