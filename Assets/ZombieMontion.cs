using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMontion : MonoBehaviour
{
    public float attackRange = 0.2f;
    public float attackAngle = 90f;
    public float attackPower = 1f;

    private NavMeshAgent agent;
    private MonsterHealth health;
    private GameObject playerHealth;
    private GameObject player;
    private Animator anim;
    private AnimatorStateInfo currentState;
    private Rigidbody rig;
    private Collider col;
    private float totalHealth;
    private bool isDead = false;
    private bool isBackfall = false;
    private bool inBackfall = false;
    private bool inAttack = false;

    static int backFallState = Animator.StringToHash("Base Layer.BackFall");
    static int attackState = Animator.StringToHash("Base Layer.Attack");

    // Use this for initialization
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Controller");
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        health = transform.GetComponent<MonsterHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) { return; }
        if (health.getHealth() <= 0)
        {
            anim.SetTrigger("Dead");
            health.setCanBeAttack(false);
            isDead = true;
            StartCoroutine(dead());
            rig.useGravity = false;
            rig.velocity = Vector3.zero;
            agent.enabled = false;
            GetComponentInChildren<CanvasController>().disableHealthBar();
            col.enabled = false;
            return;
        }

        currentState = anim.GetCurrentAnimatorStateInfo(0);
        if (inBackfall || currentState.fullPathHash == backFallState)
        {
            if (currentState.fullPathHash == backFallState)
            {
                inBackfall = false;
            }
            return;
        }
        health.setCanBeAttack(true);
        agent.enabled = true;
        rig.useGravity = true;

        if (!isBackfall && health.getHealth() < health.health * 0.5f)
        {
            anim.SetTrigger("BackFall");
            health.setCanBeAttack(false);
            isBackfall = true;
            inBackfall = true;
            rig.useGravity = false;
            rig.velocity = Vector3.zero;
            agent.enabled = false;
            return;
        }

        if (inAttack || currentState.fullPathHash == attackState)
        {
            if (currentState.fullPathHash == attackState)
            {
                anim.SetBool("Attack", false);
                inAttack = false;
            }
            return;
        }
        agent.SetDestination(player.transform.position);
        if (Vector3.Distance(player.transform.position, transform.position) < (1f + attackRange) && !inAttack)
        {
            transform.LookAt(player.transform);
            anim.SetBool("Attack", true);
            inAttack = true;
            judgeAttack();
        }
    }

    void judgeAttack()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < (1f + attackRange))
        {
            Vector3 relative = transform.InverseTransformPoint(player.transform.position);
            float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
            if (Mathf.Abs(angle) <= attackAngle * 0.5f)
            {
                playerHealth.transform.GetComponent<PlayerHealth>().beAttacked(attackPower);
            }
        }
    }

    IEnumerator dead()
    {
        yield return new WaitForSeconds(10f);
        Destroy(transform.gameObject);
    }
}
