using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This is the default controls for handling camera movement on the world map.
/// Attach this script to the main camera.
/// </summary>
public class CameraHandler : MonoBehaviour
{
    private Camera Camera;

    protected static float ZOOM_SPEED = 0.4f; // Mouse Wheel Speed
    protected static float DRAG_SPEED = 0.025f; // Middle Mouse Drag Speed
    protected static float PAN_SPEED = 20f; // WASD Speed
    protected static float MIN_CAMERA_SIZE = 3f;
    protected static float MAX_CAMERA_SIZE = 30f;
    protected bool IsLeftMouseDown;
    protected bool IsRightMouseDown;
    protected bool IsMouseWheelDown;

    public void FocusPosition(Vector2 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }

    private void Start()
    {
        Camera = GetComponent<Camera>();
    }


    public virtual void Update()
    {
        // Position Boundaries
        float minX = 0, maxX = 0, minY = 0, maxY = 0;

        minX = World.Singleton.MinWorldX;
        maxX = World.Singleton.MaxWorldX;
        minY = World.Singleton.MinWorldY;
        maxY = World.Singleton.MaxWorldY;

        // Scroll
        if (Input.mouseScrollDelta.y != 0)
        {
            Camera.orthographicSize += -Input.mouseScrollDelta.y * ZOOM_SPEED;

            // Zoom Boundaries
            if (Camera.orthographicSize < MIN_CAMERA_SIZE) Camera.orthographicSize = MIN_CAMERA_SIZE;
            if (Camera.orthographicSize > MAX_CAMERA_SIZE) Camera.orthographicSize = MAX_CAMERA_SIZE;
        }

        // Dragging with right/middle mouse button
        if (Input.GetKeyDown(KeyCode.Mouse2)) IsMouseWheelDown = true;
        if (Input.GetKeyUp(KeyCode.Mouse2)) IsMouseWheelDown = false;
        if (Input.GetKeyDown(KeyCode.Mouse1)) IsRightMouseDown = true;
        if (Input.GetKeyUp(KeyCode.Mouse1)) IsRightMouseDown = false;
        if (IsMouseWheelDown)
        {
            float speed = DRAG_SPEED * Camera.orthographicSize;
            float canvasScaleFactor = GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;
            transform.position += new Vector3(-Input.GetAxis("Mouse X") * speed / canvasScaleFactor, -Input.GetAxis("Mouse Y") * speed / canvasScaleFactor, 0f);
        }

        // Panning with WASD
        if(Input.GetKey(KeyCode.W)) transform.position += new Vector3(0f, PAN_SPEED * Time.deltaTime, 0f);
        if(Input.GetKey(KeyCode.A)) transform.position += new Vector3(-PAN_SPEED * Time.deltaTime, 0f, 0f);
        if(Input.GetKey(KeyCode.S)) transform.position += new Vector3(0f, -PAN_SPEED * Time.deltaTime, 0f);
        if(Input.GetKey(KeyCode.D)) transform.position += new Vector3(PAN_SPEED * Time.deltaTime, 0f, 0f);

        // Drag triggers
        if (Input.GetKeyDown(KeyCode.Mouse0) && !IsLeftMouseDown)
        {
            IsLeftMouseDown = true;
            OnLeftMouseDragStart();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && IsLeftMouseDown)
        {
            IsLeftMouseDown = false;
            OnLeftMouseDragEnd();
        }

        // Bounds
        if (transform.position.x < minX) transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        if (transform.position.x > maxX) transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        if (transform.position.y < minY) transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        if (transform.position.y > maxY) transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
    }

    public static CameraHandler Singleton => Camera.main.GetComponent<CameraHandler>();

    #region Triggers

    protected virtual void OnLeftMouseDragStart() { }

    protected virtual void OnLeftMouseDragEnd() { }

    #endregion
}
