using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour {

	public float CameraMoveSpeed = 120.0f;
	public GameObject CameraFollowObj;
	Vector3 FollowPOS;
	public float clampAngle = 80.0f;
	public float inputSensitivity = 150.0f;
	public GameObject CameraObj;
	public GameObject PlayerObj;
	public float camDistanceXToPlayer;
	public float camDistanceYToPlayer;
	public float camDistanceZToPlayer;
	public float mouseX;
	public float mouseY;
	public float finalInputX;
	public float finalInputZ;
	public float smoothX;
	public float smoothY;
	private float rotY = 0.0f;
	private float rotX = 0.0f;

    public bool yAxis;

    public float yAxisDown = 30.0f;
    public float yAxisUp = 30.0f;

	void Start ()
    {
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
        if (PlayerPrefs.GetInt("LockCam") == 1)
        {
            yAxis = true;
        }
        else
        {
            yAxis = false;
        }
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (yAxis == true)
            {
                yAxis = false;
                PlayerPrefs.SetInt("LockCam", 0);
            }
            else
            {
                yAxis = true;
                PlayerPrefs.SetInt("LockCam", 1);
            }
        }

        if (Input.GetMouseButton(1))
        {
            float inputX = Input.GetAxis("RightStickHorizontal");
            float inputZ = Input.GetAxis("RightStickVertical");
            mouseX = Input.GetAxis("Mouse X");
            if (yAxis)
            {
                mouseY = Input.GetAxis("Mouse Y");
            }
            finalInputX = inputX + mouseX;
            finalInputZ = inputZ + mouseY;

            rotY += finalInputX * inputSensitivity * Time.deltaTime;
            rotX += finalInputZ * inputSensitivity * Time.deltaTime;
            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
            rotX = Mathf.Clamp(rotX, -yAxisDown, yAxisUp);

            Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            transform.rotation = localRotation;
        }

    }

    void LateUpdate () {
		CameraUpdater ();
	}

	void CameraUpdater() {
		// set the target object to follow
		Transform target = CameraFollowObj.transform;

		//move towards the game object that is the target
		float step = CameraMoveSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, target.position, step);
	}
}
