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
        if (UIManager.instance.gameEndPanel.activeInHierarchy)
        {
            UIManager.instance.gameEndPanel.SetActive(false);
        }

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

        BattleManager.instance.StartLevel();

        UIManager.instance.curtainsAnimator.SetTrigger("OpenCurtains");

        yield return new WaitForSeconds(UIManager.instance.curtainsAnimator.GetCurrentAnimatorClipInfo(0).Length);

        UIManager.instance.levelText.text = "Level: " + level;
        UIManager.instance.levelText.enabled = true;
        UIManager.instance.notificationsAnimator.SetTrigger("SetActive");
        yield return new WaitForSeconds(UIManager.instance.notificationsAnimator.GetCurrentAnimatorClipInfo(0).Length);
        UIManager.instance.notificationsAnimator.SetTrigger("SetInActive");
        yield return new WaitForSeconds(UIManager.instance.notificationsAnimator.GetCurrentAnimatorClipInfo(0).Length);

        yield return new WaitForSeconds(0.5f);
        UIManager.instance.levelText.enabled = false;

        BattleManager.instance.battleState = BattleManager.BattleState.Battling;
    }

    public void ReplayLevel(int level)
    {
        if (UIManager.instance.gameEndPanel.activeInHierarchy)
        {
            UIManager.instance.gameEndPanel.SetActive(false);
        }

        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
}
