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

    //currently
    //checks the current wave every update
    //if current wave is less the wavers length
    
    //wave is enemy type, a spawn interval and a max enemy number
    //difficulty can have an additional #endregion

    //wait wait wait.
    //create a table of enemy types
    //on each spawn roll a random number in a range to dictate enemy type.
    //difficulty adds to this number range

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

                if(waves[currentWave].waveComposition[enemiesSpawned] + gameManager.difficulty > EnemysList.Length){
                    newEnemyType = EnemysList.Length;
                }
                else{
                    newEnemyType = waves[currentWave].waveComposition[enemiesSpawned] + gameManager.difficulty;
                }

                GameObject newEnemy = (GameObject)Instantiate(EnemysList[newEnemyType]);

                newEnemy.GetComponent<HeroBehaviour>().waypoints = waypoints;
                enemiesSpawned++;
            }
            if (enemiesSpawned == waves[currentWave].waveComposition.Length && GameObject.FindGameObjectWithTag("Enemy") == null){
                gameManager.Wave++;
                gameManager.Gold = Mathf.RoundToInt(gameManager.Gold * 1.1f);
                enemiesSpawned = 0;
                lastSpawnTime = Time.time;
            }
        }
        else
        {
            gameManager.gameOver = true;
            GameObject gameOverText = GameObject.FindGameObjectWithTag("GameWon");
            gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
        }
    }

}
