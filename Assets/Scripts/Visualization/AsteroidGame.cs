using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidGame : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public GameObject enemyPrefab;
    public GameObject playerPrefab;
    public GameObject gameOverTextObject;
    public GameObject XCoordinateTextObject;
    public GameObject YCoordinateTextObject;
    public GameObject dXCoordinateTextObject;
    public GameObject dYCoordinateTextObject;
    public GameObject angleTextObject;
    public GameObject laserShotsTextObject;
    public GameObject laserRechargeTextObject;
    public static AsteroidGame S { get; private set; }

    private bool isGameOver;

    private void Awake()
    {
        S = this;
        isGameOver = false;
        gameObject.GetComponent<ScreenBounds>().AwakeCall();
        GameController.Awake();
       
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverTextObject.SetActive(false);
        XCoordinateTextObject.SetActive(true);
        YCoordinateTextObject.SetActive(true);
        dXCoordinateTextObject.SetActive(true);
        dYCoordinateTextObject.SetActive(true);
        angleTextObject.SetActive(true);
        laserShotsTextObject.SetActive(true);
        laserRechargeTextObject.SetActive(true);
        GameController.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {          
            GameController.Update();
            XCoordinateTextObject.GetComponent<Text>().text = GameController.XCoorditaneText;
            YCoordinateTextObject.GetComponent<Text>().text = GameController.YCoorditaneText;
            dXCoordinateTextObject.GetComponent<Text>().text = GameController.dXCoorditaneText;
            dYCoordinateTextObject.GetComponent<Text>().text = GameController.dYCoorditaneText;
            angleTextObject.GetComponent<Text>().text = GameController.angleText;
            laserShotsTextObject.GetComponent<Text>().text = GameController.laserShotsText;
            laserRechargeTextObject.GetComponent<Text>().text = GameController.laserRechargeTimeText;
        }
        else
        {
            if (Input.GetKeyDown("space"))
            {
                Awake();
                Start();
                isGameOver = false;
            }
        }
    }

    public void PlayerSpawn(Physics player, System.Numerics.Vector2 position)
    {
        Instantiate(playerPrefab, new Vector2(position.X, position.Y), new Quaternion());
    }

    public void AsteroidSpawn(Physics asteroid, System.Numerics.Vector2 position)
    {
        GameObject ast = Instantiate(asteroidPrefab, new Vector2(position.X, position.Y), new Quaternion());
        ast.GetComponent<AsteroidController>().physics = asteroid;
        ast.GetComponent<AsteroidController>().isShard = ((AsteroidPhysics)asteroid).isShard;
    }

    public void EnemySpawn(Physics enemy, System.Numerics.Vector2 position)
    {
        GameObject ast = Instantiate(enemyPrefab, new Vector2(position.X, position.Y), new Quaternion());
        ast.GetComponent<EnemyController>().physics = enemy;
    }

    public void GameOver(string gameOverText)
    {
        gameOverTextObject.GetComponent<Text>().text = gameOverText;
        gameOverTextObject.SetActive(true);
        XCoordinateTextObject.SetActive(false);
        YCoordinateTextObject.SetActive(false);
        dXCoordinateTextObject.SetActive(false);
        dYCoordinateTextObject.SetActive(false);
        angleTextObject.SetActive(false);
        laserShotsTextObject.SetActive(false);
        laserRechargeTextObject.SetActive(false);
        isGameOver = true;
    }
}
