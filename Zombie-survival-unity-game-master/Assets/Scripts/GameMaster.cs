using UnityEngine;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;
    public static int gameScore;
    public static UI_Update ui;

    [SerializeField]
	private Transform playerPrefab;
	private Transform playerSpawnPoint;
    [SerializeField]
    private GameObject gameOverUI;
   
    private void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
        //  reference check
        if (playerPrefab == null)
        {
            Debug.LogError("GAME MASTER: No player prefab referenced!");
        }
        if (gameOverUI == null)
        {
            Debug.LogError("GAME MASTER: No gameOverUI referenced!");
        }

        ui = GameObject.Find("UI_Overlay").GetComponent<UI_Update>();
        if (ui == null)
        {
            Debug.LogError("GAME MASTER: No UI_Overlay referenced, or no UI_Update script found !");
        }
        gameScore = 0;
    }

    // Use this for initialization
    private void Start()
    {
		//	finds player spawn point
		playerSpawnPoint = GameObject.Find ("PlayerSpawnPoint").transform;
		if (playerSpawnPoint == null) {
			Debug.LogError ("GameMaster: No PlayerSpawnPoint game object found !!!");
		}
       
    }

	//	kill player
	public static void KillPlayer(Player player) {
        Destroy(player.gameObject);
	}

    public void EndGame()
    {
        
        gameOverUI.SetActive(true);
    }

    /* not using player respawn anymore
	//	player Respawn
	public IEnumerator RespawnPlayer() {
		yield return new WaitForSeconds (playerRespawnDelay);
		Instantiate (playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
	}
    */

	//	kill enemy
	public static void KillEnemy(Enemy enemy)	{
        gameScore += enemy.GetComponent<Enemy>().stats.killScore;
        Destroy (enemy.gameObject, 3f);
	}
}
