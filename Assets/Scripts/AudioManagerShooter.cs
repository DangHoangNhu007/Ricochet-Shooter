using UnityEngine;

public class AudioManagerShooter : MonoBehaviour
{
    public static AudioManagerShooter Instance;

    [Header("Clips")]
    public AudioClip shootClip;
    public AudioClip bounceClip;
    public AudioClip explodeClip;
    
    private AudioSource _source;

    void Awake()
    {
        Instance = this;
        _source = GetComponent<AudioSource>();
        // Nếu chưa có AudioSource thì tự thêm
        if (_source == null) _source = gameObject.AddComponent<AudioSource>();
    }

    public void PlayShoot()
    {
        // Random pitch một chút để nghe đỡ chán
        _source.pitch = Random.Range(0.9f, 1.1f);
        _source.PlayOneShot(shootClip);
    }

    public void PlayBounce()
    {
        _source.pitch = Random.Range(0.8f, 1.2f);
        // Tiếng nảy nên nhỏ hơn tiếng bắn/nổ
        _source.PlayOneShot(bounceClip, 0.7f); 
    }

    public void PlayExplode()
    {
        _source.pitch = 1f;
        _source.PlayOneShot(explodeClip);
    }
}