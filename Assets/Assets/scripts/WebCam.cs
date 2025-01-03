using UnityEngine;

public class WebCamScript : MonoBehaviour
{
    private WebCamTexture webcamTexture;

    void Start()
    {
        // Get available devices
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length > 0)
        {
            // Assuming the first device is the front camera (or you can select a specific camera index)
            int cameraIndex = 1;  // Change this if you need to use a different camera (e.g., 1 for rear camera)

            // Check if the camera has a name
            if (devices[cameraIndex].isFrontFacing)
            {
                Debug.Log("Using Front Camera");
            }
            else
            {
                Debug.Log("Using Rear Camera");
            }

            // Create a WebCamTexture using the selected camera
            webcamTexture = new WebCamTexture(devices[cameraIndex].name);

            // Apply the webcam texture to the material
            GetComponent<Renderer>().material.mainTexture = webcamTexture;

            // Start the webcam feed
            webcamTexture.Play();
        }
        else
        {
            Debug.LogError("No webcam detected.");
        }
    }

    void OnApplicationQuit()
    {
        // Stop the webcam when the application quits
        if (webcamTexture != null && webcamTexture.isPlaying)
        {
            webcamTexture.Stop();
        }
    }
}
