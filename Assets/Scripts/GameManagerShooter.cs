using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using System.Collections;

public class GameManagerShooter : MonoBehaviour
{
    public static GameManagerShooter Instance;
    
    [Header("UI")]
    public GameObject winPanel;

    private int _enemyCount;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        
        if(winPanel != null) 
            winPanel.SetActive(false);
    }

    public void OnEnemyKilled()
    {
        _enemyCount--;
        
        if (_enemyCount <= 0)
        {
            Debug.Log("ALL ENEMIES DEAD! WIN!");
            StartCoroutine(WinSequence());
        }
    }

    IEnumerator WinSequence()
    {
        yield return new WaitForSecondsRealtime(1.5f); 
        
        if(winPanel != null)
            winPanel.SetActive(true);
    }

    public void LoadNextLevel()
    {
        Debug.Log("Loading Next Level...");
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0; 
        }
        
        SceneManager.LoadScene(nextSceneIndex);
    }
    
    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}