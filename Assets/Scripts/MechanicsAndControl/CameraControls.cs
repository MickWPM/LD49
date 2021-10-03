using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public Transform rotationTransform;

    public Transform zoomedInPosTransform, zoomedOutPosTransform;


    public GameOptionsPersistent gameOptionsPersistent;
    GameManager gameManager;
    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.GameOverEvent += GameManager_GameOverEvent;
    }
    bool gameover = false;
    private void GameManager_GameOverEvent(GameManager.GameOverCause obj)
    {
        gameover = true;
    }
    public float DEBUG_ZOOM;
    [Range(0, 1)]
    public float zoom = 0;
    void LateUpdate()
    {
        float zoomSpeed = 5;
        if (gameOptionsPersistent == null)
        {
            GameObject.FindObjectOfType<GameOptionsPersistent>();
            if(gameOptionsPersistent != null)
            {
                AudioListener.volume = gameOptionsPersistent.MuteAudio ? 0 : 1;
            }
        } else
        {
            zoomSpeed = gameOptionsPersistent.MouseScrollZoomSpeed;
        }

        if (gameManager.GameOptionsPersistent != null)
        {
            zoomSpeed = gameManager.GameOptionsPersistent.MouseScrollZoomSpeed;
        }
        DEBUG_ZOOM = zoomSpeed;

        if(gameover)
        {
            zoom = Mathf.Clamp01(zoom + Input.mouseScrollDelta.y * Time.unscaledDeltaTime * zoomSpeed);
            rotationTransform.Rotate(new Vector3(0, rotationSpeed * Time.unscaledDeltaTime * -Input.GetAxisRaw("Horizontal"), 0));
            transform.position = Vector3.Lerp(zoomedOutPosTransform.position, zoomedInPosTransform.position, zoom);
            transform.rotation = Quaternion.Lerp(zoomedOutPosTransform.rotation, zoomedInPosTransform.rotation, zoom);
        } else
        {
            zoom = Mathf.Clamp01(zoom + Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed);
            rotationTransform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime * -Input.GetAxisRaw("Horizontal"), 0));
            transform.position = Vector3.Lerp(zoomedOutPosTransform.position, zoomedInPosTransform.position, zoom);
            transform.rotation = Quaternion.Lerp(zoomedOutPosTransform.rotation, zoomedInPosTransform.rotation, zoom);
        }
    }
}
