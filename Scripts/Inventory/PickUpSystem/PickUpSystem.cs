using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;

    private void OnTriggerEnter(Collider collision)
    {

        
        Item item = collision.GetComponent<Item>();
       
        if (item != null )
        {
            SoundManager.Instance.Play("Effect/8-bit 16-bit Sound Effects/Confirm 1", eSoundType.PlayerEffect);
            int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
            if (reminder == 0)
            {

                item.DestroyItem();
            }           
            else
            {
              
                item.Quantity = reminder;

            }         
        }
    }

}
