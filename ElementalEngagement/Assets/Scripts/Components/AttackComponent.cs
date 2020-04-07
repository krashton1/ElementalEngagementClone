using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{

    public enum AttackType {Ranged, Melee, Magic};

    public AttackType type;
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
                if (type == AttackType.Ranged) RangedAttack(dist);
                else if (type == AttackType.Melee) MeleeAttack();
            }
            current_attack_frame = 0;
        }
    }

    private void RangedAttack(Vector3 dist){
        Projectile MP = Instantiate(projectile, transform.position + (dist.normalized * 2 * GetComponent<CapsuleCollider>().radius), new Quaternion()).GetComponent<Projectile>();
        MP.SetDirection(dist);
        MP.SetDamage(25);
        MP.SetGO(gameObject);
    }

    void MeleeAttack(){

    }
}
