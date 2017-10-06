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

    public GameObject speechBubble;

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

    public void NewSpeechBubble(Transform transform, string text, float maxSize, float desiredSize, float transformSpeed, float fadeSpeed)
    {
        GameObject newSpeechBubble = Instantiate(speechBubble, transform.position, Quaternion.identity, transform);

        SpeechBubble speechBubbleComponent = newSpeechBubble.GetComponent<SpeechBubble>();

        speechBubbleComponent.text = text;
        speechBubbleComponent.maxSize = maxSize;
        speechBubbleComponent.desiredSize = desiredSize;
        speechBubbleComponent.transformSpeed = transformSpeed;
        speechBubbleComponent.fadeSpeed = fadeSpeed;
    }
}
