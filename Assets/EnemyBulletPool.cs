using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int poolSize = 10;

    private IObjectPool<GameObject> enemybulletPool;

    void Awake()
    {

        enemybulletPool = new ObjectPool<GameObject>(
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
        return enemybulletPool.Get();
    }


    public void ReturnBullet(GameObject bullet)
    {
        enemybulletPool.Release(bullet);
    }
}
