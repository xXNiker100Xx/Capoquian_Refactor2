using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpaceShooter : MonoBehaviour
{
    public SpaceshipController SpaceShip;
    public int health;
    public float minFR, MaxFR;
    private float FireRate;
    private float storedFireRate;
    public float BulletSpeed;

    public float moveSpeed;
    public float moveInterval;

    public GameObject bulletPrefab; // Each enemy has its own bullet prefab

    private IObjectPool<GameObject> EnemyBulletPool;

    public Vector3 InitialPosition;
    // Start is called before the first frame update
    void Start()
    {
        InitialPosition = transform.position;
        //minFR = 1
        //maxFR = 5
        FireRate = Random.Range(minFR, MaxFR);
        storedFireRate = FireRate;
        //InvokeRepeating
        InvokeRepeating("MoveEnemy",5, moveInterval);

        EnemyBulletPool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false);
                return bullet;
            },
            actionOnGet: (bullet) => bullet.SetActive(true),
            actionOnRelease: (bullet) => bullet.SetActive(false),
            actionOnDestroy: (bullet) => { },
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );

    }


    // Update is called once per frame
    void Update()
    {
        FireRate -= Time.deltaTime;
        if (FireRate <= 0)
        {
            SpawnBullet();
            FireRate = storedFireRate;
        }
        if (health <= 0)
        {
            //Destroy(gameObject);
            //gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            health--;
            if (health <= 0)
            {
                SpaceShip.score++;
                gameObject.SetActive(false);
            }
            BulletPool.Instance.ReturnBullet(collision.gameObject);
        }
    }

    public void SpawnBullet()
    {
        GameObject bullet = EnemyBulletPool.Get();

        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.linearVelocity = new Vector2(0f, -BulletSpeed);
        StartCoroutine(ReturnBulletAfterDelay(bullet, 3f));

    }

    IEnumerator ReturnBulletAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnBullet(bullet);
    }

    public void ReturnBullet(GameObject bullet)
    {
        EnemyBulletPool.Release(bullet);
    }
    public void MoveEnemy()
    {
        //Moves the enemy downwards aloth the y axis.
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }
}
