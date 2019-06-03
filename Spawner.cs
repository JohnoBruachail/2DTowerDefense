using UnityEngine;
using System.Collections;

[System.Serializable]
public class Wave
{
    public int[] waveComposition;
    public float spawnInterval = 2;
}

public class Spawner : MonoBehaviour
{
    public GameObject[] waypoints;
    public GameObject[] EnemysList;
    GameObject newEnemy;
    public Wave[] waves;
    public int timeBetweenWaves = 5;

    private GameManagerBehaviour gameManager;
    private int newEnemyType;
    private float lastSpawnTime;
    private int enemiesSpawned = 0;

    void Start()
    {
        lastSpawnTime = Time.time;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
        gameManager.spawners.Add(this);
    }

    void Update()
    {
        //check if all waves have ended
        //if(gameManager.Wave);

        int currentWave = gameManager.Wave;
        if (currentWave < waves.Length){
            float timeInterval = Time.time - lastSpawnTime;
            float spawnInterval = waves[currentWave].spawnInterval;

            if (((enemiesSpawned == 0 && timeInterval > timeBetweenWaves) || timeInterval > spawnInterval) && enemiesSpawned < waves[currentWave].waveComposition.Length){
                lastSpawnTime = Time.time;

                newEnemyType = waves[currentWave].waveComposition[enemiesSpawned];

                //GameObject newEnemy = (GameObject)Instantiate(EnemysList[newEnemyType]);
                newEnemy = Instantiate(EnemysList[newEnemyType], new Vector3(1000,1000,0), Quaternion.identity);

                newEnemy.GetComponent<HeroBehaviour>().waypoints = waypoints;
                enemiesSpawned++;
            }
            if (enemiesSpawned == waves[currentWave].waveComposition.Length && GameObject.FindGameObjectWithTag("Enemy") == null){
                gameManager.Wave++;
                gameManager.Mana = Mathf.RoundToInt(gameManager.Mana * 1.1f);
                enemiesSpawned = 0;
                lastSpawnTime = Time.time;
            }
        }
        else
        {
            gameManager.GameOverWin();
        }
    }

}
