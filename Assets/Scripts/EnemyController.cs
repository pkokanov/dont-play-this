using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    public GameObject enemy1Prefab;
    public GameObject enemy2Prefab;
    public GameObject[] enemySpawnPoints;
    public int maxNumEnemies;
    public int maxNumEnemies1;
    public int maxNumEnemies2;

    public int currentEnemies;
    public int currentEnemies1;
    public int currentEnemies2;
    private Hashtable enemySpawnMap;
    private ArrayList enemySpawnList;
	// Use this for initialization
	void Start () {
        currentEnemies1 = 0;
        currentEnemies2 = 0;
        currentEnemies = 0;
        enemySpawnMap = new Hashtable();
        enemySpawnList = new ArrayList();
        enemySpawnList.AddRange(enemySpawnPoints);
        SpawnEnemies();
    }
	
	// Update is called once per frame
	void Update () {
        ArrayList list = new ArrayList();
        ICollection keys = enemySpawnMap.Keys;
        list.AddRange(keys);

        foreach (Object key in list)
        {
            GameObject enemy = enemySpawnMap[key] as GameObject;
            if (!enemy)
                break;
            EnemyMove enemyControl = enemy.GetComponent<EnemyMove>();
            if(!enemyControl)
                break;
            if(enemyControl.dead)
            {
                currentEnemies--;
                if (enemyControl.myName == "Enemy1") {
                    currentEnemies1--;
                }
                if (enemyControl.myName == "Enemy2") {
                    currentEnemies2--;
                }
                Destroy(enemy);
                enemySpawnList.Add(key);
                enemySpawnMap.Remove(key);
            }
        }
	}

    Object InstantiateInternal(Object obj, GameObject spawnPoint)
    {
        return Instantiate(obj, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }

    public void SpawnEnemies()
    {
        for (int numEnemies = currentEnemies; numEnemies < maxNumEnemies; numEnemies++)
        {
            int spawnPointIdx = Random.Range(0, enemySpawnList.Count);
            GameObject spawnPoint = enemySpawnList[spawnPointIdx] as GameObject;
            enemySpawnList.RemoveAt(spawnPointIdx);
            if (currentEnemies1 < maxNumEnemies1 && currentEnemies2 < maxNumEnemies2)
            {
                int enemyType = Random.Range(0, 2);
                if (enemyType == 0)
                {
                    enemySpawnMap[spawnPoint] = InstantiateInternal(enemy1Prefab, spawnPoint);
                    (enemySpawnMap[spawnPoint] as GameObject).GetComponent<EnemyMove>().myName = "Enemy1";
                    currentEnemies1++;
                }
                else
                {
                    enemySpawnMap[spawnPoint] = InstantiateInternal(enemy2Prefab, spawnPoint);
                    (enemySpawnMap[spawnPoint] as GameObject).GetComponent<EnemyMove>().myName = "Enemy2";
                    currentEnemies2++;
                }
            }
            else if (currentEnemies1 < maxNumEnemies1)
            {
                enemySpawnMap[spawnPoint] = InstantiateInternal(enemy1Prefab, spawnPoint);
                (enemySpawnMap[spawnPoint] as GameObject).GetComponent<EnemyMove>().myName = "Enemy1";
                currentEnemies1++;
            }
            else if (currentEnemies2 < maxNumEnemies2)
            {
                enemySpawnMap[spawnPoint] = InstantiateInternal(enemy2Prefab, spawnPoint);
                (enemySpawnMap[spawnPoint] as GameObject).GetComponent<EnemyMove>().myName = "Enemy2";
                currentEnemies2++;
            }
            currentEnemies++;
        }
    }

    void DestroyEnemies()
    {
        ArrayList list = new ArrayList();
        ICollection keys = enemySpawnMap.Keys;
        list.AddRange(keys);

        foreach (Object key in list)
        {
            GameObject enemy = enemySpawnMap[key] as GameObject;
            if (!enemy)
                break;
            EnemyMove enemyControl = enemy.GetComponent<EnemyMove>();
            if (!enemyControl)
                break;
            currentEnemies--;
            if (enemyControl.myName == "Enemy1")
            {
                currentEnemies1--;
            }
            if (enemyControl.myName == "Enemy2")
            {
                currentEnemies2--;
            }
            Destroy(enemy);
            enemySpawnList.Add(key);
            enemySpawnMap.Remove(key);
        }
    }

    public void RecreateAllEnemies()
    {
        gameObject.SetActive(false);
        DestroyEnemies();
        SpawnEnemies();
        gameObject.SetActive(true);
    }
}
