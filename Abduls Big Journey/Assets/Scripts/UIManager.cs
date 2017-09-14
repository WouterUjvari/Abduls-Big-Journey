using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    public GameObject introScreen;

    public GameObject curtains;
    public Animator curtainsAnimator;

    public GameObject itemPanel;

    public GameObject forceCursor;
    public Image forceCursorFill;

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
