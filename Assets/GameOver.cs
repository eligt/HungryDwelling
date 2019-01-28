using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour {

    float delay = 0.0f;
    float startTime = 0.0f;

    GameObject Screen;

	// Use this for initialization
	void Start ()
    {
        BulldozerSpawner spawner = BulldozerSpawner.Instance;
        delay = spawner.spawnInterval;
        startTime = 0.0f;

        Screen = transform.Find("Screen").gameObject;
        Screen.SetActive(false);
    }

    private void Awake()
    {
        BulldozerSpawner spawner = BulldozerSpawner.Instance;
        delay = spawner.spawnInterval;
        startTime = 0.0f;

        Screen = transform.Find("Screen").gameObject;
        Screen.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        startTime += Time.deltaTime;

		if (startTime > delay)
        {
            HousePlacer placer = HousePlacer.Instance;

            if (placer.GetPlacedCount() == 0)
            {
                PlayerControl player = GameObject.FindObjectsOfType(typeof(PlayerControl))[0] as PlayerControl;
                Screen.SetActive(true);
                Screen.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = player.totalEaten.ToString();
            }
        }
	}
}
