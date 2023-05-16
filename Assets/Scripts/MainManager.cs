using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public GameObject HighscoreText;
    
    private bool m_Started = false;
    private int m_Points;
    private int m_HighscorePosition;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        //Checks if current score is a highscore
        m_HighscorePosition = CheckHighscore(m_Points);        
        if (m_HighscorePosition >= 0)
        {
            HighscoreText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "NEW HIGHSCORE: " + m_Points + "!";
            HighscoreText.SetActive(true);   
        }
        else
        {
            m_GameOver = true;
            GameOverText.SetActive(true);
        }
    }

    public int CheckHighscore(int score)
    {
        if(DataManager.Instance != null)
        {
            for (int i = 0; i < DataManager.Instance.highscore.Length; i++)
            {
                if(score > DataManager.Instance.highscore[i])
                {
                    //Return the position of the highscore
                    return i;
                }
            }
        }
        //Return -1 if DataManager.Instance is null or if the score is not a highscore
        return -1;
    }

    public void UpdateHighscore()
    {       
        for (int i = DataManager.Instance.highscore.Length - 1; i > m_HighscorePosition; i--)
        {
            Debug.Log("i = " + i);
            DataManager.Instance.highscore[i] = DataManager.Instance.highscore[i - 1];
            DataManager.Instance.names[i] = DataManager.Instance.names[i - 1];
        }
        DataManager.Instance.highscore[m_HighscorePosition] = m_Points;
        DataManager.Instance.names[m_HighscorePosition] = DataManager.Instance.playerName;
        DataManager.Instance.SaveHighscore();
    }

    public void SetPlayerName(string newName)
    {
        DataManager.Instance.playerName = newName;
    }
}
