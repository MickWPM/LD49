using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public Transform rotationTransform;

    public Transform zoomedInPosTransform, zoomedOutPosTransform;

    public float zoomSpeed = 10f;

    public GameOptionsPersistent gameOptionsPersistent;

    [Range(0, 1)]
    public float zoom = 0;
    void LateUpdate()
    {
        if(gameOptionsPersistent == null)
        {
            GameObject.FindObjectOfType<GameOptionsPersistent>();
            if(gameOptionsPersistent != null)
            {
                AudioListener.volume = gameOptionsPersistent.MuteAudio ? 0 : 1;
            }
        }
        zoom = Mathf.Clamp01(zoom + Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed);
        rotationTransform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime * -Input.GetAxisRaw("Horizontal"), 0));
        transform.position = Vector3.Lerp(zoomedOutPosTransform.position, zoomedInPosTransform.position, zoom);
        transform.rotation = Quaternion.Lerp(zoomedOutPosTransform.rotation, zoomedInPosTransform.rotation, zoom);
    }
}
