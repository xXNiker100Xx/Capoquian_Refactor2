using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;
    public GameObject bulletPrefab;
    public int poolSize = 10;

    private IObjectPool<GameObject> bulletPool;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        bulletPool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false);
                return bullet;
            },
            actionOnGet: (bullet) =>
            {
                bullet.SetActive(true);
            },
            actionOnRelease: (bullet) =>
            {
                bullet.SetActive(false);
            },
            actionOnDestroy: (bullet) => { },
            collectionCheck: false,
            defaultCapacity: poolSize,
            maxSize: poolSize * 2
        );
    }

    public GameObject GetBullet()
    {
        return bulletPool.Get();
    }

    public void ReturnBullet(GameObject bullet)
    {
        bulletPool.Release(bullet);
    }
}
