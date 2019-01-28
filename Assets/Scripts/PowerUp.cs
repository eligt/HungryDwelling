using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    //variables
    public bool powerUpActive = false;

    public GameObject bulldozer;
    public GameObject powerUpEffect;

    void OnTriggerEnter(Collider other)
    {
        //if the player makes contact with the powerup it runs the powerup function
        if (other.gameObject.name == "Player")
        {
            Powerup(other);
        }
    }

    void Powerup(Collider player)
    {
        //puts the effect on the player
        Instantiate(powerUpEffect, transform.position, transform.rotation);

        //the effect of the powerup goes here
        GetComponent<BulldozerControl>();

        //gets rid of the powerup on when made contact with
        Destroy(gameObject);
    }
}
