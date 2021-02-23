using UnityEngine;

public class Zoom : MonoBehaviour
{
    public float sensitivity = 1;
    Camera cam;
    [HideInInspector]
    public float defaultFOV;
    [Tooltip("Effectively the min FOV that we can reach while zooming with this camera.")]
    public float maxZoom = 15;
    [HideInInspector]
    public float zoomAmount;


    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        defaultFOV = cam.fieldOfView;
    }

    void Update()
    {
        zoomAmount += Input.mouseScrollDelta.y * sensitivity * .05f;
        zoomAmount = Mathf.Clamp01(zoomAmount);
        cam.fieldOfView = Mathf.Lerp(defaultFOV, maxZoom, zoomAmount);
    }
}
