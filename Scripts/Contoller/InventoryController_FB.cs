using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
	// 인벤토리 ui및 모델 과 통신 두 요소 모두에 종속된다

	public class InventoryController_FB : MonoBehaviour
	{
		[SerializeField]
		private FarmingBox farmingBox;

		[SerializeField]
		private UIInventoryPage_FB inventoryUI;

		[SerializeField]
		private InventorySO inventoryData_player;

		[SerializeField]
		private InventorySO inventoryData_merged;

		[SerializeField]
		private Button BtnCancel;

		private Action OnOpened;
		private Action OnClosed;

        private GameObject character;

		UIInventoryPage_FB _inventorypage_FB;
		PlayerInput playerInput;

		private void Start()
		{
			PrepareUI();
			AddItemsFromServer();
			playerInput = GetComponent<PlayerInput>();
			OnOpened = farmingBox.OnOpened;
			OnClosed = farmingBox.OnClosed;

			BtnCancel.onClick.AddListener(() => CloseInventoryUI());
			BtnCancel.onClick.AddListener(() => OnClosed?.Invoke());

            character = GameObject.FindWithTag("Pilot");
		}

		/// <summary>
		/// 수정 중. 아이템 스폰 방식에 대해 좀 더 이야기 해 볼 필요가 있어 보임.
		/// </summary>
		public void AddItemsFromServer(/*서버에서 받아온 아이템 리스트 or 스폰시킬 아이템 목록*/)
		{
			List<BaseItem> items = new List<BaseItem>();

			// 초기 아이템 리스트가 정의되지 않았다면 새로 생성

			inventoryData_merged.Initialize();
			inventoryData_merged.OnInventoryUpdated += UpdateInventoryUI;
			UpdateInventoryUI(inventoryData_merged.GetCurrentInventoryState());

			Dictionary<int, InventoryItem> inventoryData = new Dictionary<int, InventoryItem>();

			// 받아온 아이템 리스트를 순회하면서 임시 인벤토리 양식에 추가
			foreach (var item in items)
			{
				//// 서버 아이템 ID와 일치하는 ItemList의 아이템 찾습니다
				//InventoryItem foundItem = ItemList.Find(item => item.item.ItemID == serverItem.TemplateId);
				//if (foundItem.item != null)
				//{
				//    Debug.Log(serverItem + "순회");
				//    // 아이템을 찾았다면, 새 인스턴스를 생성하고 수량을 설정합니다
				//    InventoryItem newItem = new InventoryItem(); // 새로운 InventoryItem 인스턴스를 만듭니다
				//    newItem.item = foundItem.item; // 찾은 아이템의 정보를 새 인스턴스에 복사합니다
				//    newItem.quantity = serverItem.Quantity; // 서버에서 받은 수량을 설정합니다
				//    inventoryData.AddItem(newItem);

				//}
			}
		}

		/// <summary>
		/// 인벤토리 데이터
		/// </summary>
		/// <param name="inventoryState"></param>
		private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
		{
			inventoryUI.ResetAllItems();
			//inventoryUI.ResetAllItems();//인벤토리 데이터를 업데이트 할때 한번초기화
			foreach (var item in inventoryState)
			{
				inventoryUI.UpdateData(item.Key, item.Value.item.ItemIcon, item.Value.quantity);
			}
		}

		/// <summary>
		/// 인벤토리 칸 세팅. 반드시 한 번만 호출되어야 한다
		/// </summary>
		private void PrepareUI()
		{
			inventoryUI.InitializeinventoryUI(inventoryData_player.Size, farmingBox.InventorySize);
			Debug.Log($"player:{inventoryData_player.Size}, farmingBox:{farmingBox.InventorySize}");
			inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
			inventoryUI.OnSwapItems += HandleSwapItems;
			inventoryUI.OnStartDragging += HandleDragging;
			inventoryUI.OnItemActionRequested += HandleItemActionRequest;
		}

		/// <summary>
		/// 플레이어 인벤토리 + 보관함 인벤토리 형태의 임시 SO 세팅
		/// </summary>
		private void PrepareMergedSO()
		{
			inventoryData_merged.Size = inventoryData_player.Size + farmingBox.InventorySize;
			inventoryData_merged.Initialize();

			for (int i = 0; i < inventoryData_player.Size; i++)
			{
				inventoryData_merged.InsertItem(i, inventoryData_player.GetItemAt(i));
			}

			foreach (KeyValuePair<int, InventoryItem> item in farmingBox.ItemList)
			{
				inventoryData_merged.InsertItem(item.Key + (inventoryData_player.Size - 1), item.Value);
			}
		}

		private void HandleItemActionRequest(int itemIndex)
		{
			InventoryItem inventoryItem = inventoryData_merged.GetItemAt(itemIndex);
			if (inventoryItem.IsEmpty)
				return;
		}

		private void DropItem(int itemIndex, int quantity)
		{
			inventoryData_merged.RemoveItem(itemIndex, quantity);
			inventoryUI.ResetSelection();
		}
		public void PerformAction(int itemIndex)
		{
			InventoryItem inventoryItem = inventoryData_merged.GetItemAt(itemIndex);
			if (inventoryItem.IsEmpty)
				return;


			IItemAction itemAction = inventoryItem.item as IItemAction;
			if (itemAction != null)
			{
				itemAction.PerformAction(character);

				if (inventoryData_merged.GetItemAt(itemIndex).IsEmpty)
				{
					inventoryUI.ResetSelection();
				}

			}

			IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
			if (destroyableItem != null)
			{
				inventoryData_merged.RemoveItem(itemIndex, 1);
			}
		}
		/// <summary>
		/// 인덱스를 입력받고, 해당 인덱스의 아이템을 드래그 중인 아이템으로 지정한다.
		/// </summary>
		/// <param name="itemIndex"></param>
		private void HandleDragging(int itemIndex)
		{
			InventoryItem inventoryItem = inventoryData_merged.GetItemAt(itemIndex);
			if (inventoryItem.IsEmpty)
			{
				return;
			}

			inventoryUI.CreateDraggedItem(inventoryItem.item.ItemIcon, inventoryItem.quantity);
		}

		/// <summary>
		/// 드래그 시작 지점의 인덱스와 드래그 끝 지점의 인덱스를 입력받아 서로의 아이템 값을 교체한다.
		/// </summary>
		/// <param name="itemIndex_1"></param>
		/// <param name="itemIndex_2"></param>
		private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
		{
			inventoryData_merged.SwapItems(itemIndex_1, itemIndex_2);
		}

		/// <summary>
		/// 실질적으로 아이템 설명 출력을 담당하는 부분
		/// </summary>
		/// <param name="itemIndex"></param>
		private void HandleDescriptionRequest(int itemIndex)
		{
			InventoryItem inventoryItem = inventoryData_merged.GetItemAt(itemIndex);
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


		/// <summary>
		/// 파밍 UI 를 연다.
		/// </summary>
		public void OpenInventoryUI()
		{
			if (inventoryUI.isActiveAndEnabled == false)
			{
                PrepareMergedSO();

				//playerInput.CanControl = false;
				foreach (var item in inventoryData_player.GetCurrentInventoryState())
				{
					inventoryUI.UpdateData(item.Key, item.Value.item.ItemIcon, item.Value.quantity);
				}

				foreach (var item in farmingBox.ItemList)
				{
					inventoryUI.UpdateData(
						item.Key + (inventoryData_player.Size - 1),
						item.Value.item.ItemIcon,
						item.Value.quantity
					);
				}

				inventoryUI.Show();
				OnOpened?.Invoke();
			}
		}

		/// <summary>
		/// 아이템 리스트에서 플레이어 인벤토리 구간과 보관함 인벤토리 구간을 구분하여, 서로 원상복구 해 주어야 한다.
		/// </summary>
		private void CloseInventoryUI()
		{
			if (inventoryUI.isActiveAndEnabled == true)
			{
				inventoryUI.Hide();

				UpdateInventories();

				OnClosed?.Invoke();

				farmingBox.SendFarmingBoxClose();
				//playerInput.CanControl = true;
			}
		}

		private void UpdateInventories()
		{

			Dictionary<int, InventoryItem> temp = inventoryData_merged.GetCurrentInventoryState();

			Dictionary<int, InventoryItem> farmingBoxInventory = new Dictionary<int, InventoryItem>();

			List<InventoryItem> playerInventory = SetEmptyPlayerInventory();

			foreach (KeyValuePair<int, InventoryItem> fbi in temp)
			{
				if (fbi.Key > inventoryData_player.Size - 1)
				{
					farmingBoxInventory.Add(fbi.Key - (inventoryData_player.Size - 1), fbi.Value);
				}
				else
				{
					playerInventory[fbi.Key] = fbi.Value;
				}
			}

			//보관함 인벤토리에 변경 사항 반영
			farmingBox.SetItemList(farmingBoxInventory);

			//플레이어 인벤토리에 변경 사항 반영
			inventoryData_player.SetInventoryItems(playerInventory);
			inventoryData_player.InformAboutChange();

			//파밍 UI 전용 임시 SO 초기화
			inventoryData_merged.Initialize();
		}

		private List<InventoryItem> SetEmptyPlayerInventory()
		{

			List<InventoryItem> playerInventory = new List<InventoryItem>();

			for (int i = 0; i < inventoryData_player.Size; i++)
			{
				playerInventory.Add(InventoryItem.GetEmptyItem());
			}

			return playerInventory;
		}
	}
}
