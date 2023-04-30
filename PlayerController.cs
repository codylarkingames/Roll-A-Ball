using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float speed;
    public GameObject winText;
    public GameObject nextLevel;
    public GameObject obstacles;
    public GameObject pauseMenu;
    public GameObject restartLevel;
    public Transform cam;
    public AudioSource obstacle;
    public static bool obstaclesOn = true;

    private Collider[] obstacleTrigger;
    public static GameObject[] cubes;
    private Rigidbody rb;
    private Scene scene;
    public static int count;
    private int pickups;
    private bool cursorOn;
    public bool musicon;
    private bool next;
    private string sceneName;

    void Start()
    {
        obstaclesOn = true;
        obstacle = GameObject.FindGameObjectWithTag("ObstacleSound").GetComponent<AudioSource>();
        scene = SceneManager.GetActiveScene();
        sceneName = scene.name;
        musicon = true;
        if (sceneName == "_Title")
        {
            cursorOn = true;
            next = true;
        }
        else if (sceneName != "_Title")
        {
            cursorOn = false;
            next = false;
        }
        count = 0;
        rb = GetComponent<Rigidbody>();
        nextLevel.SetActive(false);
        obstacleTrigger = obstacles.GetComponentsInChildren<Collider>();
        print(obstaclesOn);
    }

    void FixedUpdate ()
    {
        Vector2 inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (inputDirection.sqrMagnitude > 1)
        {
            inputDirection = inputDirection.normalized;
        }
        Vector3 newRight = Vector3.Cross(Vector3.up, cam.forward);
        Vector3 newForward = Vector3.Cross(newRight, Vector3.up);
        Vector3 movement = (newRight * inputDirection.x) + (newForward * inputDirection.y);

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        rb.AddForce(movement * speed);
    }

    private void Update()
    {
        cubes = GameObject.FindGameObjectsWithTag("Pick Up");
        pickups = cubes.Length;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle") || (other.gameObject.CompareTag("Ammo")))
        {
            if (obstaclesOn && this.tag == "Player")
            {
                PlayerPrefs.SetInt("NewLevel", 1);
                obstacle.Play();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                lvl22_02.rideActive = false;
                lvl22_02.rideEnd = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            if (SceneManager.GetActiveScene().name == "_Title")
            {
                other.gameObject.GetComponent<MeshRenderer>().enabled = false;
                this.gameObject.GetComponent<AudioSource>().Play();
            }
            else
            {
                this.gameObject.GetComponent<AudioSource>().Play();
                other.gameObject.GetComponent<MeshRenderer>().enabled = false;
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
                count = count + 1;
                SetCountText();
            }
        }
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (obstaclesOn)
            {
                PlayerPrefs.SetInt("NewLevel", 1);
                obstacle.Play();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        if (other.gameObject.CompareTag("Item"))
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void SetCountText()
    {
        if (SceneManager.GetActiveScene().name != "Level_25")
        {
            if (count >= pickups)
            {
                winText.SetActive(true);
                nextLevel.SetActive(true);
                pauseMenu.SetActive(false);
                restartLevel.SetActive(false);
                this.gameObject.GetComponent<PlayerController>().enabled = false;
                this.gameObject.GetComponent<LevelComplete>().enabled = true;
                foreach (Collider obstacleTriggers in obstacleTrigger)
                {
                    obstacleTriggers.isTrigger = false;
                }
                next = true;
            }
        }
    }

}


