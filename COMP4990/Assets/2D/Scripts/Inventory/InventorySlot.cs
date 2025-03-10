using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
/*
    Inventory Slot script. Crates, colours, and sets the inventory slots accordingly
*/
public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;

    private void Awake(){
        Deselect();
    }
    public void Select(){
        image.color = selectedColor;
    }

    public void Deselect(){
        image.color = notSelectedColor;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0){
            Debug.Log($"On drop {transform.name}");
            GameObject dropped = eventData.pointerDrag;
            InventoryItem drag = dropped.GetComponent<InventoryItem>();
            drag.parentAfterDrag = transform;
        }
    }
}