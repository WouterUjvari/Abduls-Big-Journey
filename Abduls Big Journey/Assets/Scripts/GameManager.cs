using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

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
        if (level == 0)
        {
            UIManager.instance.curtainsAnimator.SetTrigger("CloseCurtains");

            yield return new WaitForSeconds(UIManager.instance.curtainsAnimator.GetCurrentAnimatorClipInfo(0).Length);

            UIManager.instance.gameplayPanel.SetActive(false);
            UIManager.instance.itemPanel.SetActive(false);

            AsyncOperation operation = SceneManager.LoadSceneAsync(level);
            UIManager.instance.introScreen.SetActive(false);

            while (!operation.isDone)
            {
                yield return null;
            }

            UIManager.instance.curtainsAnimator.SetTrigger("OpenCurtains");
        }
        else
        {
            UIManager.instance.curtainsAnimator.SetTrigger("CloseCurtains");

            yield return new WaitForSeconds(UIManager.instance.curtainsAnimator.GetCurrentAnimatorClipInfo(0).Length + 0.5f);

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
    }

    public void ReplayLevel(int level)
    {
        if (UIManager.instance.gameEndPanel.activeInHierarchy)
        {
            UIManager.instance.gameEndPanel.SetActive(false);
        }

        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void DisableClickedButton(Button button)
    {
        button.interactable = false;
    }
}
