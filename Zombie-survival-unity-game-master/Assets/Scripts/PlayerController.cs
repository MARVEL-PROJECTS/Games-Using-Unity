using UnityEngine;

public class PlayerController : PhysicsObject
{

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    private Animator animator;
    private bool facingRight;                           //  player facing direction
    public LayerMask whatToHit;
    Transform firePoint;
	[SerializeField]
	private float fireRate = 3;						//	how many shots per second player can do
	float timeToFire = 0;

    public Transform bulletTrailPrefab;

    // Use this for initialization
    private void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No player weapon firePoint!!!");
        }
        animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;                                //	reset movement vector at frame start
        float horizontal = Input.GetAxis("Horizontal");         //	get user input for movement alon x axis
        move.x = horizontal;
        animator.SetFloat("speed", Mathf.Abs(horizontal));          //	activate runing animation

        //	if user wants and is able to jump
        if (Input.GetButtonDown("Jump") && grounded)
        {
			AudioManager.instance.PlaySound("PlayerJump");
            velocity.y = jumpTakeOffSpeed;                          //	change velocity to jump velocity
        }
        else if (Input.GetButtonUp("Jump"))
        {                       //	if jump key released mid air
            if (velocity.y > 0)
            {                                   //	if player is still moving up	
                velocity.y *= .5f;                                  //	reduce players vertical velocity by half
            }
        }

        targetVelocity = move * maxSpeed;

        Flip(horizontal);

        //	if player wants to attack
        if (Input.GetButtonDown("Fire1"))
        {
			if (timeToFire <= 0) {
				Shoot ();
				animator.SetTrigger ("attack");
				timeToFire = 1 / fireRate;
			} 
        }
		if (timeToFire > 0) {
			timeToFire -= Time.deltaTime;
		}
    }

    private void Shoot()
    {
		Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
		Vector2 firingDirection;
		firingDirection = facingRight ? Vector2.right : Vector2.left;
		RaycastHit2D hit = Physics2D.Raycast(firePointPosition, firingDirection, 100, whatToHit);

		//	if bullet had hit something
		if (hit.collider != null)
		{
			float hitX = hit.collider.transform.position.x;         //  get hit x coordinate
			Enemy enemy = hit.collider.GetComponent<Enemy>();      //  try to get Enemy reference
			BulletTrail(hitX, true, enemy);
		}
		else
		{   // .. if not
			BulletTrail(0, false, null);
		}
		AudioManager.instance.PlaySound ("RifleSound");

		
    }

    private void Flip(float horizontal)
    {
        //  if player is facing wrong direction...
        if (horizontal < 0 && facingRight || horizontal > 0 && !facingRight)
        {
            //  ...flip	
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void BulletTrail(float hitX, bool hasHit, Enemy enemy)
    {
        if (hasHit)
        {   //  has hit something, set hit arguments
            if (facingRight)
            {
                Instantiate(bulletTrailPrefab, firePoint.position, Quaternion.LookRotation(Vector3.down, Vector3.forward)).GetComponent<MoveBullet>().setHit(hitX, enemy);
            }
            else
            {
                Instantiate(bulletTrailPrefab, firePoint.position, Quaternion.LookRotation(Vector3.up, Vector3.forward)).GetComponent<MoveBullet>().setHit(hitX, enemy);
            }
        }
        else
        {        //  hasn`t hit anything
            if (facingRight)
            {
                Instantiate(bulletTrailPrefab, firePoint.position, Quaternion.LookRotation(Vector3.down, Vector3.forward));
            }
            else
            {
                Instantiate(bulletTrailPrefab, firePoint.position, Quaternion.LookRotation(Vector3.up, Vector3.forward));
            }
        }

    }
}
