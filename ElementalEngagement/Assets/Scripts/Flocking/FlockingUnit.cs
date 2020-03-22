using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based of Sebastian Lague's Boids project: github.com/SebLague/Boids

public class FlockingUnit : Unit {
    public enum SteeringBehaviour {
        SEEKING,
        ARRIVAL,
        WANDER,
        FOLLOWER
    }
    // Start is called before the first frame update
    public float maxSpeed = 2.0f;

    [Header("Leader & Follower Behaviour")]
    public bool isLeader = false;
    public GameObject leader;
    public float followDistance = 2.0f;

    [Header("Perception")]
    public float sightRadius = 1.0f;
    public float sightAngle = 130f;

    [Header("Forces")]
    public float maxForce = 3;
    public float alignWeight = 1;
    public float cohesionWeight = 1;
    public float seperateWeight = 1;
    public float steeringWeight = 2;

    [Header("Seperation")]
    public float seperationRadius = 0.5f;

    [Header("Behaviour")]
    public SteeringBehaviour currentBehaviour = SteeringBehaviour.SEEKING;

    [Header("DEBUG")]
    public bool seeking = false;
    public Vector3 targetPos;



    List<GameObject> units = new List<GameObject>();
    Rigidbody rb;
    System.Random rnd = new System.Random();

    void Start()
    {
        units.AddRange(GameObject.FindGameObjectsWithTag("Flock"));
        transform.Find("HealthBarWidget").gameObject.SetActive(false);
        rb = gameObject.GetComponent<Rigidbody>();
        if (!isLeader)
            currentBehaviour = SteeringBehaviour.FOLLOWER;
    }

    // Update is called once per frame
    void Update()
    {

        int numberOfVisibleNeighbours = 0;
        int numberOfNeighboursToAvoid = 0;
        Vector3 seperationDirection = Vector3.zero;
        Vector3 averageForward = Vector3.zero;
        Vector3 centerOfGroup = Vector3.zero;
        Vector3 finalForce = Vector3.zero;

        Vector3 steeringForce = Vector3.zero;

        // The leader wanders, or seeks if a point is selected
        // Followers use the Arrive behaviour, arriving at a point behind the leader

        // At every step, calculate forces
        // 1. Force towards target position - steering behaviour
        // 2. Seperation force - move away from other units
        // 3. Cohesion force - point in the average direction
        // 4. Alignment force - move towards center of group


        switch (currentBehaviour)
        {
            case SteeringBehaviour.SEEKING:
                steeringForce = Vector3.Normalize(targetPos - transform.position);
                if (Vector3.Distance(transform.position, targetPos) < seperationRadius * 10)
                {
                    currentBehaviour = SteeringBehaviour.ARRIVAL;
                }
                break;
            case SteeringBehaviour.WANDER:
                int wander = rnd.Next(1, 4);
                switch (wander)
                {
                    case 1:
                        targetPos = transform.position + 20 * transform.forward;
                        break;
                    case 2:
                        targetPos = transform.position + 20 * transform.right;
                        break;
                    case 3:
                        targetPos = transform.position + 20 * -transform.right;
                        break;
                    case 4:
                        targetPos = transform.position + 20 * -transform.forward;
                        break;
                    default:
                        targetPos = transform.position;
                        break;
                }
                currentBehaviour = SteeringBehaviour.SEEKING;
                break;
            case SteeringBehaviour.ARRIVAL:
                int rng = rnd.Next(1, 100);
                Debug.Log("RNG: " + rng);
                if (rng < 5)
                {
                    currentBehaviour = SteeringBehaviour.WANDER;
                }
                else
                {
                    steeringForce = Vector3.zero;
                }
                break;
            case SteeringBehaviour.FOLLOWER:
            default:
                targetPos = leader.transform.position - followDistance * leader.transform.forward;
                Vector3 offset = targetPos - transform.position;
                float distance = Vector3.Magnitude(offset);
                steeringForce = offset.normalized * (distance / 2);
                break;
        }
        /*if (isLeader)
        {
            if (seeking)
            {
                //seek
                steeringForce = targetPos - transform.position;
            }
            else
            {
                //wander
                steeringForce = transform.forward;
            }
        }

        else
        {
            // arrive
            targetPos = leader.transform.position - followDistance * leader.transform.forward;
            Vector3 offset = targetPos - transform.position;
            float distance = Vector3.Magnitude(offset);
            steeringForce = offset.normalized * (distance / 2);
        }*/

        // Look at nearby units - calculate important variables
        foreach (GameObject neighbour in units)
        {
            if (Vector3.Distance(neighbour.transform.position, transform.position) < sightRadius
            && Vector3.Angle(transform.forward, (neighbour.transform.position - transform.position)) < sightAngle)
            {
                numberOfVisibleNeighbours += 1;
                averageForward += neighbour.transform.forward;
                centerOfGroup += neighbour.transform.position;

                if (Vector3.Distance(neighbour.transform.position, transform.position) < seperationRadius)
                {
                    seperationDirection -= (neighbour.transform.position - transform.position).normalized;
                    numberOfNeighboursToAvoid += 1;
                }
            }
        }

        // Calculate forces
        if (numberOfVisibleNeighbours > 0)
        {

            averageForward /= numberOfVisibleNeighbours;
            centerOfGroup /= numberOfVisibleNeighbours;
            seperationDirection = seperationDirection.normalized;


            Vector3 alignForce = SteerTowards(averageForward) * alignWeight;
            Vector3 cohesionForce = SteerTowards(centerOfGroup - transform.position) * cohesionWeight;
            Vector3 seperationForce = SteerTowards(seperationDirection) * seperateWeight;
            finalForce = finalForce + alignForce + cohesionForce + seperationForce;
        }

        rb.AddForce(finalForce + steeringForce);


        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.AddForce(-rb.velocity * Mathf.Pow((rb.velocity.magnitude - maxSpeed), 2));
        }

        transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);

    }

    // These target handlers are inherited from entity
    override public void targetEntity(GameObject target)
    {
    }

    override public void targetPosition(Vector3 point)
    {
        if (isLeader)
        {
            setSeekPosition(point);
        }
    }

    void setSeekPosition(Vector3 point)
    {
        currentBehaviour = SteeringBehaviour.FOLLOWER;
        targetPos = point;
    }

    Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 v = vector.normalized * maxSpeed;
        return Vector3.ClampMagnitude(v, maxForce);
    }

}
