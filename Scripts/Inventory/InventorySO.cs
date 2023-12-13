using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//
[CreateAssetMenu]

//모델
public class InventorySO : ScriptableObject
{


    [SerializeField]
    private List<InventoryItem> inventoryItems;
    BaseItem item;
    [field: SerializeField]
    public int Size { get; set; } = 10;

    public List<InventoryItem> InventoryItems { get; private set; }

    public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

    public void Initialize()
    {
        inventoryItems = new List<InventoryItem>();
        for (int i = 0; i < Size; i++)
        {
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }

    public void SetInventoryItems(List<InventoryItem> items)
    {
        inventoryItems = items;
    }

    public void InsertItem(int index, InventoryItem item)
    {
        if (index >= 0 && index < inventoryItems.Count)
        {
            inventoryItems[index] = item;
        }
    }

    public int AddItem(BaseItem item, int quantity)
    {
        if (item.IsStackable == false)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {

                while (quantity > 0 && IsInventoryFull() == false)
                {
                    quantity -= AddItemToFirstFreeSlot(item, 1);

                }
                InformAboutChange();
                return quantity;
            }
        }


        quantity = AddStackableItem(item, quantity);
        InformAboutChange();
        return quantity;
    }

    private int AddItemToFirstFreeSlot(BaseItem item, int quantity)

    {
        InventoryItem newItem = new InventoryItem
        {
            item = item,
            quantity = quantity,
        };

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
            {
                inventoryItems[i] = newItem;
                return quantity;
            }
        }
        return 0;
    }

    private bool IsInventoryFull() => inventoryItems.Where(item => item.IsEmpty).Any() == false;

    private int AddStackableItem(BaseItem item, int quantity)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
                continue;
            if (inventoryItems[i].item.ItemID == item.ItemID)
            {
                int amountPossibleToTake =
                       inventoryItems[i].item.ItemMaxStack - inventoryItems[i].quantity;

                if (quantity > amountPossibleToTake)
                {
                    inventoryItems[i] = inventoryItems[i]
                        .ChangeQuantity(inventoryItems[i].item.ItemMaxStack);
                    quantity -= amountPossibleToTake;
                }
                else
                {
                    inventoryItems[i] = inventoryItems[i]
                        .ChangeQuantity(inventoryItems[i].quantity + quantity);
                    InformAboutChange();
                    return 0;
                }
            }
        }
        while (quantity > 0 && IsInventoryFull() == false)// 갇득 찼거나  수량이 0보다 작으면 
        {
            int newQuantity = Mathf.Clamp(quantity, 0, item.ItemMaxStack);
            quantity -= newQuantity;
            AddItemToFirstFreeSlot(item, newQuantity);
        }
        return quantity;
    }
    public void RemoveItem(int itemIndex, int amount)
    {
       
        if (inventoryItems.Count > itemIndex)
        {
            if (inventoryItems[itemIndex].IsEmpty)
                return;
            int reminder = inventoryItems[itemIndex].quantity - amount;
            if (reminder <= 0)
                inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
            else
                inventoryItems[itemIndex] = inventoryItems[itemIndex]
                    .ChangeQuantity(reminder);

            InformAboutChange();
        }
    }

    public Dictionary<int, InventoryItem> GetCurrentInventoryState()
    {
        Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
            {
                continue;
            }
            returnValue[i] = inventoryItems[i];
        }
        return returnValue;
    }

    public InventoryItem GetItemAt(int itemIndex)
    {
        return inventoryItems[itemIndex];
    }

    public void AddItem(InventoryItem item)
    {
        AddItem(item.item, item.quantity);
    }

    public void SwapItems(int itemIndex_1, int itemIndex_2)
    {
        InventoryItem item1 = inventoryItems[itemIndex_1];
        inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
        inventoryItems[itemIndex_2] = item1;
        InformAboutChange();
    }

    public void InformAboutChange() // 현재 가저온 인벤토리에이터  인벤토리컨트롤러에 전달 
    {
        OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
    }
}



[Serializable]
public struct InventoryItem
{
    public int quantity;
    public BaseItem item;

    public bool IsEmpty => item == null;

    public InventoryItem ChangeQuantity(int newQuantity)
    {
        return new InventoryItem
        {
            item = this.item,
            quantity = newQuantity,
        };
    }

    public static InventoryItem GetEmptyItem()
        => new InventoryItem
        {
            item = null,
            quantity = 0,
        };

}