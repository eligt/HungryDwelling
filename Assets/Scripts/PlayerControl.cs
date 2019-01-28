using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float speed = 3.0f;             //Floating point variable to store the player's movement speed.
    public GameObject HouseObject;
    
    private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
    private bool insideChaseArea = false;
    private bool insidePerson = false;

    private float updateInputInterval = 0.1f;
    private float inputUpdateTime = 0.0f;

    private int _totalEaten = 0;


    public int totalEaten
    {
        get { return _totalEaten; }
    }
    
    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();

        if (getCurrentHouse() == null)
            createNewHouse();
    }

    public HouseControl getCurrentHouse()
    {
        HouseControl[] houses = GetComponentsInChildren<HouseControl>();

        if (houses.Length > 0)
            return houses[0];

        return null;
    }

    void createNewHouse()
    {
        GameObject house = Instantiate(HouseObject, transform);
        house.transform.localPosition = new Vector3(0, 0, 0);
        house.SetActive(true);
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
    }

    void Update()
    {
        inputUpdateTime += Time.deltaTime;

        //if (inputUpdateTime >= updateInputInterval)
        {
            inputUpdateTime = 0.0f;

            //Store the current horizontal input in the float moveHorizontal.
            float moveHorizontal = Input.GetAxis("Horizontal");

            //Store the current vertical input in the float moveVertical.
            float moveVertical = Input.GetAxis("Vertical");

            //Use the two store floats to create a new Vector2 variable movement.
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);

            if (movement != Vector2.zero)
            {
                //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
                //rb2d.velocity = Vector2.Lerp(rb2d.velocity, movement * speed, 0.5f);
                rb2d.velocity = movement * speed;
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 mouseClick = Input.mousePosition;
                mouseClick.z = transform.position.z;
                Vector3 mouseGlobal = Camera.main.ScreenToWorldPoint(mouseClick);
                Vector2 toClick = (mouseGlobal - transform.position).normalized * speed;

                //rb2d.AddForce(new Vector2(mouseGlobal.x, mouseGlobal.y).normalized * 2.0f);
                rb2d.velocity = toClick;
            }
            else
            {
                rb2d.velocity = Vector2.zero;
            }

            Vector2 v = rb2d.velocity;

            if (v != Vector2.zero)
            {
                float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                //transform.rotation = Quaternion.Slerp(Quaternion.AngleAxis(angle, Vector3.forward), transform.rotation, 0.5f);
                transform.rotation = Quaternion.AngleAxis(angle % 360, Vector3.forward);

                //Debug.Log("FixedUpdate: " + movement.ToString());
                //Debug.Log(v);
                //Debug.Log(angle);
            }
        }

        HouseControl house = getCurrentHouse();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (house.isFull)
            {
                Vector2 position = new Vector2(transform.position.x, transform.position.y);
                
                if (HousePlacer.Instance.isInsideAvailablePlace(position))
                {
                    PopulationCounterUI.populationCount += 4;
                    HousePlacer.Instance.placeHouseAt(house, position);
                    _totalEaten += house.peopleCount;
                    createNewHouse();
                }
            }
        }
        
        if (house.state != HouseState.PLACED)
        {
            HouseState state = HouseState.NEUTRAL;

            if (insidePerson)
                state = HouseState.EATING;
            else if (insideChaseArea)
                state = HouseState.CHASING;

            house.state = state;
        }

        insidePerson = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PersonControl pc = other.gameObject.GetComponent<PersonControl>();

        if (pc != null)
        {
            HouseControl house = getCurrentHouse();

            if (!house.isFull)
            {
                if (other is CapsuleCollider2D)
                {
                    insidePerson = true;
                    
                    house.peopleCount++;

                    PersonSpawner.Instance.UnspawnPerson(pc);
                }
                else if (other is CircleCollider2D)
                {
                    insideChaseArea = true;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("OnTriggerExit2D: ");
        if (other is CapsuleCollider2D)
        {
            insidePerson = false;
        }
        else if (other is CircleCollider2D)
        {
            insidePerson = false;
            insideChaseArea = false;
        }
        //Debug.Log("OnTriggerExit2D 2: " + insidePerson.ToString() + " " + insideChaseArea.ToString());
    }
}
