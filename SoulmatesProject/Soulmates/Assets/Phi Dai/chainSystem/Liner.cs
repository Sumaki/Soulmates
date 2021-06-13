using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Liner : MonoBehaviour {

public GameObject OtherGameObject;
LineRenderer line;
Vector3 position;
Vector3 otherPosition;


public float RopeWidth=0.5f;
	void Start () {
		if(OtherGameObject == null)
		{
			Debug.LogWarning("Please Attach Other GameObject in inspector");
			return;
		}
		line = GetComponent<LineRenderer>();


		line.SetWidth(RopeWidth,RopeWidth);

		line.useWorldSpace = true;

		line.positionCount = 2;

        position = gameObject.transform.position;
        position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z);

        otherPosition = OtherGameObject.transform.position;
        otherPosition = new Vector3(OtherGameObject.transform.position.x, OtherGameObject.transform.position.y + 0.5f, OtherGameObject.transform.position.z);


        line.SetPosition(0,position);
		line.SetPosition(1,OtherGameObject.transform.position);
	}
	
	// Update is called once per frame
	void Update () {

        position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z);
        otherPosition = new Vector3(OtherGameObject.transform.position.x, OtherGameObject.transform.position.y + 0.5f, OtherGameObject.transform.position.z);
        line.SetPosition(0,position);
		line.SetPosition(1,otherPosition);
		
	}
}
