using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineManager : MonoBehaviour {


    public SteamVR_TrackedObject trackedObj;

    public ColorManager colMgr;
    private MeshLineRenderer currLine;

    private int numClicks = 0;
    private int numClouds = 0;

    public Material lMat;

	// Use this for initialization
	void Start () {

        colMgr = ColorManager.Instance;
	}

    // Update is called once per frame
    void Update()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {

            //GameObject go = new GameObject(ColorManager.Instance.cloudLabel.name);

            GameObject go = new GameObject(ColorManager.Instance.cloudLabel);


            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            currLine = go.AddComponent<MeshLineRenderer>();


            currLine.setColorManager(colMgr);
            currLine.setWidth(0.1f);

            currLine.lmat = this.lMat;

            GetComponent<MeshRenderer>().material.color = colMgr.color;

            Vector3 dir = trackedObj.transform.position - trackedObj.transform.forward * 2.0f;
            Vector3 left = Vector3.Cross(dir, Vector3.up).normalized;

            GameObject label = Instantiate(Resources.Load(ColorManager.Instance.cloudLabel), trackedObj.transform.position + trackedObj.transform.forward * 2.0f, Camera.main.transform.rotation) as GameObject;
            label.transform.eulerAngles = new Vector3(label.transform.eulerAngles.x, label.transform.eulerAngles.y, 180); //label.transform.RotateAround(transform.position, transform.up, 180f);
            label.transform.localScale = new Vector3(0.5F, 0.5f, 0);

            label.transform.parent = go.transform;

            numClicks = 0;
        }
        else if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            //currLine.SetVertexCount(numClicks + 1);
            //currLine.SetPosition(numClicks, trackedObj.transform.position);


            currLine.AddPoint(trackedObj.transform.position + trackedObj.transform.forward * 2.0f);
            numClicks++;
        }
    }
}
