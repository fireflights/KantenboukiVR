using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour {

    public SteamVR_LaserPointer laserPointer;
    public ColorManager cm;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //GetComponent<Renderer>().material.color = cm.color;
        laserPointer.color = cm.color;
		
	}
}
