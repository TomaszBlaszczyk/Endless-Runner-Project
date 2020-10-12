using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlatformGenerator : MonoBehaviour
{
    public Camera mainCamera;
    public Camera sideCamera;
    public Vector3 spawnPoint = new Vector3(0, 0, -0.5f);
    public PlatformTile tilePrefab;
    public float movingSpeed = 12;
    public int tilesToPreSpawn = 15; //Number of tiles showed in one time
    public int tilesWithoutObstacles = 3; //Number of tiles without obstacles at the beginning   
    public Canvas mainCanvas;  
    public static PlatformGenerator instance;
    public static bool gameStarted = false;

    List<PlatformTile> spawnedTiles = new List<PlatformTile>();  
    private MenuController menuController;

    private bool defaultCameraView = true;
    private Vector3 defaultCameraPosition = new Vector3(0, 5, -15);

    private float score = 0;
    public Text scoreUI;

    public bool gameOver = false;
    public Text gameOverUI;

    void Start()
    {
        menuController = FindObjectOfType<MenuController>();
        mainCanvas.enabled = false;
        gameOverUI.enabled = false;
        instance = this;

        if(PlayerPrefs.HasKey("CameraView"))
        {
            if(PlayerPrefs.GetInt("CameraView") == 1)
            {
                SetView_Side();
            }
            else
            {
                SetView_Default();
            }
        }

        Vector3 spawnPosition = spawnPoint;
        int tilesWithNoObstaclesTmp = tilesWithoutObstacles;

        //Spawn set number of tiles to PreSpawn
        for (int i = 0; i < tilesToPreSpawn; i++)
        {
            spawnPosition -= tilePrefab.startPoint.localPosition;
            PlatformTile spawnedTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
            if (tilesWithNoObstaclesTmp > 0)
            {
                spawnedTile.DisableAllObstacles();
                tilesWithNoObstaclesTmp--;
            }
            else
            {
                spawnedTile.GetRandomObstacle();
            }

            spawnPosition = spawnedTile.endPoint.position;
            spawnedTile.transform.SetParent(transform);
            spawnedTiles.Add(spawnedTile);
        }
    }

    void Update()
    {
        //Moving and adding points while playing
        if (!gameOver && gameStarted)
        {
            transform.Translate(-spawnedTiles[0].transform.forward * Time.deltaTime * (movingSpeed + (score / 500)), Space.World);
            score += Time.deltaTime * movingSpeed;
        }

        //Move the tile to the front if it's behind the Camera
        if (mainCamera.WorldToViewportPoint(spawnedTiles[0].endPoint.position).z < 0)
        {           
            PlatformTile tileTmp = spawnedTiles[0];
            spawnedTiles.RemoveAt(0);
            tileTmp.transform.position = spawnedTiles[spawnedTiles.Count - 1].endPoint.position - tileTmp.startPoint.localPosition;
            tileTmp.GetRandomObstacle();
            spawnedTiles.Add(tileTmp);
        }

        //Enable UI needs to start the game again
        if ((gameOver || !gameStarted) && mainCanvas.enabled == false && !menuController.paused)
        {
            mainCanvas.enabled = true;
        }

        scoreUI.text = "Score: " + (int)score;
    }

    public void StartButton() //Method used by UI to start the game
    {
        if (gameOver)
        {
            Time.timeScale = 1;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        else
        {
            gameStarted = true;
        }

        mainCanvas.enabled = false;
    }

    public void ChangeView() //Method used by UI to change camera's view
    {
        if(defaultCameraView)
        {
            SetView_Side();
        }
        else
        {
            SetView_Default();
        }

        defaultCameraView = !defaultCameraView;
    }

    private void SetView_Side() //Change position and rotation of camera to side view and save
    {
        mainCamera.gameObject.SetActive(false);
        sideCamera.gameObject.SetActive(true);
        PlayerPrefs.SetInt("CameraView", 1);
    }

    private void SetView_Default() //Change position and rotation of camera to default view and save
    {
        sideCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);  
        PlayerPrefs.SetInt("CameraView", 0);
    }
}