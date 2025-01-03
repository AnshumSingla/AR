using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SimpleARPlacementManager : MonoBehaviour
{
    [SerializeField] private XROrigin _xrOrigin;
    [SerializeField] private ARPlaneManager _planeManager;
    [SerializeField] private ARRaycastManager _raycastManager;
    [SerializeField] private GameObject _placementObject;

    private GameObject _instantiatedObject = null;

    private List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();

    private void Update()
    {
        // Only handle touch input if it's not interacting with the UI
        if (Input.touchCount > 0 && !isButtonPressed())
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                bool collision = _raycastManager.Raycast(touch.position, _raycastHits, TrackableType.PlaneWithinPolygon);

                if (collision)
                {
                    Debug.Log("Raycast hit at position: " + _raycastHits[0].pose.position);

                    // Instantiate the object if not already instantiated
                    if (_instantiatedObject == null)
                    {
                        _instantiatedObject = Instantiate(_placementObject, _raycastHits[0].pose.position, _raycastHits[0].pose.rotation);
                        Debug.Log("Object instantiated at position: " + _raycastHits[0].pose.position);

                        // Disable planes after placement
                        foreach (ARPlane plane in _planeManager.trackables)
                        {
                            plane.gameObject.SetActive(false);
                        }
                        _planeManager.enabled = false;
                    }
                }
                else
                {
                    Debug.Log("Raycast did not hit any plane.");
                }
            }
        }
    }

    public void SwitchFurniture(GameObject furniture)
    {
        _placementObject = furniture;
    }

    // Check if a UI element is currently selected
    public bool isButtonPressed()
    {
        return EventSystem.current.currentSelectedGameObject?.GetComponent<Button>() != null;
    }
}
