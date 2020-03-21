using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class MoveablePiece : Unit {
    // Start is called before the first frame update
    public float speed = 1.0f;
    public float attack_range;
    public float stop_range;
    private NavMeshAgent nav;
    public GameObject target;
    public GameObject projectile;
    public int attack_frame_counter = 30;
    public int current_attack_frame;
    private float mElapsedTime = 0.0f;

    // Structure for Hermite Curve
    // Since we don't do pathfinding, all our paths have only two points - start and end
    // Thus we don't use Catmull-Rom
    struct HermiteSpline
    {
        // Points, tangents, and segment lengths that form the piecewise spline
        public Vector3 pointA;
        public Vector3 pointB;
        public Vector3 tangentA;
        public Vector3 tangentB; 
        public float length; // length[i] is the length of curve segments from point 0 up to point 'i'
    }

    HermiteSpline spline = new HermiteSpline();
    public AnimationCurve easingCurve;
    public GameObject testSpherePrefab;
    GameObject testSphere;
    float timeToTravel = 0;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        current_attack_frame = 0;
        healthCurrent = healthCap;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            TargetEnemy();
            AttackEnemy();
        }

        // Time since last update, this is only here so that health can be modified over time
        mElapsedTime += Time.deltaTime;
        if (mElapsedTime >= 0.1f)
        {
            mElapsedTime = mElapsedTime % 0.1f;

            if (healthCurrent < healthCap)
            {
                healthCurrent++;
            }
        }

        // If a health bar has been attached, enable it if we have selected this piece, make it follow the piece, and update its value to the piece's current health.
        if (hpBarUi != null)
        {
            if (selected)
            {
                hpBarUi.gameObject.SetActive(true);
                hpBarUi.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2, 0));
                hpBarUi.value = ((float)(healthCurrent) / (float)(healthCap));
                //Debug.Log(hpBarUi.value);
            }
            else
            {
                hpBarUi.gameObject.SetActive(false);
            }

        }
    }

    // These target handlers are inherited from entity
    override public void targetEntity(GameObject target){
        //SetTarget(target);
    }

    override public void targetPosition(Vector3 point){
        //SetFuturePosition(point);
        CreatePathToPoint(point);
    }


    public void SetFuturePosition(Vector3 V)
    {
        nav.destination = V;
        target = null;
    }

    public void SetTarget(GameObject GO)
    {
        target = GO;
    }

    private void TargetEnemy()
    {
        float dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist > stop_range)
        {
            nav.destination = target.transform.position;
        }
        else
        {
            nav.destination = transform.position;
        }
    }

    private void AttackEnemy()
    {
        if (current_attack_frame++ > attack_frame_counter)
        {
            Vector3 dist = target.transform.position - transform.position;
            if (dist.magnitude < attack_range)
            {
                Projectile MP = Instantiate(projectile, transform.position + (dist.normalized * 2 * GetComponent<CapsuleCollider>().radius), new Quaternion()).GetComponent<Projectile>();
                MP.SetDirection(dist);
                MP.SetDamage(25);
                MP.SetGO(gameObject);
            }
            current_attack_frame = 0;
        }
    }

// ==============================================================================================================
// ||                                            Spline Movement                                               ||
// ============================================================================================================== 

    // Create a Hermite Curve from the current position to a given point
    void CreatePathToPoint(Vector3 p){

        StopAllCoroutines();
        Destroy(testSphere);

        testSphere = GameObject.Instantiate(testSpherePrefab, p, Quaternion.identity);

        spline.pointA = gameObject.transform.position;
        spline.pointB = p;
     
        spline.tangentA = gameObject.transform.rotation * Vector3.forward;
        spline.tangentB = (p - gameObject.transform.position).normalized;

        spline.length = Vector3.Distance(spline.pointA, spline.pointB);

        timeToTravel = spline.length / speed;

        float tangentMultiplier = Mathf.Clamp( timeToTravel * 2 ,1 ,20); // we multiply the tangent to make the curve more noticeable
        spline.tangentA *= tangentMultiplier;
        // Start animation
        StartCoroutine("Move");
    }

     // Get position along Hermite Curve using parameter t = [0, 1]
    Vector3 HermiteSplineInterp(HermiteSpline spline, float t)
    {
        float t2 = Mathf.Pow(t, 2);
        float t3 = Mathf.Pow(t, 3);
        Vector3 pos = (2 * t3 - 3 * t2 + 1) * spline.pointA +
                      (t3 - 2 * t2 + t) * spline.tangentA +
                      (-2 * t3 + 3 * t2) * spline.pointB +
                      (t3 - t2) * spline.tangentB;
        return pos;
    }

    IEnumerator Move()
    {
        // Move along the path by interpolating from 0 -> 1
        // Update position 20 times per second
        for (float s = 0.0f; s < timeToTravel; s += 0.05f)
        {
            // Apply an easing function
            float t = Mathf.InverseLerp(0, timeToTravel, s);
            t = easingCurve.Evaluate(t);

            //Vector3 pos = Vector3.Lerp(spline.pointA, spline.pointB,t);   // simple lerp
            Vector3 pos = HermiteSplineInterp(spline, t);                   // hermite curve

            this.transform.position = new Vector3(pos.x, this.transform.position.y, pos.z);

            // Get orientation from tangent along the curve
            Vector3 curve_tan = HermiteSplineInterp(spline, t + 0.01f) - HermiteSplineInterp(spline, t);
            curve_tan.Normalize();
            // Check if we are close to the last point along the path
            if (t >= 0.99f){
                // The last point does not have a well-defined tangent, so use the one of the curve
                curve_tan = spline.tangentB;
            }
            // Create orientation from the tangent
            Quaternion orient = new Quaternion();
            orient.SetLookRotation(curve_tan, Vector3.up);
            // The following rotations are needed to set the proper initial orientation
            orient *= Quaternion.AngleAxis(180, Vector3.forward);
            orient *= Quaternion.AngleAxis(180, Vector3.right);
            orient *= Quaternion.AngleAxis(180, Vector3.up);
            // We only set the Y rotation. Units cannot rotate along other axis - they will jerk around if we try
            this.transform.eulerAngles = new Vector3(this.transform.rotation.eulerAngles.x, orient.eulerAngles.y, this.transform.eulerAngles.z);
            yield return new WaitForSeconds(0.05f);
        }
        this.transform.position = spline.pointB;
        yield return null;
    }

}



