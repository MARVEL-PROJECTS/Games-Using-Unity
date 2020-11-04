using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    public enum SpawnerState {SPAWING, WAITING, COUNTING};

    [SerializeField]    //  prefab to spawn as enemt
    private Transform enemyPrefab;
    [SerializeField]    //  points where to spawn enemies
    private Transform[] enemySpawnPoints;
    [SerializeField]    //  delay between waves
    private float waveSpawnDelay;
    [SerializeField]    //  delay between enemy spawns
    private float enemySpawnDelay;
    [SerializeField]
    private float enemyCheckDelay;
    private float enemyCheckCountDown;

    private int waveCounter;        //  wave counter
    private int enemyCount;         //  enemyCount
    private float waveCountDown;     //  time count down before new wave spawn

    private SpawnerState state = SpawnerState.COUNTING;

    private void Start()
    {
        waveCounter = 0;
        enemyCount = 0;
        waveCountDown = waveSpawnDelay;
        enemyCheckCountDown = enemyCheckDelay; 
    }

    private void Update()
    {   //  if waiting for player to kill all enemies
        if (state == SpawnerState.WAITING)
        {   //  check if there is alive enemies..
            if (!IsEnemyAlive())
            {   //  ..and if is change state to next wave countdown
                state = SpawnerState.COUNTING;
                waveCountDown = waveSpawnDelay;
            }   //  .. if enemies still alive exit code
            else
            {
                return;
            }
        }
        //  if it is time to spawn ne wave
        if (waveCountDown <= 0) 
        {
            //  if spawner isn`t spawning already
            if (state != SpawnerState.SPAWING)
            {
				if (waveCounter == 0) {
					GameMaster.ui.SetWaveClearText("WAVE CLEAR");
				}
                waveCounter++;                                                  //	inc wave number
                enemyCount += waveCounter;                                      //	increase enemy count in wave
                GameMaster.ui.SetWaveNumber(waveCounter);                       //  update wave number in ui
                StartCoroutine(SpawnWave(enemyCount));                          //  start spawning
            }
        }
        else
        {// do countdown
            waveCountDown -= Time.deltaTime;
			if (waveCounter == 0) {
				GameMaster.ui.SetWaveClearText("WAVE INCOMING");
			}
            GameMaster.ui.SetWaveClearOpacity(Mathf.Clamp((waveCountDown / waveSpawnDelay),0,1));
        }
    }

    //  check if there is alive enemies 
    private bool IsEnemyAlive()
    {   //  delay for checking
        enemyCheckCountDown -= Time.deltaTime;
        if (enemyCheckCountDown <= 0f)
        {   //  resetind delay counter
            enemyCheckCountDown = enemyCheckDelay;
            //  checking enemies
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {   // if no alive found
                return false;
            }
        }   //  if check delay is still on or alive enemies
        return true;
    }

    //  spawner
    IEnumerator SpawnWave(int _enemyCount)
    {   //  change state
        state = SpawnerState.SPAWING;
        //  spawn enemies till the requered amount is spawned
        for (int i = 0; i < _enemyCount; i++)
        {
            SpawnEnemy();                                  
            yield return new WaitForSeconds(enemySpawnDelay);
        }
        //  when all enemeis spawned change to wait state
        state = SpawnerState.WAITING;
        yield break;
    }

    private void SpawnEnemy()
    {
        Transform selectedSpawnPoint = enemySpawnPoints[(int)Random.Range(0, enemySpawnPoints.Length)];    				 //	get spawn position												
        Transform _enemyTemp  = Instantiate(enemyPrefab, selectedSpawnPoint.position, selectedSpawnPoint.rotation);      //	spawn   
		_enemyTemp.transform.tag = "Enemy";
    }
}
