using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;

    private Vector3 offset;
    private float moveSpeed;

    // Use this for initialization
    void Start () {
        offset = transform.position - player.transform.position;
        moveSpeed = 25;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = player.transform.position + offset;
        }
    }

