using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField]    // target for camera to follow
    private Transform target;
    
    [Range(0f, 1f)]     //  smoothing amaunt
    private float smoothSpeed = 0.125f;
    
    [SerializeField]    //  camera limits
    private Transform bottomLeftLimit;
    [SerializeField]
    private Transform topRightLimit;

    private float camHeight;
    private float camWidth;
    private Camera cam;

    private Vector2 xBounds;
    private Vector2 yBounds;

    private Vector3 velocity = Vector3.zero;

	private float nextPlayerSearchTime = 0;

    private void Awake()
    {
        cam = this.GetComponent<Camera>();
        {
            if (cam == null)
            {
                Debug.LogError("Camera Folow: no Camera Component found!");
            }
        }
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        // resets boundaries vector
        xBounds = Vector2.zero;
        yBounds = Vector2.zero;
        // Sets camera boundaries
        SetLimits();
    }

    private void SetLimits()
    {
        if (bottomLeftLimit == null)
        {
            Debug.LogWarning("Camera Folow: no camera bottom-left limit set, seting to default value of -900");
            xBounds.x -= 900;
            yBounds.x -= 900;
        }
        else
        {
            xBounds.x = bottomLeftLimit.transform.position.x;
            yBounds.x = bottomLeftLimit.transform.position.y;
        }

        if (topRightLimit == null)
        {
            Debug.LogWarning("Camera Folow: no camera top-right limit set, seting to default value of +900");
            xBounds.x += 900;
            yBounds.x += 900;
        }
        else
        {
            xBounds.y = topRightLimit.transform.position.x;
            yBounds.y = topRightLimit.transform.position.y;
        }
    }

    private void LateUpdate()
    {
		if (target == null) {
			FindPlayer ();
			return;
		}
        //  get desied position and clamp it inside bounds
        float desiredX = Mathf.Clamp(target.position.x, xBounds.x +camWidth, xBounds.y - camWidth);
        float desiredY = Mathf.Clamp(target.position.y, yBounds.x + camHeight, yBounds.y - camHeight);
        Vector3 desiredPosition = new Vector3(desiredX, desiredY, -10);
       
        //  do smoohing
        Vector3 smoothedPostion = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        //  apply to camera 
        transform.position = smoothedPostion;
    }

	void FindPlayer() {
		if (nextPlayerSearchTime <= Time.time) {
			GameObject searchRezult = GameObject.FindGameObjectWithTag ("Player");
			if (searchRezult != null) {
				target = searchRezult.transform;
				nextPlayerSearchTime = Time.time + 0.5f;
			}
		}

	}
}
