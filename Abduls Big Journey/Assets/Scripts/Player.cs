using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    private bool instantiatedItem;

    public Animator anim;

    public Transform throwingPosition;
    public GameObject itemAboutToThrow;

    public int maxHealth;
    public int currentHealth;

    [Header("Attack Force")]
    public float throwForce;
    public float maxForce = 80;
    public float forceIncreaseSpeed = 3f;

    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            BattleManager.instance.battleState = BattleManager.BattleState.End;
        }
    }

    public void ChargeAttack()
    {
        AnimatorClipInfo[] currentAnimationClip = anim.GetCurrentAnimatorClipInfo(0);
        if (!anim.GetBool("CanAttack"))
        {
            anim.SetBool("CanAttack", true);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.15f)
        {
            anim.speed = 0.8f;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.35f)
        {
            anim.speed = 0.2f;

            if (!instantiatedItem)
            {
                itemAboutToThrow = Instantiate(BattleManager.instance.items[BattleManager.instance.itemSelected], throwingPosition.position, Quaternion.identity);
                itemAboutToThrow.transform.rotation *= Quaternion.Euler(180, 0, 0);
                itemAboutToThrow.transform.SetParent(throwingPosition);
                itemAboutToThrow.GetComponent<Rigidbody2D>().isKinematic = true;

                instantiatedItem = true;
            }
        }
        else
        {
            anim.speed = 0.05f;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            Attack();
            return;
        }

        if (throwForce <= maxForce)
        {
            throwForce += Time.deltaTime * forceIncreaseSpeed;
        }
        UIManager.instance.forceCursorFill.fillAmount = throwForce / maxForce;
    }

    public void Attack()
    {
        BattleManager.instance.playerCanAttack = false;

        // instantiating the item, adding force to it and removing it from the list

        itemAboutToThrow.transform.SetParent(null);
        itemAboutToThrow.GetComponent<Rigidbody2D>().isKinematic = false;
        itemAboutToThrow.GetComponent<Item>().canRotate = true;

        Vector2 direction = (Battle.instance.mousePos - transform.position).normalized;
        itemAboutToThrow.GetComponent<Rigidbody2D>().AddForce(direction * throwForce);

        Battle.instance.availableItems.Remove(BattleManager.instance.items[BattleManager.instance.itemSelected]);

        BattleManager.instance.selectedItem = false;

        // leave behind an image of the cursor with a low alpha to let the player know where his last throw was
        if (Battle.instance.lastThrowLocation != null)
        {
            Destroy(Battle.instance.lastThrowLocation);
        }

        Battle.instance.lastThrowLocation = Instantiate(UIManager.instance.forceCursor, UIManager.instance.forceCursor.transform.position, Quaternion.identity, UIManager.instance.forceCursor.transform.parent);

        Image[] images = Battle.instance.lastThrowLocation.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            image.CrossFadeAlpha(0.2f, 0, true);
        }

        anim.SetBool("CanAttack", false);
        anim.speed = 1f;

        instantiatedItem = false;

        // resetting force 
        UIManager.instance.forceCursor.SetActive(false);
        throwForce = 0;
        UIManager.instance.forceCursorFill.GetComponent<Image>().fillAmount = 0;
    }

    public void CancelAttack()
    {
        anim.SetBool("CanAttack", false);
        anim.speed = 1f;
        anim.StopPlayback();

        if (itemAboutToThrow != null)
        {
            Destroy(itemAboutToThrow);
            instantiatedItem = false;
        }

        BattleManager.instance.selectedItem = false;

        // resetting force
        UIManager.instance.forceCursor.SetActive(false);
        throwForce = 0;
        UIManager.instance.forceCursorFill.fillAmount = 0;

        // setting the item button to interactable again
        BattleManager.instance.lastSelectedItemButton.interactable = true;
    }

    public void Hit(int damage)
    {
        currentHealth -= damage;
    }
}
