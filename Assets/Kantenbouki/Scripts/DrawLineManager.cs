using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineManager : MonoBehaviour {


    public SteamVR_TrackedObject trackedObj;
    public ColorManager colorMgr;

    private MeshLineRenderer currLine;

    private int numClicks = 0;

    public Material lMat;

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            GameObject go = new GameObject();
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            currLine = go.AddComponent<MeshLineRenderer>();

            currLine.setWidth(0.1f);
            
           currLine.lmat = this.lMat;

            if (go.GetComponent<MeshRenderer>().material != null)
            {
                 go.GetComponent<MeshRenderer>().material.color = colorMgr.color;
            }


            numClicks = 0;
        }
        else if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            //currLine.SetVertexCount(numClicks + 1);
            //currLine.SetPosition(numClicks, trackedObj.transform.position);

            currLine.AddPoint(trackedObj.transform.position);
            numClicks++;
        }
    }
}
