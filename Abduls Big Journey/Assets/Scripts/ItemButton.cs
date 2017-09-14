using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{

    public int item;

    public void SelectItem(int item)
    {
        Battle.instance.SelectItem(item, GetComponent<Button>());
    }
}
