using UnityEngine;
using Valve.VR;

public class Draggable : MonoBehaviour
{

    public Transform minBound;

	public bool fixX;
	public bool fixY;
	public Transform thumb;	
	bool dragging;

    public SteamVR_TrackedObject rightController;

	void FixedUpdate()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)rightController.index);
		if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger) )
        { 
			dragging = false;
            Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);
            //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (GetComponent<Collider>().Raycast(ray, out hit, 100)) {
				dragging = true;
			}
		}
		if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)) dragging = false;
		if (dragging && device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)) {
           
            Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);
            //var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit hit;
            if (GetComponent<Collider>().Raycast(ray, out hit, 100))
            {
                var point = hit.point; 
                //point = GetComponent<Collider>().ClosestPointOnBounds(point);
                SetThumbPosition(point);
                SendMessage("OnDrag", Vector3.one - (thumb.localPosition - minBound.localPosition) / GetComponent<BoxCollider>().size.x);
            }
           
		}
	}

	void SetDragPoint(Vector3 point)
	{
		point = (Vector3.one - point) * GetComponent<Collider>().bounds.size.x + GetComponent<Collider>().bounds.min;
		SetThumbPosition(point);
	}

	void SetThumbPosition(Vector3 point)
	{
        Vector3 temp = thumb.localPosition;
        thumb.position = point;
		thumb.localPosition = new Vector3(fixX ? temp.x : thumb.localPosition.x, fixY ? thumb.localPosition.y : point.y, thumb.localPosition.z -1);
	}
}
