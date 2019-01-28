using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulldozerControlNew : MonoBehaviour {


    //    SET-UP VARIABLES   //
    GameObject bulldozer;
    public Transform houseParent;


    //    HOUSE SELECTION   //
    bool houseIsSelected;
    HouseControl selectedHouse;
    float smallestDistance = 1000;

    //    ATTACK HOUSE   //
    float attackRadius = 2;

    //     LISTS    //
    HouseControl[] activeHouseList;
    List<float> distanceList = new List<float>();

    //   TRAVEL SPEED   //
    float walkSpeed = 5;

    //   ROTATION   //
    float strength = 0.8f;
    public float rotationSpeed = 100000;
    private Vector3 targetDirection;
    private Quaternion targetRotation;

    //   PAUSE   //
    float pauseTime = 3.0f;
    bool pauseTaken;
    bool doingPause = false;
    float pauseAmount = 0.0f;
    bool isMoving = false;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (selectedHouse == null)
        {
            // scan for the closest house and then target it:
            GetActiveHouses();
            SelectClosestHouse();
        }

        if (doingPause)
        {
            pauseAmount += Time.deltaTime;

            if (pauseAmount >= pauseTime)
            {
                pauseAmount = 0.0f;
                doingPause = false;
            }

            if (doingPause)
                return;
        }

        if (selectedHouse != null)
        {
            Transform target = selectedHouse.transform;
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            Vector3 targetDir = targetPosition - transform.position;

            // The step size is equal to speed times frame time.
            float step = walkSpeed * Time.deltaTime;
            float rotationZ = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            
            if (!isMoving)
            {
                float angleDiff = Mathf.Abs(transform.rotation.eulerAngles.z - rotationZ);
                float modulus = angleDiff % 360;

                if (modulus > 5.0f && modulus < 355.0f)
                {
                    //Debug.Log("Update Mathf.Abs" + (transform.rotation.eulerAngles.z - rotationZ).ToString());
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, rotationZ), step);
                }
                else
                {
                    pauseAmount = pauseTime * 0.5f;
                    doingPause = true;
                    isMoving = true;
                }
            }
            else
            {
                if (Vector3.Distance(targetPosition, transform.position) > attackRadius)
                {
                    // move towards the nearest house
                    transform.position = Vector3.MoveTowards(transform.position, selectedHouse.transform.position, step);
                }
                else
                {
                    HousePlacer.Instance.DestroyHouse(selectedHouse);

                    selectedHouse = null;
                    pauseAmount = 0.0f;
                    doingPause = true;
                    isMoving = false;
                }
            }
        }
    }

    void GetActiveHouses()
    {
        activeHouseList = houseParent.GetComponentsInChildren<HouseControl>();
    }

    void SelectClosestHouse()
    {
        // Reset smallestDistance to ensure values from previous scans don't carry through to the next
        smallestDistance = 5000;

        foreach (HouseControl house in activeHouseList)
        {
            if (house.Bulldozer != null)
                continue;

            Transform trans = house.transform;
            Vector3 thisPos = transform.position;
            Vector3 otherPos = new Vector3(trans.position.x, trans.position.y, thisPos.z);

            //Calculate the distance form the bulldozer to the house in question
            float distance = Vector3.Distance(trans.position, transform.position);

            // If the current distance is less than the smallest distance recorded so far, overwrite and select that house
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                selectedHouse = house;
            }
        }

        Debug.Log("SelectClosestHouse " + transform.name);

        if (selectedHouse != null)
        {
            Debug.Log("SelectClosestHouse2 " + selectedHouse.transform.name);
            selectedHouse.Bulldozer = this;
        }
    }
    
    private void FixedUpdateOldie()
    {

        // Check if there are any standing houses
        if (selectedHouse != null)
        {
            // Do the following as long as the bulldozer is NOT facing the selected house (within 5 degrees )
            if (Vector3.Angle(transform.position, selectedHouse.transform.position) > 5.0)
            {
                Quaternion rot = transform.rotation;
                Vector3 eulerAngles = rot.eulerAngles;

                // Turn the right into a transform and find the direction towards the next house:
                Vector3 right = transform.TransformDirection(Vector3.right);
                Vector3 direction = selectedHouse.transform.position - transform.position;

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
                transform.position = Vector3.MoveTowards(transform.position, selectedHouse.transform.position, Time.deltaTime * walkSpeed);

            }

            // If within a given radius, stop moving and attack the house
            if (Vector3.Distance(selectedHouse.transform.position, transform.position) < attackRadius)
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
