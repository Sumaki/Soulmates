using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    Rigidbody rb;

    public bool player_1;
    public bool player_2;

    public float playerSpeed;
    public float tugSpeed;
    public float tugTimeLengthToRelease;

    float p1_horizontal;
    float p1_vertical;
    float p2_horizontal;
    float p2_vertical;
    Vector3 player1Input;
    Vector3 player2Input;

    Vector3 midpoint;
    float distanceBetween;
    float pull_threshold;
    bool tug = false;

    public GameObject p1;
    public GameObject p2;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(player_1)
            Player1_Inputs();
        if (player_2)
            Player2_Inputs();

        CalculateMidPoint();
        CalculatePullThreshold();
        //Debug.Log("The midpoint of the players are: " + midpoint);
        Debug.Log("The distance between the two players are: " + distanceBetween);
        Debug.Log("Pull threshold amount: " + pull_threshold);
        //Debug.Log("Are we tugging: " + tug);

        Debugs();
       
    }

    void Player1_Inputs()
    {
        p1_horizontal = Input.GetAxis("Horizontal_P1"); 
        p1_vertical = Input.GetAxis("Vertical_P1");
        player1Input = new Vector3(p1_horizontal, 0, p1_vertical);

        if (p1_horizontal == 0 && p1_vertical == 0)
        {
            player1Input = Vector3.zero;
            pull_threshold = 0.0f;
        }

        if(distanceBetween <= 15f && player1Input != Vector3.zero)
            rb.MovePosition(p1.transform.position + player1Input * Time.deltaTime * playerSpeed);
        else if (distanceBetween > 15f)
        {
            Vector3 revertPosition = midpoint - p1.transform.position;
            revertPosition = revertPosition.normalized;
            revertPosition *= (distanceBetween - 15);
            p1.transform.position += revertPosition;
        }
    }

    void Player2_Inputs()
    {
        p2_horizontal = Input.GetAxis("Horizontal_P2"); 
        p2_vertical = Input.GetAxis("Vertical_P2");
        player2Input = new Vector3(p2_horizontal, 0, p2_vertical);

        if (p2_horizontal == 0 && p2_vertical == 0)
        {
            player2Input = Vector3.zero;
            pull_threshold = 0.0f;
        }

        if (distanceBetween <= 15f && player2Input != Vector3.zero)
            rb.MovePosition(p2.transform.position + player2Input * Time.deltaTime * playerSpeed);
        else if (distanceBetween > 15f)
        {
            Vector3 revertPosition = midpoint - p2.transform.position;
            revertPosition = revertPosition.normalized;
            revertPosition *= (distanceBetween - 15);
            p2.transform.position += revertPosition;
        }
    }

    void CalculateMidPoint()
    {       
        midpoint.x = p1.transform.position.x + (p2.transform.position.x - p1.transform.position.x) / 2;
        midpoint.y = p1.transform.position.y + (p2.transform.position.y - p1.transform.position.y) / 2;
        midpoint.z = p1.transform.position.z + (p2.transform.position.z - p1.transform.position.z) / 2;

        // we want to know how to limit the player from going further than the distance listed
        distanceBetween = Vector3.Distance(p1.transform.position, p2.transform.position);
    }

    void CalculatePullThreshold()
    {
        if (    (p1_horizontal != 0 || p1_vertical != 0 && p2_horizontal != 0 || p2_vertical != 0 )     && distanceBetween >= 14f)
        {
            //Debug.Log("Force pulling activated!");
            if (pull_threshold >= 0 && pull_threshold <= tugTimeLengthToRelease)
                pull_threshold += 2f * Time.deltaTime;
        }

        if (pull_threshold >= tugTimeLengthToRelease)
            tug = true;

        if(tug && player1Input == Vector3.zero && player2Input == Vector3.zero)
        {
            float currentLeapTime = 0f;
            Vector3 midpointTarget = midpoint;

            if (p1.transform.position != midpointTarget && p2.transform.position != midpointTarget && tug)
            {
                currentLeapTime += Time.deltaTime * tugSpeed; // tugSpeed can be increased overtime from 0 - tugSpeed the longer the player holds the tug
                if (currentLeapTime > 1)
                {
                    currentLeapTime = 1;
                    tug = false;
                }
                float percentComplete = currentLeapTime / 1;
                p1.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(p1.transform.position, midpointTarget, percentComplete));
                p2.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(p2.transform.position, midpointTarget, percentComplete));
            }
            else if (p1.transform.position == midpointTarget && p2.transform.position == midpointTarget)
                tug = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Environment")
        {
            // we stop moving into the midpoint (maybe)
            if(tug)
                tug = false;
        }

        if(collision.gameObject.tag == "Breakable")
        {
            // we break the object ONLY if the tug is happening
            Debug.Log("Destroyed object: " + collision.gameObject);
            if (tug)
            {
                collision.gameObject.SetActive(false); // try something else if time 
                tug = false;
            }
        }
    }


    void Debugs()
    {
        //reload scene to test randomizer
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    
    }
}
