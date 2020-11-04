using UnityEngine;
using System;

public class EnemyController : PhysicsObject
{
    //	Variables
    public float maxSpeed = 3;                          //	max movement speed
    public float jumpTakeOffSpeed = 15;                 //	jump force
    private bool attacking = false;                      //	atacking state booL
    private float horizontal;                           //	horizontal movememtn variable
    private bool facingRight;                           //  zombie facing direction
    [HideInInspector]
    public bool isAlive = true;

    //	target related
    private Transform target;                           //	reference to target to chase
    private float nextPlayerSearchTime = 0;             //	timer for for FindPlayer method, not to call it to often

    //	references 
    [HideInInspector] public Animator animator;         //	reference to animator
    private Transform zombie;                           //	reference to zombie position

    //	attack related
    [SerializeField] private float attackTime = 1;      //	attack time
    private float timeLeftforAttack;                    //	attack freeze timer

    //	obstacle colision related
    [SerializeField] private LayerMask whatIsGround;    //	obstacle colision check LayerMask
    private Transform obstacleCheck;                    //	reference to obstacle check point
    private bool hasObstacle;                           //	obstacle bool

    private Enemy enemy;

    void Awake()
    {   //  getting references
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
        zombie = GetComponent<Transform>();
        timeLeftforAttack = attackTime;
        //	finding obstacle check point
        obstacleCheck = transform.Find("ObstacleCheck");
        if (obstacleCheck == null)
        {
            Debug.LogError("No Zombie ObstacleCheck point!!!");
        }

        FindPlayer();
    }

    //	searching player
    void FindPlayer()
    {
        if (nextPlayerSearchTime <= Time.time)
        {
            GameObject searchRezult = GameObject.FindGameObjectWithTag("Player");
            if (searchRezult != null)
            {
                target = searchRezult.transform;
                nextPlayerSearchTime = Time.time + 0.5f;
            }
        }
    }

    //	movement
    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;                                //	reset movement vector at frame start
        horizontal = 0;

        if (isAlive)    //  do movement only if enemt is alive
        {
            if (target == null)     //  if no player to chase
            {
                animator.SetFloat("speed", Mathf.Abs(horizontal));      //  pass 0 to activate Idle animation
                FindPlayer();                                           //	and try to find player
                return;                                                 //	ignore all other movement script
            }

            //	if zombie is attacking
            if (attacking)
            {
                AttackFreezeTimer();    //  freeze it in place
            }

            if (!attacking) //  if not attacking
            {
                //	check if zombie has run into an obstacle
                hasObstacle = HasObstacle();                                

                //	if no obstacles blocking path
                if (!hasObstacle)
                {
                    //	if zombie is near player
                    if (Math.Abs(target.position.x - zombie.position.x) < 1)
                    {
                        //	stop moving
                        horizontal = 0;
                        //	.. and if near player on Y axis
                        if ((Math.Abs(target.position.y - zombie.position.y) < 1))
                        {
                            //	..atack
							AudioManager.instance.PlaySound("EnemyAttack");
                            animator.SetTrigger("attack");
                            target.GetComponent<Player>().DamagePlayer(enemy.stats.Damage);
                            attacking = true;
                        }
                    }   //	else if player is to the right
                    else if (target.position.x > zombie.position.x)
                    {
                        horizontal = 1;
                    }    //	..if to the left
                    else if (target.position.x < zombie.position.x)
                    {
                        horizontal = -1;
                    }
                }
                else
                {   //	..if has run intro obstacle, ignore horizontal movement and do jump
                    velocity.y = jumpTakeOffSpeed;                          //	change velocity to jump velocity
                } 
            }
        }

        move.x = horizontal;                                        //	pass rezultin script value for movement along x axis
        if (isAlive)
        {
            animator.SetFloat("speed", Mathf.Abs(horizontal));          //	activate runing animation
        }

        targetVelocity = move * maxSpeed;                           //	seting resulting target velocity
        Flip(horizontal);                                           //	fliping object
    }


    //	fliping of sprite to face right direction
    private void Flip(float _horizontal)
    {
        //  if zombie is facing wrong direction...
        if (_horizontal < 0 && facingRight || _horizontal > 0 && !facingRight)
        {
            //  ...flip
            facingRight = !facingRight;
            Vector3 _scale = transform.localScale;
            _scale.x *= -1;
            transform.localScale = _scale;
        }
    }

    //	holding zombie in attack state for a moment
    private void AttackFreezeTimer()
    {
        //	count how long it needs to be in atack state
        timeLeftforAttack -= Time.deltaTime;
        if (timeLeftforAttack < 0)
        {
            //exit out off atack state and reset timer value
            attacking = false;
            timeLeftforAttack = attackTime;
        }
    }

    //	check if zombie has run into obstacle
    private bool HasObstacle()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(obstacleCheck.position, 0.01f, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }

}
