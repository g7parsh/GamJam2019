using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMiningBeam : MonoBehaviour
{
    private Material BeamMat;
    private LineRenderer Line;
    private Transform SourceTransform;
    private Transform DestTransform;

    private void Awake()
    {
        Line = GetComponent<LineRenderer>();
    }

    public void Initialize(Transform source, Transform dest)
    {
        SourceTransform = source;
        DestTransform = dest;
        Line.SetPositions(new Vector3[] { source.position, dest.position });
    }

    // Update is called once per frame
    void Update()
    {
        Line.SetPosition(0, SourceTransform.position);
        Line.SetPosition(Line.positionCount-1, DestTransform.position);
    }
}
