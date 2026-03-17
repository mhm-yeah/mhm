using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject meleeEnemyPrefab;
    [SerializeField]
    private GameObject rangedEnemyPrefab;

    [SerializeField]
    private float screenOffset = 3f; //how far outside the screen 


    [SerializeField]
    private float meleeEnemyInterval = 3f;
    [SerializeField]
    private float rangedEnemyInterval = 7f;
    private void Start()
    {
        StartCoroutine(spawnEnemy(meleeEnemyInterval, meleeEnemyPrefab));
        StartCoroutine(spawnEnemy(rangedEnemyInterval, rangedEnemyPrefab));
    }

    private IEnumerator spawnEnemy(float spawnTime, GameObject enemy)
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            Instantiate(enemy, GetOffScreenCoord(), Quaternion.identity);
        }
    }
    private Vector3 GetOffScreenCoord()
    {
        Vector2 bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        //float offset = 3f; 
        float x = Random.Range(-bounds.x - screenOffset, bounds.x + screenOffset);
        float y = Random.Range(-bounds.y - screenOffset, bounds.y + screenOffset);

        if (Mathf.Abs(x) < bounds.x) // if inside screen - froce it out
        {
            x = Mathf.Sign(x) * (bounds.x + screenOffset);
        }
        if (Mathf.Abs(y) < bounds.y) // same but y axis
        {
            y = Mathf.Sign(y) * (bounds.y + screenOffset);
        }

        return new Vector3(x, y, 0);
    }
}
