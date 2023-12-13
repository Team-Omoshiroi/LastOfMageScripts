
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{

    // 인벤토리 ui및 모델 과 통신 두요소모두에 종속된다  

    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private List<InventoryItem> ItemList = new List<InventoryItem>();
        [SerializeField] private InventorySO inventoryData;
        private UIInventoryPage inventoryUI;

        public List<InventoryItem> initialItems = new();

        private void Start()
        {
            inventoryUI = UIManager.Instance.ShowUI<UIInventoryPage>(null, UIController.Instance.UIRoot);

            if (inventoryUI)
            {
                UIController.Instance.InventoryUI = inventoryUI.gameObject;
                UIController.Instance.InventoryUI.SetActive(false);
                UIController.Instance.BtnCancel = inventoryUI.CancleBtn;
            }
            else
            {
                Debug.LogError("NullinventoryUI");
            }

            PrepareUI();
            PrepareInventoryData();
            //AddItemsFromServer(DataManager.Instance.items);

            UIController.Instance.BtnInventory.onClick.AddListener(() =>
            {
                SoundManager.Instance.Play("Effect/MenuSFX/Abstract/abs-confirm-1", eSoundType.PlayerEffect);
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    //_inventorypage = UIManager.Instance.ShowUI<UIInventoryPage>();  //ui매니져 사용해서 동적으로 껐다키기 
                    inventoryUI.Show();
                    //playerInput.CanControl = false;
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UpdateData(item.Key, item.Value.item.ItemIcon, item.Value.quantity);
                    }
                }
                else
                {
                    inventoryUI.Hide();
                    //playerInput.CanControl = true;
                }
            });

            UIController.Instance.BtnCancel.onClick.AddListener(() =>
            {
                SoundManager.Instance.Play("Effect/MenuSFX/Abstract/abs-cancel-1", eSoundType.PlayerEffect);
                inventoryUI.Hide();
            });
        }
        //public void AddItemsFromServer(List<PlayerItemRes> serverItems)
        //{
        //    // 초기 아이템 리스트가 정의되지 않았다면 새로 생성

        //    inventoryData.Initialize();
        //    inventoryData.OnInventoryUpdated += UpdateInventoryUI;
        //    // 서버 아이템을 순회하면서 인벤토리에 추가
        //    foreach (var serverItem in serverItems)
        //    {

        //        // 서버 아이템 ID와 일치하는 ItemList의 아이템을 찾습니다
        //        InventoryItem foundItem = ItemList.Find(item => item.item.ItemID == serverItem.TemplateId);
        //        if (foundItem.item != null)
        //        {
        //            Debug.Log(serverItem + "순회");
        //            // 아이템을 찾았다면, 새 인스턴스를 생성하고 수량을 설정합니다
        //            InventoryItem newItem = new InventoryItem(); // 새로운 InventoryItem 인스턴스를 만듭니다
        //            newItem.item = foundItem.item; // 찾은 아이템의 정보를 새 인스턴스에 복사합니다
        //            newItem.quantity = serverItem.Quantity; // 서버에서 받은 수량을 설정합니다
        //            inventoryData.AddItem(newItem);

        //        }
        //    }

        //}
        private void PrepareInventoryData()//인베토리 데이터가 바뀔때 호출 
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;

            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                inventoryData.AddItem(item);
            }
        }
        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();//인벤토리데이터 업데이트 할때 한번초기화 
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemIcon, item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeinventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }



        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
        }

        private void DropItem(int itemIndex, int quantity)
        {
           
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();
        }

        public void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;


            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject); // 소비아이템 사용 

                if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                {
                    inventoryUI.ResetSelection();
                }

            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                return;
            }

            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemIcon, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            BaseItem item = inventoryItem.item;

            if (item.ItemType == eItemType.Weapon)
            {
                inventoryUI.UpdateDescription(itemIndex, item.ItemIcon, $"{item.name}\n{item.ItemDescription}", item.ItemDescription);
            }
            else
            {
                inventoryUI.UpdateDescription(itemIndex, item.ItemIcon, item.name, item.ItemDescription);
            }
            //----------  액션패널
            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {

                inventoryUI.ShowItemAction(itemIndex);
                inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));

            }
        }



    }
}