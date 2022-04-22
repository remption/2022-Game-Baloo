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
    void GatherJointsIntoARJointDataObjects()
    {
        AREntity entity = (AREntity)target;
        Transform t = entity.transform;
        if(entity.joints ==null) entity.joints = new List<ARJointData>();

        GatherJointsHelper(t, entity.joints);
    }

    //Creates depth-first list :)
    void GatherJointsHelper(Transform toCollectFrom, List<ARJointData> toAddTo)
    {
        if (toCollectFrom == null) return;

        ConfigurableJoint foundJoint = toCollectFrom.GetComponent<ConfigurableJoint>();
        if (foundJoint != null)
        {
           if (!ListContainsConfigurableJoint(foundJoint, toAddTo))
            {
                ARJointData ajd = new ARJointData();
                ajd.joint = foundJoint;
                toAddTo.Add(ajd);
            }
        }
        for (int i = 0; i < toCollectFrom.childCount; i++)
        {
            GatherJointsHelper(toCollectFrom.GetChild(i), toAddTo);
        }
    }

    /// <summary>
    /// True if the ARJointData list contains an entry for the given joint already
    /// </summary>
    /// <param name="j"></param>
    /// <param name="_arDatas"></param>
    /// <returns></returns>

    #endregion
   
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
