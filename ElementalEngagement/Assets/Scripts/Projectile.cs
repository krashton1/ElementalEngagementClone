using UnityEngine;

public class Projectile : MonoBehaviour {

	
	public int damage = 5;
    public float speed = 0.5f;
    public Vector3 direction;
    private GameObject parent;
    float lifetime = 1.5f;

	public ElementComponent.ElementType element_type = ElementComponent.ElementType.None;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0){
            Destroy(gameObject);
        }
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

	public void SetElementType(ElementComponent.ElementType et)
	{
		element_type = et;
	}

	public void SetGO(GameObject GO)
    {
        parent = GO;
    }

    void OnCollisionEnter(Collision C)
    {
        if (C.gameObject.CompareTag("Enemy"))
        {
            Entity E = C.gameObject.GetComponent<Entity>();
            if (E) E.Damage(damage, element_type);
            Destroy(gameObject);
        }
    }
}

