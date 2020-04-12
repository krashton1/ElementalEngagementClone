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
    public int attack_damage = 10;

    public int attack_frame_counter = 30;
    public int current_attack_frame;
    public float senseRange = 15.0f;

	private Animator anim;
    public Unit unit;

    void Start()
    {
        current_attack_frame = 0;
		anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!target || target.GetComponent<Entity>().MarkedForDeletion)
        {
            lookForTarget();
        }
        else {
            AttackEnemy();
        }
    }

    public void lookForTarget(){
    // Find the nearest structure
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, senseRange, LayerMask.GetMask("Enemy"));

        if (hitColliders.Length > 0){
            target = hitColliders[Random.Range(0, hitColliders.Length)].gameObject;
            unit.targetEntity(target);
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
                anim.SetTrigger("Attack1Trigger");
                if (type == AttackType.Ranged) RangedAttack(dist);
                else if (type == AttackType.Melee) MeleeAttack();
            }
            else{
                anim.ResetTrigger("Attack1Trigger");
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
        target.GetComponent<Entity>().Damage(attack_damage);
    }
}
