using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[ExecuteInEditMode]
[DisallowMultipleComponent]
public class TrackSplineGenerator : MonoBehaviour
{

    [InspectorButton("OnButtonClicked")]
    public bool clickMe;

    private void OnButtonClicked()
    {
        var checkpoints = GameObject.Find("Checkpoints");
        BezierPath bezierPath = new BezierPath(checkpoints.GetComponentsInChildren<Transform>(), true, PathSpace.xyz);
        if (!GameObject.Find("TrackSpline"))
        {
            GameObject trackObject = new GameObject("TrackSpline");
            trackObject.AddComponent<PathCreator>();
            trackObject.GetComponent<PathCreator>().bezierPath = bezierPath;
        }
        else
        {
            GameObject.Find("TrackSpline").GetComponent<PathCreator>().bezierPath = bezierPath;
        }
    }
}
