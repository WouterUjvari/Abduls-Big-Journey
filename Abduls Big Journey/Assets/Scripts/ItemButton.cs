using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{

    public int item;

    public void SelectItem(int item)
    {
        BattleManager.instance.SelectItem(item, GetComponent<Button>());
    }
}
