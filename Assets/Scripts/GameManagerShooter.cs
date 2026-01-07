using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Nếu muốn hiện Text UI
using System.Collections;

public class GameManagerShooter : MonoBehaviour
{
    public static GameManagerShooter Instance;
    
    [Header("UI")]
    public GameObject winPanel; // Panel hiện chữ "CLEARED" + Nút Next Level

    private int _enemyCount;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Đếm số lượng Enemy có trong màn chơi lúc bắt đầu
        _enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        
        if(winPanel != null) 
            winPanel.SetActive(false);
    }

    public void OnEnemyKilled()
    {
        _enemyCount--;
        
        // Nếu hết Enemy thì Thắng
        if (_enemyCount <= 0)
        {
            Debug.Log("ALL ENEMIES DEAD! WIN!");
            StartCoroutine(WinSequence());
        }
    }

    IEnumerator WinSequence()
    {
        // Chờ 1.5s để người chơi tận hưởng cảnh Slow-mo vỡ vụn
        yield return new WaitForSecondsRealtime(1.5f); 
        
        if(winPanel != null)
            winPanel.SetActive(true);
    }

    // Gắn vào nút "Next Level" trên UI
    public void LoadNextLevel()
    {
        Debug.Log("Loading Next Level...");
        // Load màn tiếp theo theo thứ tự trong Build Settings
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        // Nếu hết màn rồi thì quay lại màn 1
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0; 
        }
        
        SceneManager.LoadScene(nextSceneIndex);
    }
    
    // Gắn vào nút "Retry" (nếu có)
    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}