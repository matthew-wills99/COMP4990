using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
    //This script is for the inventory. Managing the slots, items, stacks, classes, etc.\
public class InventoryManager : MonoBehaviour
{
    //Allows other scripts to use this script (Building, resources, etc.)
    public static InventoryManager instance;
    public List<Item> startItems;
    public int maxStackedItems = 73;
    public GameObject InventoryItemPrefab;
    public List<InventorySlot> inventorySlots;
    int selectedSlot = -1;

    private void Awake(){
        instance = this;
    }
    //Making sure the slot selected at the start is the first slot, then adds starting items.
    private void Start(){
        ChangeSelectedSlot(0);
        foreach (var item in startItems){
            AddItem(item);
        }
    }

    //Changing which slot the user selects
    private void Update(){
        if(Input.inputString != null){
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 8){
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    //Making sure we highlight the correct slot
    void ChangeSelectedSlot(int newValue){
        if(selectedSlot >= 0){
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }
    //When "picking up" items this will search for the closets empty slot
    public bool AddItem(Item item){
        //This for loop is if a stackable item is in the inventory it will add the item to that stack
        for(int i = 0; i < inventorySlots.Count; i++){
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot != null && 
                itemInSlot.item == item && 
                itemInSlot.count < maxStackedItems &&
                itemInSlot.item.stackable){
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        //If there is no stackable item in teh inventory this will find teh closest slot (both hotbar adn backpack)
        for(int i = 0; i < inventorySlots.Count; i++){
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot == null){
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    //once the slot found from above this will add the item to the slot
    void SpawnNewItem(Item item, InventorySlot slot){
        GameObject newItemGo = Instantiate(InventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    //This checks if the item that is currently being used by the player is being used.
    public Item GetSelectedItem(){
        if (selectedSlot < 0 || selectedSlot >= inventorySlots.Count) return null;

        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if(itemInSlot != null){
            return itemInSlot.item;
        }

        return null;
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && 
            itemInSlot.item == item && 
            item.type != Item.ItemType.Tool)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            return;
            }
        }   
    }   

}
