using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {
    
	[HideInInspector] public float minGroundNormalY = .65f;					//	minimal ground normal to be considered ground
	[HideInInspector] public float gravityModifier = 1f;						//	gravity modifier
	protected Rigidbody2D rb2d;								//	object rigidbodu
    protected Vector2 velocity;								//	velocity variable

	protected Vector2 targetVelocity;						//	target velocity that the objevts wants to move (input)
	protected bool grounded;								//	is object grounded
	protected Vector2 groundNormal;
	protected ContactFilter2D contactFilter;
	protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
	protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D> (16);

	protected const float shellRadius = 0.01f;			//	shell radius for colision deceting
	protected const float minMoveDistance = 0.001f;		//	minimal move distance for object to start chechking colision

	private void OnEnable()
	{
		rb2d = GetComponent<Rigidbody2D> ();				//	reference to objects rigidbody

	}

	// Use this for initialization
	private void Start ()
    {
		//	contact filter layer setup 
		contactFilter.useTriggers = false;					
		contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask (gameObject.layer));
		contactFilter.useLayerMask = true;
	}
	
	// Update is called once per frame
	private void Update ()
    {
		targetVelocity = Vector2.zero;
		ComputeVelocity ();
	}

	protected virtual void ComputeVelocity()
	{
		
	}

    private void FixedUpdate(){
		
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
		velocity.x = targetVelocity.x;

		grounded = false;

		Vector2 deltaPosition = velocity * Time.deltaTime;

		Vector2 moveAloongGround = new Vector2 (groundNormal.y, - groundNormal.x);

		Vector2 move = moveAloongGround * deltaPosition.x;

		Movement (move, false);

		move = Vector2.up * deltaPosition.y;

		Movement (move, true);
	}

	//	moving object rigidbody
	private void Movement(Vector2 move, bool yMovement){
		float distance = move.magnitude;

		//	if object is moving check colisions
		if (distance > minMoveDistance) 
		{
			//	do colision cast in move direction, using contact filter, with move distance + shell radius
			int count = rb2d.Cast (move, contactFilter, hitBuffer, distance + shellRadius);
			hitBufferList.Clear ();
			for (int i = 0; i < count; i++) 
			{
				hitBufferList.Add(hitBuffer [i]);
			}

			for (int i = 0; i < hitBufferList.Count; i++) 
			{
				Vector2 currentNormal = hitBufferList [i].normal;
				if (currentNormal.y > minGroundNormalY) 
				{
					grounded = true;
					if (yMovement) 
					{
						groundNormal = currentNormal;
						currentNormal.x = 0;
					}
				}
				float projection = Vector2.Dot (velocity, currentNormal);
				if (projection < 0) 
				{
					velocity -= projection * currentNormal;
				}

				float modifiedDistance = hitBufferList [i].distance - shellRadius;
				distance = modifiedDistance < distance ? modifiedDistance : distance;
			}
		}

		rb2d.position += move.normalized * distance;
	}
}
