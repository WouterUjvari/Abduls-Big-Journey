using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{

    private Animator anim;
    private Text textComponent;

    [Header("Properties")]
    public string text;

    public float maxSize;
    public float desiredSize;
    public float transformSpeed;

    public float fadeSpeed;

    public bool destroy;

    private void Awake()
    {
        textComponent = transform.GetChild(0).GetComponent<Text>();
    }

    private void Update()
    {
        textComponent.text = text;

        if (destroy)
        {
            destroy = false;
            Destroy(gameObject);
        }
    }
}
