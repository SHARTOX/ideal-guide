using UnityEngine;
using UnityEngine.AI;

public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    public Animator EnemyAnim;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public float fixedRotation = 0f;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject enemyBullet;
    public int Damage = 8;
    public int hardcoreDamage = 7;
    public float HardcoreSight = 15f;
    public float HardcoreAttack = 10f;
    public float enemyRange = 20000f;
    public GameObject impactEffectEnemy;
    public AudioSource AttackSound;
    public float spread = 0.08f;
    public GameObject dirtEffect;
    public int HardcoreHealth = 50;
    public Target helth;

    //States
    public float sightRange, attackRange, listenRange;
    public bool playerInSightRange, playerInAttackRange, playerDidNoiseRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        AttackSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.eulerAngles = new Vector3(fixedRotation, transform.eulerAngles.y, fixedRotation);
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerDidNoiseRange = Physics.CheckSphere(transform.position, listenRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            EnemyAnim.SetBool("isWalking", true);
            EnemyAnim.SetBool("attacking", false);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        EnemyAnim.SetBool("isWalking", true);
        EnemyAnim.SetBool("attacking", false);
    }

    public void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        EnemyAnim.SetBool("attacking", true);
        EnemyAnim.SetBool("isWalking", false);

        if (!alreadyAttacked)
        {
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            Vector3 direction = enemyBullet.transform.forward + new Vector3(x, y, 0f);

            ///Attack code here
            RaycastHit hit;
            if(Physics.Raycast(enemyBullet.transform.position, direction, out hit, enemyRange))
            {
                CharacterMovement _char = hit.transform.GetComponent<CharacterMovement>();

                if(_char != null)
                {
                    _char.TheDamage(Damage);
                    Instantiate(impactEffectEnemy, hit.point, Quaternion.LookRotation(hit.normal));
                }
                if(hit.transform.tag == "floor")
                Instantiate(dirtEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            AttackSound.Play();

            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, listenRange);
    }

    public void usertoggle(bool tog)
    {
        if(tog == true)
        {
            Damage += hardcoreDamage;
            sightRange += HardcoreSight;
            attackRange += HardcoreAttack;
            helth.health += HardcoreHealth;
        }
        else
        if(tog == false)
        {
            Damage -= hardcoreDamage;
            sightRange -= HardcoreSight;
            attackRange -= HardcoreAttack;
            helth.health -= HardcoreHealth;
        }
    }

}
