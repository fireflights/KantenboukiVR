using UnityEngine;
using Valve.VR;

public class CloudLabelSelect : MonoBehaviour
{
    public Transform thumb;
    bool dragging;

    public SteamVR_TrackedObject rightController;

    void FixedUpdate()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)rightController.index);

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            dragging = false;
            Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);
            //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (GetComponent<Collider>().Raycast(ray, out hit, 100))
            {
                dragging = true;
                ColorManager.Instance.cloudLabel = GetComponent<Collider>().gameObject.name;
                thumb.position = GetComponent<Collider>().gameObject.transform.position;
            }
        }
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)) dragging = false;
        if (dragging && device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {

            Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);
            //var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit hit;
            if (GetComponent<Collider>().Raycast(ray, out hit, 100))
            { 
                ColorManager.Instance.cloudLabel = GetComponent<Collider>().gameObject.name;
                thumb.position = GetComponent<Collider>().gameObject.transform.position;
            }

        }
    }
}
