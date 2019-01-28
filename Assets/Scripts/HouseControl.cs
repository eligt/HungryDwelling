using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HouseState
{
    NEUTRAL,
    CHASING,
    EATING,
    PLACED
}

public class HouseControl : MonoBehaviour {
    public int fullCount = 4;
    public Sprite HouseEmpty;
    public Sprite HouseFull;
    public Sprite ArmsNeutral;
    public Sprite ArmsExtended;
    public Sprite ArmsEat;

    [HideInInspector]
    public int peopleCount = 0;
    [HideInInspector]
    private HouseState _state;
    [HideInInspector]
    public BulldozerControlNew Bulldozer = null;
    private float eatingStart = 0.0f;

    public bool isFull
    {
        get { return peopleCount >= fullCount; }
    }

    public bool isPlaced
    {
        get { return state == HouseState.PLACED; }
        set { state = HouseState.PLACED; }
    }

    public HouseState state
    {
        get { return _state; }
        set
        {
            if (_state != value)
            {
                HouseState oldState = _state;

                _state = value;

                UpdateState(oldState);
            }
        }
    }

    public Bounds GetBounds()
    {
        SpriteRenderer houseRender = transform.Find("HouseRender").gameObject.GetComponent<SpriteRenderer>();
        return houseRender.bounds;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        GameObject counter = transform.Find("CountText").gameObject;

        //if (counter != null)
        {
            MeshRenderer textRender = counter.GetComponent<MeshRenderer>();
            TextMesh text = counter.GetComponent<TextMesh>();

            if (isPlaced)
            {
                textRender.enabled = false;
            }
            else
            {
                textRender.enabled = true;
                text.text = peopleCount.ToString();
            }
        }

        SpriteRenderer houseRender = transform.Find("HouseRender").gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer armsRender = transform.Find("ArmsRender").gameObject.GetComponent<SpriteRenderer>();

        if (isFull)
        {
            houseRender.sprite = HouseFull;
        }
        else
        {
            houseRender.sprite = HouseEmpty;
        }

        armsRender.enabled = true;
        float eatingDiff = Time.time - eatingStart;
        bool playingEatingAnim = eatingDiff < 1.5f;

        if (eatingStart > 0.0f && playingEatingAnim && state != HouseState.PLACED)
        {
            int animStep = (int)(eatingDiff * 10.0f) % 2;

            if (animStep == 0)
                armsRender.sprite = ArmsExtended;
            else
                armsRender.sprite = ArmsEat;
        }
        else
        {
            switch (state)
            {
                case HouseState.PLACED:
                    {
                        armsRender.enabled = false;
                        break;
                    }
                case HouseState.NEUTRAL:
                    {
                        armsRender.sprite = ArmsNeutral;
                        break;
                    }
                case HouseState.CHASING:
                    {
                        armsRender.sprite = ArmsExtended;
                        break;
                    }
                case HouseState.EATING:
                    {
                        armsRender.sprite = ArmsEat;
                        break;
                    }
            }
        }
    }

    void UpdateState(HouseState oldState)
    {
        //Debug.Log("UpdateState: " + oldState.ToString() + " " + _state.ToString());

        if (oldState == HouseState.CHASING && _state == HouseState.EATING)
        {
            eatingStart = Time.time;
        }
    }
}
