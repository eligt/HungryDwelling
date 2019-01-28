using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulldozerControl : MonoBehaviour {


    //    SET-UP VARIABLES   //
    GameObject bulldozer;
    public Transform houseParent;


    //    HOUSE SELECTION   //
    bool houseIsSelected;
    Transform selectedHouse;
    float smallestDistance = 1000;

    //    ATTACK HOUSE   //
    float attackRadius = 2;

    //     LISTS    //
    List<Transform> activeHouseList = new List<Transform>();
    List<float> distanceList = new List<float>();

    //   TRAVEL SPEED   //
    float walkSpeed = 5;

    //   ROTATION   //
    float strength = 0.8f;
    public float rotationSpeed = 100000;
    private Vector3 targetDirection;
    private Quaternion targetRotation;

    //   PAUSE   //
    float pauseTime = 8f;
    bool pauseTaken;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        // Check if there are any standing houses
        if (houseParent.childCount > 0)
        {
            // Constantly scan for the closest house and then target it:
            GetActiveHouses();
            SelectClosestHouse();
        }

    }

    void GetActiveHouses()
    {
        // Reset the list
        activeHouseList.Clear();

        foreach (Transform child in houseParent)
        {
            // Add the houses to the list. They are added using their Transform component. You can later retrieve the GameObject from this
            activeHouseList.Add(child);
        }
    }

    void SelectClosestHouse()
    {
        // Reset smallestDistance to ensure values from previous scans don't carry through to the next
        smallestDistance = 5000;

        foreach (Transform house in activeHouseList)
        {

            //Calculate the distance form the bulldozer to the house in question
            float distance = Vector3.Distance(house.position, transform.position);

            // If the current distance is less than the smallest distance recorded so far, overwrite and select that house
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                selectedHouse = house;                
                                
            }
 
        }
        
    }

    private void FixedUpdate()
    {

        // Check if there are any standing houses
        if (houseParent.childCount > 0)
        {


            // Do the following as long as the bulldozer is NOT facing the selected house (within 5 degrees )
            if (Vector3.Angle(transform.up, selectedHouse.position - transform.position) > 5)
            {

                Quaternion rot = transform.rotation;
                Vector3 eulerAngles = rot.eulerAngles;

                // Turn the right into a transform and find the direction towards the next house:
                Vector3 right = transform.TransformDirection(Vector3.right);
                Vector3 direction = selectedHouse.position - transform.position;

                // Determine whether the direction is to the left or right of the "right" transform
                if (Vector3.Dot(right, direction) < 0)
                {
                    // The selected house is on the RIGHT side of the bulldozer (line of symmetry down the centre), turn the bulldozer right
                    eulerAngles.z += Time.deltaTime * rotationSpeed;
                }
                else
                {
                    // The selected house is on the LEFT side of the bulldozer (line of symmetry down the centre), turn the bulldozer left
                    eulerAngles.z -= Time.deltaTime * rotationSpeed;
                }

                // Continue turning until within 5 degrees of the selected house, then set pauseTaken to true (therefore moving the bulldozer towards the selected house)
                rot.eulerAngles = eulerAngles;
                transform.rotation = rot;

            }
            else
            {
                pauseTaken = true;
            }

            // After a pause has occured, do the following:
            if (pauseTaken == true)
            {
                // Start travelling towards the nearest house
                transform.position = Vector3.MoveTowards(transform.position, selectedHouse.position, Time.deltaTime * walkSpeed);

            }

            // If within a given radius, stop moving and attack the house
            if (Vector3.Distance(selectedHouse.position, transform.position) < attackRadius)
            {
                pauseTaken = false;
                Invoke("Pause", pauseTime);
                Destroy(selectedHouse.gameObject);
            }


        }

    }


   void Pause()
   {
       pauseTaken = true;
   }

    
}
