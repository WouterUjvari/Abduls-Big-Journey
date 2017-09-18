using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    public GameObject introScreen;

    public GameObject curtains;
    public Animator curtainsAnimator;

    public GameObject notificationPanel;
    public Text levelText;
    public Text notificationText;
    public Animator notificationsAnimator;

    public GameObject gameplayPanel;
    public GameObject itemPanel;

    public GameObject forceCursor;
    public Image forceCursorFill;

    public Image playerHealthbar;
    public Image enemyTotalHealthbar;

    public Text turnText;

    public GameObject gameEndPanel;
    public Text victoryOrDefeatText;

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
}
