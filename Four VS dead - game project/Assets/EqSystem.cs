using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EqSystem : MonoBehaviour
{
    public ItemSO[] allItems;
    public GameObject itemPrefab;
    public GameObject ContentSV;
    public GameObject selectedMenu;
    public Image selectedImg;
    public TMP_Text selectedName;
    public TMP_Text selectedDes;
    public int selectedItem;

    private void Start()
    {
        AddItemToEq(0);
    }
    public void Select(int itemId)
    {
        selectedItem = itemId;
        foreach(ItemSO it in allItems)
        {
            if(it.id == itemId)
            {
                selectedImg.sprite = it.ItemImg;
                selectedName.text = it.ItemName;
                selectedDes.text = it.ItemDescription;
            }
        }
        selectedMenu.SetActive(true);
    }

    public void Use()
    {
        switch (selectedItem)
        {
            case 0:
                break;
        }
    }

    public void AddItemToEq(int id)
    {
        GameObject obj = Instantiate(itemPrefab) as GameObject;
        obj.transform.parent = ContentSV.transform;
        obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        obj.GetComponent<EqItemHandler>().id = id;
        foreach (ItemSO it in allItems)
        {
            if (it.id == id)
            {
                obj.GetComponent<EqItemHandler>().ItemIcon.sprite = it.ItemImg;
                break;
            }
        }
        
    }
}
