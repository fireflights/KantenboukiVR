using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteSelector : MonoBehaviour {

	private bool isActive = false;

	public GameObject paletteObj;
	public GameObject lineRenderer;
    public SteamVR_TrackedObject rightController;

    // Use this for initialization
    void Start () {
		
	}

	void Update()
	{
		SteamVR_Controller.Device device = SteamVR_Controller.Input((int)rightController.index);
		if (device.GetTouchDown (SteamVR_Controller.ButtonMask.Touchpad)) {

			isActive = !isActive;

			if (paletteObj != null) {
				paletteObj.SetActive (!isActive);
			}

			if (lineRenderer != null) {
				lineRenderer.SetActive (isActive);
			}

		}
	}
}
