using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
        #endregion

        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
        UIManager.instance.introScreen.SetActive(false);
        UIManager.instance.StartLevel();
    }
}
