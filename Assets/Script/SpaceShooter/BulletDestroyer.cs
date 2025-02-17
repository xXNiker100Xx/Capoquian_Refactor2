using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    [SerializeField] EnemySpaceShooter[] ESS;
    GameObject[] enemies;
    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        ESS = new EnemySpaceShooter[enemies.Length];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            BulletPool.Instance.ReturnBullet(collision.gameObject);
        }
        if (collision.CompareTag("EnemyBullet"))
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                ESS[i] = enemies[i].GetComponent<EnemySpaceShooter>();
            }
        }
    }
}
