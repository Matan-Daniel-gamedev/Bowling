using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    // Move the ball
    // Manage the score
    // Manage the turns

    public GameObject ball;
    int score = 0;
    GameObject[] pins;
    public Text scoreUI;
    int turnCounter = 0;
    Vector3[] positions;
    bool check_count = true;
    public int build_index;
    public GameObject panelObject;

    // Start is called before the first frame update
    void Start()
    {
        pins = GameObject.FindGameObjectsWithTag("Pin");
        positions = new Vector3[pins.Length];
        for (int i = 0; i < pins.Length; i++)
        {
            positions[i] = pins[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveBall();
        if (ball.transform.position.z > 1)
        {
            countPinsDown();
            if (check_count == true)
            {
                check_count = false;
                turnCounter++;
            }
            bool next_level = LevelCompleted();
            if (next_level == true)
            {
                StartCoroutine(NextLevel(panelObject));
            }
            else
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    if (turnCounter == 1)
                    {
                        RemovePins();
                        ResetBall();
                    }
                    else if (turnCounter == 2)
                    {
                        turnCounter = 0;
                        score = 0;
                        UpdateScoreText();
                        ResetPins();
                        ResetBall();
                    }
                    check_count = true;
                }
            }
        }
    }

    void MoveBall()
    {
        Vector3 position = ball.transform.position;
        position += Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime;
        // limit the horizontal movement
        position.x = Mathf.Clamp(position.x, -0.4f, 0.4f);
        ball.transform.position = position;
    }

    void countPinsDown()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            Hit hit = pins[i].GetComponent<Hit>();
            if (pins[i].transform.eulerAngles.z > 5 && pins[i].transform.eulerAngles.z < 355 && hit.GetHit() == false)
            {
                score++;
                hit.SetHit(true);
            }
        }
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreUI.text = "Score: " + score.ToString();
    }

    void ResetBall()
    {
        ball.transform.position = new Vector3(0, 0.05400001f, -7);
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.transform.rotation = Quaternion.identity;
    }

    void RemovePins()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            Hit hit = pins[i].GetComponent<Hit>();
            if (hit.GetHit() == true)
            {
                pins[i].SetActive(false);
            }
        }
    }

    void ResetPins()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            pins[i].SetActive(true);
            Hit hit = pins[i].GetComponent<Hit>();
            hit.SetHit(false);
            pins[i].transform.position = positions[i];
            pins[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            pins[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            pins[i].transform.rotation = Quaternion.identity;
        }
    }

    bool LevelCompleted()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            Hit hit = pins[i].GetComponent<Hit>();
            if (hit.GetHit() == false)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator NextLevel(GameObject panelObject)
    {
        //co-routines
        panelObject.SetActive(true);
        for (float i = 2; i > 0; i--)
        {
            Debug.Log("Level completed!");
            yield return new WaitForSeconds(1);       // co-routines
        }
        SceneManager.LoadScene(build_index);
    }
}
