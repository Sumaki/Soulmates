using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    Rigidbody p1_rb;
    Rigidbody p2_rb;


    public float playerSpeed;
    public float tugSpeed;
    public float tugTimeLengthToRelease;
    public float ropeDistance;

    float p1_horizontal;
    float p1_vertical;
    float p2_horizontal;
    float p2_vertical;
    Vector3 player1Input;
    Vector3 player2Input;

    Vector3 midpoint;
    Vector3 tempMidpoint;
    float distanceBetween;
    float pull_threshold;
    bool tug = false;
    bool movingToTarget = false;

    public GameObject p1;
    public GameObject p2;
    public GameObject respawnPoint;

    Vector3 startPosition;

    void Start()
    {
        p1_rb = p1.GetComponent<Rigidbody>();
        p2_rb = p2.GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        Player1_Inputs();
        Player2_Inputs();

        CalculateMidPoint();
        CalculatePullThreshold();
        //Debug.Log("The midpoint of the players are: " + midpoint);
        //Debug.Log("The distance between the two players are: " + distanceBetween);
        Debug.Log("Pull threshold amount: " + pull_threshold);
        Debug.Log("Are we tugging: " + tug);
        //Debug.Log("Player1 movement: " + player1Input);
        //Debug.Log("Player2 movement: " + player2Input);
        //Debug.Log("Moving to midpoint: " + movingToTarget);
        //Debug.Log("Player1 position: " + p1.transform.position);
        //Debug.Log("Midpoint: " + tempMidpoint);
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

        if (distanceBetween <= ropeDistance && player1Input != Vector3.zero)
            p1_rb.MovePosition(p1.transform.position + player1Input * Time.deltaTime * playerSpeed);
        else if (distanceBetween > ropeDistance)
        {
            Vector3 revertPosition = midpoint - p1.transform.position;
            revertPosition = revertPosition.normalized;
            revertPosition *= (distanceBetween - ropeDistance);
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

        if (distanceBetween <= ropeDistance && player2Input != Vector3.zero)
            p2_rb.MovePosition(p2.transform.position + player2Input * Time.deltaTime * playerSpeed);
        else if (distanceBetween > ropeDistance)
        {
            Vector3 revertPosition = midpoint - p2.transform.position;
            revertPosition = revertPosition.normalized;
            revertPosition *= (distanceBetween - ropeDistance);
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
        if (   (player1Input != Vector3.zero && player2Input != Vector3.zero) && distanceBetween >= ropeDistance - 0.5f)
        {

            if (pull_threshold >= 0 && pull_threshold <= tugTimeLengthToRelease && !tug)
            {
                Debug.Log("Force pulling activated!");
                pull_threshold += 2f * Time.deltaTime;
            }
        }

        if (pull_threshold >= tugTimeLengthToRelease)
        {
            tug = true;
            CheckMidPoint();
        }

        if (tug && player1Input == Vector3.zero && player2Input == Vector3.zero)
        {
            float currentLeapTime = 0f;
            Vector3 midpointTarget = tempMidpoint;
            pull_threshold = 0.0f;

            if (p1.transform.position != midpointTarget && p2.transform.position != midpointTarget && tug)
            {
                currentLeapTime += Time.deltaTime * tugSpeed; // tugSpeed can be increased overtime from 0 - tugSpeed the longer the player holds the tug
                if (currentLeapTime > 1)
                {
                    currentLeapTime = 1;
                    tug = false;                    
                }
                movingToTarget = true;
                float percentComplete = currentLeapTime / 1;
                p1.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(p1.transform.position, midpointTarget, percentComplete));
                p2.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(p2.transform.position, midpointTarget, percentComplete));
            }
            else if (p1.transform.position == midpointTarget && p2.transform.position == midpointTarget)
            {
                tug = false;                
            }
        }
    }

    void CheckMidPoint()
    {
        tempMidpoint.x = p1.transform.position.x + (p2.transform.position.x - p1.transform.position.x) / 2;
        tempMidpoint.y = p1.transform.position.y + (p2.transform.position.y - p1.transform.position.y) / 2;
        tempMidpoint.z = p1.transform.position.z + (p2.transform.position.z - p1.transform.position.z) / 2;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Environment")
        {
            // we stop moving into the midpoint (maybe)
            if (tug)
            {
                tug = false;
                pull_threshold = 0.0f;
            }
        }

        if (collision.gameObject.tag == "Breakable")
        {
            // we break the object ONLY if the tug is happening
            //Debug.Log("Destroyed object: " + collision.gameObject);
            if (movingToTarget)
            {
                tug = false;
                movingToTarget = false;
                pull_threshold = 0.0f;
                SetGameObjectVariables(collision.gameObject); 
               
            }
        }

        if (collision.gameObject.tag == "Respawn")
        {
            p1.transform.position = respawnPoint.transform.position;
            p2.transform.position = respawnPoint.transform.position;
        }


    }

    void SetGameObjectVariables(GameObject obj)
    {
        obj.GetComponent<BoxCollider>().enabled = false;
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<MeshRenderer>().enabled = false;
        obj.GetComponent<PlayObjectParticle>().playParticle = true;
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
