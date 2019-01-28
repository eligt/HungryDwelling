using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        PlayerControl player = GameObject.FindObjectsOfType(typeof(PlayerControl))[0] as PlayerControl;
        Transform score = transform.Find("ScoreText");

        score.GetComponent<Text>().text = player.totalEaten.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
