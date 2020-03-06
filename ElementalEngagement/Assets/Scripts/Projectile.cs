using UnityEngine;
public class Projectile : MonoBehaviour {
    public int damage = 5;
    public float speed = 1.0f;
    public Vector3 direction;
    private GameObject parent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + direction.normalized * speed;
    }

    public void SetDirection(Vector3 V)
    {
        direction = V;
    }

    public void SetDamage(int damage_amount)
    {
        damage = damage_amount;
    }

    public void SetGO(GameObject GO)
    {
        parent = GO;
    }

    void OnCollisionEnter(Collision C)
    {
        if (!C.gameObject.Equals(parent))
        {
            Unit U = C.gameObject.GetComponent<Unit>();
            U.Damage(damage);
            Destroy(gameObject);
        }
    }
}

