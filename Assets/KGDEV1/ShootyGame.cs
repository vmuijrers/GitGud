using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootyGame : MonoBehaviour
{
    const float PLAYER_SPEED = 10f;
    const float ENEMY_SPEED = 5f;
    const float BULLET_SPEED = 25f;
    const float BULLET_LIFE = 5f;
    const int ENEMY_HEALTH = 25;

    public GameObject playerInstance;
    public GameObject enemyPrefab;
    public GameObject bulletPrefab;

    private List<GameObject> enemies = new List<GameObject>();
    private List<int> enemyHealth = new List<int>();

    private List<BulletEntity> bullets = new List<BulletEntity>();
    private List<float> bulletTimers = new List<float>();

    private float refireTime = 0;

    private ObjectPool<BulletEntity> bulletObjectPool; 

    // Start is called before the first frame update
    void Start()
    {
        bulletObjectPool = new ObjectPool<BulletEntity>(bulletPrefab, 10);
        // Spawn some enemies
        for ( int i = 0; i < 10; ++i ) {
            GameObject enemy = GameObject.Instantiate(enemyPrefab);
            enemy.transform.position = new Vector3(Random.Range(-100,100), 0, Random.Range(-100,100));
            enemies.Add(enemy);
            enemyHealth.Add(ENEMY_HEALTH);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update enemies
        for ( int i = 0; i < enemies.Count; ++i ) {
            // Move towards player
            enemies[i].transform.Translate( ( playerInstance.transform.position - enemies[i].transform.position ).normalized * Time.deltaTime * ENEMY_SPEED );
        }

        // Move player
        playerInstance.transform.Translate( Input.GetAxis("Horizontal") * Time.deltaTime * PLAYER_SPEED, 0, Input.GetAxis("Vertical") * Time.deltaTime * PLAYER_SPEED, Space.World);
		
        Vector3 mousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
		mousePosition.y = transform.position.y;

		// Fire gun
		refireTime -= Time.deltaTime;
        if ( Input.GetMouseButton(0) && refireTime <= 0 ) {
            //GameObject bullet = GameObject.Instantiate(bulletPrefab);			
            BulletEntity bullet = bulletObjectPool.RequestFromPool();			
			bullet.GameObjectInstance.transform.position = playerInstance.transform.position + ( mousePosition - playerInstance.transform.position ).normalized;
            bullet.GameObjectInstance.transform.LookAt(mousePosition);
            bullets.Add(bullet);
            bulletTimers.Add(BULLET_LIFE);
		}

        // LOOKAT
		transform.LookAt(mousePosition);
        // Update bullets
        for ( int i = 0; i < bullets.Count; ++i ) {
            RaycastHit hitInfo;
            if ( Physics.Raycast(bullets[i].transform.position, bullets[i].transform.forward, out hitInfo, BULLET_SPEED * Time.deltaTime)) {

                for ( int j = 0; j < enemies.Count; ++j ) {
                    if ( enemies[j] == hitInfo.collider.gameObject ) {
                        enemyHealth[j] -= 1;
                        if ( enemyHealth[j] <= 0 ) {
                            GameObject.Destroy(enemies[j]);
                            enemies.RemoveAt(j);
                            enemyHealth.RemoveAt(j);
                        }
                        break;
                    }
                }
                bulletObjectPool.ReturnToPool(bullets[i]);
                //GameObject.Destroy(bullets[i]);
                bullets.RemoveAt(i--);
                continue;
            }
            else {
                bulletTimers[i] -= Time.deltaTime;
                if ( bulletTimers[i] <= 0 ) {
                    bulletObjectPool.ReturnToPool(bullets[i]);
                    //GameObject.Destroy(bullets[i]);
                    bullets.RemoveAt(i);
                    bulletTimers.RemoveAt(i--);
                    continue;
                }
                bullets[i].transform.Translate(bullets[i].transform.forward * BULLET_SPEED * Time.deltaTime, Space.World);
            }
        }
    }
}


public static class Actions
{
   
    public static void Jump(GameObject jumper, float height)
    {

    }

}

public class Context : MonoBehaviour
{
    public void Start()
    {
        Enemy enemy = new();
        ScoreBoard scoreBoard = new ScoreBoard();

        //enemy.OnAddScore += scoreBoard.AddPoint;
    }

}

public class EnemyManager
{
    public Enemy prefab;

    void Start()
    {
    }

    private void SpawnEnemies()
    {
        Enemy enemy = MonoBehaviour.Instantiate(prefab);
        enemy.OnDeath += HandleEnemyDeath;
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        enemy.OnDeath -= HandleEnemyDeath;
        Object.Destroy(enemy.gameObject);
    }
}


namespace EventExample
{
    public enum EventType
    {
        ON_GAME_STARTED = 0,
        ON_PLAYER_DIED = 1
    }
    public static class EventManager
    {
        private static Dictionary<EventType, System.Action> eventDictionary = new Dictionary<EventType, System.Action>();

        public static void RaiseEvent(EventType eventType)
        {
            if (eventDictionary.ContainsKey(eventType))
            {
                eventDictionary[eventType]?.Invoke();
            }
        }

        public static void SubscribeToEvent(EventType eventType, System.Action action)
        {
            if (eventDictionary.ContainsKey(eventType))
            {
                eventDictionary[eventType] += action;
            }
            else
            {
                eventDictionary.Add(eventType, action);
            }

        }

        public static void UnSubscribeFromEvent(EventType eventType, System.Action action)
        {
            if (eventDictionary.ContainsKey(eventType))
            {
                eventDictionary[eventType] -= action;
            }
        }
    }
}
