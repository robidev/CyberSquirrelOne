using UnityEngine;

public class FillScreenPlane:MonoBehaviour
{
    public float lUnityPlaneSize = 10.0f; // 10 for a Unity3d plane
    void Update()
    {
        Camera lCamera = Camera.main;

        if(lCamera.orthographic)
        {
            float lSizeY = lCamera.orthographicSize * 2.0f;
            float lSizeX = lSizeY *lCamera.aspect;
            transform.localScale = new Vector3(lSizeX/lUnityPlaneSize, 1,lSizeY/lUnityPlaneSize);
        }
        else
        {
            float lPosition = (lCamera.nearClipPlane + 0.01f);
            transform.position = lCamera.transform.position + lCamera.transform.forward * lPosition;
            transform.LookAt (lCamera.transform);
            transform.Rotate (90.0f, 0.0f, 0.0f);

            float h = (Mathf.Tan(lCamera.fieldOfView*Mathf.Deg2Rad*0.5f)*lPosition*2f) /lUnityPlaneSize;
            transform.localScale = new Vector3(h*lCamera.aspect,1.0f, h);
        }
    }
}