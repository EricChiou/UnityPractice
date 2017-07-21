using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonMotion : MonoBehaviour
{

    public float attackRange = 0.3f;
    public float attackAngle = 60f;
    public float attackPower = 2f;
    public float skillCD = 10f;

    private NavMeshAgent agent;
    private MonsterHealth health;
    private GameObject playerHealth;
    private GameObject player;
    private Animator anim;
    private AnimatorStateInfo currentState;
    private Rigidbody rig;
    private Collider col;
    private float totalHealth;
    private float skillTime;
    private bool isDead = false;
    private bool isBackfall = false;
    private bool inBackfall = false;
    private bool inAttack = false;
    private bool inSkill = false;
    private Vector3 skillDest;

    static int knockBackState = Animator.StringToHash("Base Layer.KnockBack");
    static int attackState = Animator.StringToHash("Base Layer.Attack");
    static int runState = Animator.StringToHash("Base Layer.Run");

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
        skillTime = Time.time;
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
        if (inAttack || currentState.fullPathHash == attackState)
        {
            if (currentState.fullPathHash == attackState)
            {
                inAttack = false;
                anim.SetBool("Attack", false);
            }
            return;
        }

        if (inBackfall || currentState.fullPathHash == knockBackState)
        {
            if (currentState.fullPathHash == knockBackState)
            {
                inBackfall = false;
            }
            return;
        }

        if (inSkill || currentState.fullPathHash == runState)
        {
            if (currentState.fullPathHash == runState)
            {
                inSkill = false;
            }
            return;
        }

        if (!isBackfall && health.getHealth() < health.health * 0.5f)
        {
            anim.SetTrigger("KnockBack");
            isBackfall = true;
            inBackfall = true;
            rig.useGravity = false;
            rig.velocity = Vector3.zero;
            agent.enabled = false;
            return;
        }

        agent.enabled = true;
        rig.useGravity = true;
        if ((Time.time - skillTime) > skillCD)
        {
            inSkill = true;
            skillDest = player.transform.position;
            anim.SetBool("Skill", true);
            StartCoroutine(skill());
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

    IEnumerator skill()
    {
        yield return new WaitForSeconds(1f);
        while (currentState.fullPathHash != runState) { yield return null; }
        float startTime = Time.time;
        while ((Time.time - startTime) < 3f)
        {
            float step = 1.6f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, skillDest, step);
            if (Vector3.Distance(transform.position, skillDest) < 1f) { break; }
            yield return null;
        }
        skillTime = Time.time;
        anim.SetBool("Skill", false);
    }

    IEnumerator dead()
    {
        yield return new WaitForSeconds(10f);
        Destroy(transform.gameObject);
    }
}
