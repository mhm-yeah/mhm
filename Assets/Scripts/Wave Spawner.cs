using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] Wave[] waves;

    private Wave currentWave;
    private GameObject enemyFolder;
    private int currentWaveNumber;
    private bool canSpawn = true;
    private float nextSpawnTime;
    private float spawnInterval = 1f;
    private int minimumEnemiesLeft = 5;
    private int previousWave = -1;
    private bool isBossSpawn = false;

    public GameObject bossPrefab;
    private Vector3 bossSpawnOffset = new Vector3(0f, 10f, 0f);
    private GameObject playerObj;

    private void Start()
    {
        enemyFolder = GameObject.Find("Enemies");
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        currentWave = waves[currentWaveNumber];
        SpawnWave();
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (totalEnemies.Length <= minimumEnemiesLeft && !canSpawn && currentWaveNumber + 1 != waves.Length)
        {
            currentWaveNumber++;
            canSpawn = true;
        }
        if (currentWaveNumber != previousWave)
        {
            previousWave = currentWaveNumber;

            ChangeMusicForWave(currentWaveNumber);
        }
        //if(totalEnemies.Length <= 0 && !isBossSpawn && currentWaveNumber > 4)
        //{
            
        //    isBossSpawn = true;
        //}
    }

    private void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            GameObject randomEnemy = currentWave.enemyTypes[Random.Range(0, currentWave.enemyTypes.Length)];
            Instantiate(randomEnemy, GetOffScreenCoord(), Quaternion.identity, enemyFolder.transform);
            currentWave.numberOfEnemies--;
            nextSpawnTime = Time.time + spawnInterval;
            if (currentWave.numberOfEnemies <= 0)
            {
                canSpawn = false;
                if(currentWaveNumber == 3)
                {
                    Vector3 position = playerObj.transform.position + bossSpawnOffset;
                    GameObject boss = Instantiate(bossPrefab, position, Quaternion.identity);
                }
            }

        }
    }

    private Vector3 GetOffScreenCoord()
    {
        Vector2 bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        float offset = 3f; //how far outside the screen 
        float x = Random.Range(-bounds.x - offset, bounds.x + offset);
        float y = Random.Range(-bounds.y - offset, bounds.y + offset);

        if (Mathf.Abs(x) < bounds.x) // if inside screen - froce it out
        {
            x = Mathf.Sign(x) * (bounds.x + offset);
        }
        if (Mathf.Abs(y) < bounds.y) // same but y axis
        {
            y = Mathf.Sign(y) * (bounds.y + offset);
        }

        return new Vector3(x, y, 0);
    }
    private void ChangeMusicForWave(int wave)
    {
        if (wave < 2)
        {
            AudioManager.instance.PlayMusic(AudioManager.instance.background1);
        }
        else if (wave < 3)
        {
            AudioManager.instance.PlayMusic(AudioManager.instance.background2);
        }
        else if (wave < 4)
        {
            AudioManager.instance.PlayMusic(AudioManager.instance.background3);
        }
        else
        {
            AudioManager.instance.PlayMusic(AudioManager.instance.bossBattle);
        }
    }
}
[System.Serializable]
public class Wave
{
    public string waveName;
    public int numberOfEnemies;
    public GameObject[] enemyTypes;
}