using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class Done_GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public double timeOver;//used to calc percent of Pb bar.
    private double timeOverUpdate;//used to update the time in script


    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Text timeOverText;

    private bool gameOver;
    private bool restart;
    private static int score;
    private bool compleatedLevel;

    void Start()
    {
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        timeOverUpdate = timeOver;
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            }
        }
        if (compleatedLevel)
        {
          if (Input.anyKeyDown)//It will not return true until the user has released all keys / buttons and pressed any key / buttons again.
          {
          SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex) + 1);

          }
        }

        if (timeOverUpdate <= 0)//level Compleated time expired
        {
          gameOverText.text = "Level Compleated!";
          compleatedLevel = true;
        }

        timeOverUpdate = timeOverUpdate - 0.01666666667;//Countdown time
        AddScore(1);//add score
        float TotalTimeOver = (float)timeOver;
        GameObject.Find("UI_ProgressBar").GetComponent<ProgressBar>().BarValue += 0.01666666667f * TotalTimeOver;//update Pb bar

    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {

            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "Press 'R' for Restart";
                restart = true;
                break;
            }

        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
}
