using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    public GameObject introScreen;

    public GameObject curtains;
    public Animator curtainsAnimator;

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

    public void StartLevel()
    {
        // play curtain open animation
        curtainsAnimator.SetTrigger("CurtainsOpen");
    }

    public void EndLevel()
    {
        // play curtain close animation
    }
}
