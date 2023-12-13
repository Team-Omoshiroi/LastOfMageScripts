using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Inventory;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Google.Protobuf.Collections;

/// <summary>
/// 테스트가 용이하도록 임의대로 아이템 스폰을 자체적인 SO 로 관리하도록 해 두었음.
/// </summary>
///

/// 파밍박스 인벤토리 데이터 요청 패킷을 생성, 서버 측으로 보낸다. 이후, 서버에서 이에 대한 응답을 받아왔다면
/// S_FarmingBoxOpenHandler => OpenBox 순서로 실행되며 파밍박스를 열지 말지 결정하게 된다.

public class FarmingBox : BattleFieldObject, ILootable, IInteractable, IPointerDownHandler
{
    [SerializeField]
    private int inventorySize;

    [SerializeField]
    private InventorySO inventorySO;

    [SerializeField]
    private InventoryController_FB inventoryController;

    private Collider farmingBoxCollider;

    public Dictionary<int, InventoryItem> ItemList { get; private set; }
    public int InventorySize
    {
        get { return inventorySize; }
        private set { inventorySize = value; }
    }

    public Action OnOpened;
    public Action OnClosed;

    private void Awake()
    {
        OnOpened = () => SoundManager.Instance.Play("Effect/Shapeforms Audio Free Sound Effects/The Mint – Coins and Money Preview/AUDIO/Coins in Sack Dropped on Soft Surface", eSoundType.PlayerEffect);
        OnClosed = () => SoundManager.Instance.Play("Effect/Shapeforms Audio Free Sound Effects/The Mint – Coins and Money Preview/AUDIO/Coins In Sack Dropped on Wood", eSoundType.PlayerEffect);
    }

    // Start is called before the first frame update
    private void Start()
    {
        ItemList = new Dictionary<int, InventoryItem>();

        Dictionary<int, InventoryItem> items = inventorySO.GetCurrentInventoryState();

        foreach (KeyValuePair<int, InventoryItem> item in items)
        {
            ItemList.Add(item.Key, item.Value);
        }

        inventoryController = GetComponent<InventoryController_FB>();
        farmingBoxCollider = GetComponent<Collider>();
        
    }

    /// <summary>
    /// 이 보관함의 인벤토리에서 아이템을 가져갈 때 마다 호출 될 예정.
    /// </summary>
    public void Loot() { }

    public void Interact() { }

    public void SetItemList(Dictionary<int, InventoryItem> items)
    {
        ItemList = items;
    }

