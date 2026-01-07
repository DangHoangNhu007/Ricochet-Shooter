using UnityEngine;
using DG.Tweening; // Vẫn dùng DOTween cho mượt

public class BulletController : MonoBehaviour
{
    [Header("Explosion FX")]
    public GameObject explosionPrefab; // Prefab nổ (Particle System)
    public int bounces = 0; // Đếm số lần nảy (để tính điểm nếu thích)
    public int maxBounces = 5; // Giới hạn nảy để tránh đạn bay mãi mãi

    private bool _hasHitEnemy = false;

    void OnCollisionEnter(Collision collision)
    {
        // 1. Va chạm với Tường -> Nảy
        if (collision.gameObject.CompareTag("Wall"))
        {
            bounces++;
            // Hiệu ứng nảy nhẹ (Squash) cho viên đạn thêm sinh động
            transform.DOPunchScale(Vector3.one * 0.2f, 0.1f);

            // Nếu nảy quá nhiều thì hủy đạn
            if (bounces > maxBounces)
            {
                Explode(false); // Nổ nhỏ, không làm gì cả
            }
        }

        // 2. Va chạm với Kẻ địch -> BÙM!
        else if (collision.gameObject.CompareTag("Enemy") && !_hasHitEnemy)
        {
            _hasHitEnemy = true;
            KillEnemy(collision.gameObject);
        }
    }

    void KillEnemy(GameObject enemy)
    {
        // Tắt physics của đạn để nó dừng lại ngay tại chỗ va chạm
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;

        // Ẩn hình ảnh viên đạn đi
        GetComponent<Renderer>().enabled = false;

        // --- XỬ LÝ KẺ ĐỊCH ---
        // Gọi hàm chết của Enemy (nếu có script riêng), hoặc xử lý trực tiếp ở đây
        // Ở đây mình làm trực tiếp cho nhanh:

        // Tạo hiệu ứng nổ hoành tráng
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, enemy.transform.position, Quaternion.identity);
        }

        ShatterEffect shatter = enemy.GetComponent<ShatterEffect>();

        if (shatter != null)
        {
            shatter.Explode(); // BÙM! Vỡ tan tành
        }
        else
        {
            // Nếu quên gắn script thì destroy bình thường
            Destroy(enemy);
        }

        // Tạo hiệu ứng Slow Motion (Juice cực mạnh!)
        DoSlowMotionEffect();

        // Hủy kẻ địch
        //Destroy(enemy);

        // Hủy viên đạn sau 1 chút
        Destroy(gameObject, 1f);

        // TODO: Gọi GameManager báo thắng
        Debug.Log("ENEMY DOWN! LEVEL CLEARED!");
        if (GameManagerShooter.Instance != null)
        {
            GameManagerShooter.Instance.OnEnemyKilled();
        }
    }

    void Explode(bool isBigExplosion)
    {
        // Hiệu ứng nổ nhỏ khi hết đạn
        if (explosionPrefab != null && isBigExplosion)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    void DoSlowMotionEffect()
    {
        // Giảm thời gian xuống còn 10% tốc độ thật
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Cập nhật physics cho mượt slow-mo

        // Dùng DOTween để hồi phục lại thời gian sau 1 giây (thời gian thực)
        DOVirtual.DelayedCall(1f, () =>
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }, true); // true = ignoreTimeScale (chạy theo thời gian thực của máy tính)
    }
}