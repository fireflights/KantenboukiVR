using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PaletteSelector : MonoBehaviour {

	private bool isActive = false;

	public GameObject paletteObj;
	public GameObject lineRenderer;
    public SteamVR_TrackedObject rightController;
    public GameObject sphereIndicator;
    Vector2 touchpad;

    private float minDistance = 0.5f;
    private float maxDistance = 4.0f;

    // Use this for initialization
    void Start () {
		
	}

	void Update()
	{
		SteamVR_Controller.Device device = SteamVR_Controller.Input((int)rightController.index);
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {

			isActive = !isActive;

			if (paletteObj != null) {
				paletteObj.SetActive (!isActive);
			}

			if (lineRenderer != null) {
				lineRenderer.SetActive (isActive);
			}

            if (sphereIndicator != null) {
                sphereIndicator.SetActive(isActive);
            }

		}
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad) && !paletteObj.activeSelf)
        {
            //Read the touchpad values
            touchpad = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);


            // Handle movement via touchpad
            if (touchpad.y > 0.2f || touchpad.y < -0.2f && touchpad.x < 0.2f && touchpad.x > -0.2f)
            {
                // Move Forward
                sphereIndicator.transform.localPosition = new Vector3(0f, 0f, Mathf.Clamp(touchpad.y * 2.0f + 1.0f, minDistance, maxDistance));
            }

            //Debug.Log ("Touchpad X = " + touchpad.x + " : Touchpad Y = " + touchpad.y);
        }
    }
}