    /// <summary>
    /// FarmingBox 를 여는 트리거. FarmingBox 가 부착된 오브젝트 클릭/터치 시 작동한다.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        SendFarmingBoxOpen();
        inventoryController.OpenInventoryUI();
    }

    /// <summary>
    /// 서버에서 받아온 [farmingBoxID, isOpen, farmingBoxInventory] 데이터를 이용해 작업을 수행한다.
    /// 해당하는 파밍박스를 식별하고, isOpen 이 true 라면 파밍박스를 열 수 없음을 알리고 종료, false 라면 파밍박스 인벤토리를 데이터대로 갱신해주면 된다.
    /// </summary>
    public void OpenBox(S_FarmingBoxOpen FBData)
    {
        if (FBData.IsOpen)
        {
            //상자를 열 수 없는 상태라는 것을 알려주는 연출에 대한 내용.
            Debug.Log($"{FBData.FarmingBoxId} 를 누군가가 뒤져 보는 중이므로 열 수 없습니다.");
            return;
        }

        SetFBItems(FBData);
        inventoryController.OpenInventoryUI();
    }

    /// <summary>
    /// 서버에서 받아온 패킷으로 FarmingBox 인벤토리를 갱신한다.
    /// </summary>
    /// <param name="FBData">서버에서 받아온 패킷</param>
    private void SetFBItems(S_FarmingBoxOpen FBData)
    {
        Debug.Log("파밍박스 인벤토리 데이터 받아옴");
        RepeatedField<FarmingBoxItem> items = FBData.Items;

        Dictionary<int, InventoryItem> fbItems = new Dictionary<int, InventoryItem>();

        InventoryItem inven;
        DataManager dataManager = DataManager.Instance;

        for (int i = 0; i < items.Count; i++)
        {
            inven = new InventoryItem();
            inven.item = dataManager.FindItem(items[i].ItemId);
            inven.quantity = items[i].Quantity;

            fbItems.Add(i, inven);
        }

        ItemList = fbItems;
    }

    /// <summary>
    /// 파밍박스 인벤토리 데이터 요청 패킷을 생성, 서버 측으로 보낸다. 이후, 서버에서 이에 대한 응답을 받아왔다면
    /// S_FarmingBoxOpenHandler => OpenBox 순서로 실행되며 파밍박스를 열지 말지 결정하게 된다.
    /// </summary>
    private void SendFarmingBoxOpen()
    {
        Debug.Log("C_FarmingBoxOpen 패킷 전송");
        C_FarmingBoxOpen fbOpenPacket = new C_FarmingBoxOpen();
        fbOpenPacket.FarmingBoxId = ObjectId;

        NetworkManager.Instance.Send(fbOpenPacket);
    }

    /// <summary>
    /// 서버 측의 파밍박스 인벤토리 데이터 갱신 요청 패킷을 생성, 서버 측으로 보낸다. 서버는 별도의 응답을 하지 않는다.
    /// </summary>
    public void SendFarmingBoxClose()
    {
        Debug.Log("C_FarmingBoxClose 패킷 전송");
        C_FarmingBoxClose fbClosePacket = new C_FarmingBoxClose();
        fbClosePacket.FarmingBoxId = this.gameObject.GetInstanceID();
        fbClosePacket.PlayerId = ObjectId;
        FarmingBoxItem fbItem = new FarmingBoxItem();
        InventoryItem invenItem;
        foreach (KeyValuePair<int, InventoryItem> item in ItemList)
        {
            invenItem = item.Value;
            fbItem.ItemId = invenItem.item.ItemID;
            fbItem.Quantity = invenItem.quantity;
            fbClosePacket.Items.Add(fbItem);
        }

        NetworkManager.Instance.Send(fbClosePacket);
    }
}

public partial class PacketHandler
{
    public static void S_FarmingBoxOpenHandler(PacketSession session, IMessage packet)
    {
        Debug.Log("서버에서 S_FarmingBoxOpen 패킷 받아옴");
        S_FarmingBoxOpen FBPacket = packet as S_FarmingBoxOpen;

        GameObject gameObject = ObjectManager.Instance.FindById(FBPacket.FarmingBoxId);

        //해당 ID 를 가지는 파밍박스가 존재하지 않는다면 작동하지 않는다.
        if (gameObject == null)
        {
            Debug.Log("존재하지 않는 FarmingBox 입니다.");
            //return;
        }

        FarmingBox farmingBox = gameObject.GetComponent<FarmingBox>();

        farmingBox.OpenBox(FBPacket);
    }

    public static void S_FarmingBoxSpawnHandler(PacketSession session, IMessage packet)
    {
        S_FarmingBoxSpawn FBPacket = packet as S_FarmingBoxSpawn;

        RepeatedField<ObjectInfo> FBSpawnInfos = FBPacket.BoxInfos;

        foreach (ObjectInfo fb in FBSpawnInfos)
        {
            Debug.Log($"Object ID ({fb.ObjectId}) 생성 시도 중...");
            //원래는 Instantiate 할 것이 아니라, ObjectPool 에서 관리하면서 SetActive 설정하는 식이 되어야 할 것임.
            //현재는 테스트용으로 별개의 Spawner 를 두고서 Instantiate 하도록 만듦.
            //ObejctID 를 별도로 설정할 수 있는지 모르겠음. 클라이언트 측 객체에도 ObjectID 에 대응되는 멤버변수가 필요하다 생각됨.

            //farmingBox = ObjectManager.Instance.FindById(fb.ObjectId);

            //if (farmingBox == null) { return; }
            //if (farmingBox.GetComponent<FarmingBox>() == null) { return; }

            //farmingBox.transform.position = new Vector3(fb.PosInfo.PosX, fb.PosInfo.PosY, fb.PosInfo.PosZ);

            //farmingBox.SetActive(true);

            BattleFieldObjectSpawner.instance.SpawnFarmingBox(
                fb.ObjectId,
                new Vector3(fb.PosInfo.PosX, fb.PosInfo.PosY, fb.PosInfo.PosZ)
            );
        }
    }
}
