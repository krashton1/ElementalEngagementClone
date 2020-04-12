using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{

    public enum AttackType {Ranged, Melee, Magic};

    public AttackType attack_type;
	public ElementComponent.ElementType element_type;
    public float attack_range;
    public GameObject target;
    public GameObject projectile;

	public int attack_frame_counter = 30;
    public int current_attack_frame;
	
	private Animator anim;

    void Start()
    {
        current_attack_frame = 0;
		anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            AttackEnemy();
        }
		else
		{
			SenseTarget();
		}
    }

    public float SetTarget(GameObject GO)
    {
        target = GO;
        return attack_range;
    }

    private void AttackEnemy()
    {
        if (current_attack_frame++ > attack_frame_counter)
        {
            Vector3 dist = target.transform.position - transform.position;
            if (dist.magnitude < attack_range)
            {
                if (attack_type == AttackType.Ranged) RangedAttack(dist);
                else if (attack_type == AttackType.Melee) MeleeAttack();
            }
            current_attack_frame = 0;
        }
    }

    private void RangedAttack(Vector3 dist){
        Projectile MP = Instantiate(projectile, transform.position + (dist.normalized * 2 * GetComponent<CapsuleCollider>().radius), new Quaternion()).GetComponent<Projectile>();
        MP.SetDirection(dist);
        MP.SetDamage(50);
		MP.SetElementType(element_type);
        MP.SetGO(gameObject);
    }

    void MeleeAttack(){

    }

	private void SenseTarget()
	{
		GameObject[] allEntities = GameObject.FindGameObjectsWithTag("Flock");
		foreach (GameObject GO in allEntities)
		{
			if (Vector3.Distance(transform.position, GO.transform.position) <= attack_range)
			{
				SetTarget(GO);
				return;
			}
		}
	}
}
