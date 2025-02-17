using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Pool;

public class SpaceshipController : MonoBehaviour
{
    public List<EnemySpaceShooter> Enemies;

    public float Speed;
    public float BulletSpeed;
    public Transform BulletSpawnHere;
    public GameObject GameClearScreen;
    public TextMeshProUGUI textValue,hpValue;
    public int score;
    public int hitponts;
    bool isGameClear = false;
    private int storeHP;
    public GameObject GameOverScreen;
    private bool canMove = true;
    private bool canShoot = true;
    // Start is called before the first frame update

    [SerializeField] EnemySpaceShooter[] ESS;
    GameObject[] enemies;
    void Start()
    {
        storeHP = hitponts;

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        ESS = new EnemySpaceShooter[enemies.Length];
    }

    // Update is called once per frame
    void Update()
    {
        textValue.text = score.ToString();
        hpValue.text = hitponts.ToString();
        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            SpawnBullet();
        }

        if (hitponts <= 0)
        {
            canShoot = false;
            canMove = false;
            GameOverScreen.SetActive(true);
            hitponts = 0;
        }
        OnGameClear();
        
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 moveInput = new Vector3(horizontalInput, 0, 0);
            transform.position += Time.deltaTime * Speed * moveInput;
        }
    }

    public void SpawnBullet()
    {
        GameObject bullet = BulletPool.Instance.GetBullet();

        bullet.transform.position = BulletSpawnHere.position;
        bullet.transform.rotation = Quaternion.identity;
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.linearVelocity = new Vector2(0f, BulletSpeed);
        StartCoroutine(ReturnBulletAfterDelay(bullet, 3f));

    }

    IEnumerator ReturnBulletAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (bullet != null) BulletPool.Instance.ReturnBullet(bullet);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            hitponts--;
            for (int i = 0; i < enemies.Length; i++)
            {
                ESS[i] = enemies[i].GetComponent<EnemySpaceShooter>();
            }
        }
    }

    public void RestartGame()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].transform.position = Enemies[i].InitialPosition;
            Enemies[i].gameObject.SetActive(false);
            //Delays the call of a method in Ienumerator
            StartCoroutine(DelayEnemiesActive());
        }
        canMove = true;
        canShoot = true;
        hitponts = storeHP;
        score = 0;
        isGameClear = false;
        GameOverScreen.SetActive(false);
    }
    IEnumerator DelayEnemiesActive()
    {
        yield return new WaitForSeconds(0.25f);
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].gameObject.SetActive(true);
        }
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnGameClear()
    {
        isGameClear = true;
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].gameObject.activeSelf)
            {
                isGameClear = false;
                break;
            }
        }
        if (isGameClear)
        {
            GameClearScreen.SetActive(true);
        }
    }
}

