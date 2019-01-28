using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonControl : MonoBehaviour {

    Vector3 origPosition;
    float animAmount;
    float animTime = 0.1f;

	// Use this for initialization
	void Start ()
    {
        origPosition = transform.position;
        animAmount = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        animAmount += Time.deltaTime;
        float interval = animTime;// + Random.Range(-0.1f, +0.1f);

        if (animAmount >= interval)
        {
            animAmount = 0.0f;

            transform.position = new Vector3(
                origPosition.x + Random.Range(-0.08f, 0.08f),
                origPosition.y + Random.Range(-0.05f, 0.05f),
                origPosition.z
            );
        }
    }
}
