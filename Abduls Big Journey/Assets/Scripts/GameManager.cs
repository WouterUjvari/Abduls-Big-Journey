using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

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
        StartCoroutine(LoadLevelAsync(level));
    }

    public IEnumerator LoadLevelAsync(int level)
    {
        UIManager.instance.curtainsAnimator.SetTrigger("CloseCurtains");

        yield return new WaitForSeconds(UIManager.instance.curtainsAnimator.GetCurrentAnimatorClipInfo(0).Length);

        AsyncOperation operation = SceneManager.LoadSceneAsync(level);
        UIManager.instance.introScreen.SetActive(false);

        while (!operation.isDone)
        {
            yield return null;
        }

        UIManager.instance.curtainsAnimator.SetTrigger("OpenCurtains");
    }
}
