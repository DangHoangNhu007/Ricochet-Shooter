using UnityEngine;
using DG.Tweening; 

public class BulletController : MonoBehaviour
{
    [Header("Explosion FX")]
    public GameObject explosionPrefab; 
    public int bounces = 0; 
    public int maxBounces = 5; 

    private bool _hasHitEnemy = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            bounces++;
            transform.DOPunchScale(Vector3.one * 0.2f, 0.1f);
            AudioManagerShooter.Instance.PlayBounce();

            if (bounces > maxBounces)
            {
                Explode(false); 
            }
        }

        else if (collision.gameObject.CompareTag("Enemy") && !_hasHitEnemy)
        {
            _hasHitEnemy = true;
            KillEnemy(collision.gameObject);
        }
    }

    void KillEnemy(GameObject enemy)
    {
        AudioManagerShooter.Instance.PlayExplode();
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;

        GetComponent<Renderer>().enabled = false;


        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, enemy.transform.position, Quaternion.identity);
        }

        ShatterEffect shatter = enemy.GetComponent<ShatterEffect>();

        if (shatter != null)
        {
            shatter.Explode(); 
            Camera.main.transform.DOShakePosition(0.3f, 0.5f, 10, 90);
        }
        else
        {
            Destroy(enemy);
        }

        DoSlowMotionEffect();

        Destroy(gameObject, 1f);

        Debug.Log("ENEMY DOWN! LEVEL CLEARED!");
        if (GameManagerShooter.Instance != null)
        {
            GameManagerShooter.Instance.OnEnemyKilled();
        }
    }

    void Explode(bool isBigExplosion)
    {
        if (explosionPrefab != null && isBigExplosion)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    void DoSlowMotionEffect()
    {
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; 

        DOVirtual.DelayedCall(1f, () =>
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }, true); 
    }
}