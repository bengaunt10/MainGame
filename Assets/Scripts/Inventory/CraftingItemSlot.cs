using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//inherit from itemSlot

public class CraftingItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{

    public CraftingManager craftingManager;


    public int index;
    public Image iconImage;
    public Image tintOverlay;
    public TMP_Text countText;
    public TMP_Text itemNameText;



    public ItemData item;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private GameObject draggedCloneVisual;
    private Canvas canvas;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();  // Ensure RectTransform reference
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public ItemData GetItem()
    {
        return item; 
    }

    //--------------------------------------------------------------------------------------------------------------
    private void UpdateSlotIfNeeded(PointerEventData eventData)
    {
        // Get the source slot from the event data
        ItemSlot sourceSlot = eventData.pointerDrag?.GetComponent<ItemSlot>();

        if (sourceSlot != null && sourceSlot.isSlotFilled())
        {
            UpdateSlot(sourceSlot.GetItem());

            //notify the crafting manager about the item change
            craftingManager.HandleSlotItemChange();
            sourceSlot.ClearSlot();
        }
    }

    public void UpdateSlot(ItemData itemData)
    {
        if (itemData != null)
        {
            item = itemData;
            iconImage.sprite = itemData.icon;
            countText.text = itemData.count > 1 ? itemData.count.ToString() : ""; // Set item count
            iconImage.enabled = true;
            tintOverlay.enabled = false;

            if (itemData.icon == null)
            {
                itemNameText.text = itemData.itemName; // Show item name
                itemNameText.enabled = true;
            }
            else
            {
                itemNameText.enabled = false;
                iconImage.color = new Color(1.5f, 1.5f, 1.5f, 1.5f);
            }
        }
        else
        {
            ClearSlot(); // Clear the slot if no item is present
        }
    }
    //--------------------------------------------------------------------------------------------------------------

    // Clear the slot when no item is assigned
    public void ClearSlot()
    {

        item = null;
        iconImage.sprite = null;
        iconImage.color = new Color(0f, 0f, 0f, 0f);
        countText.text = "";
        itemNameText.enabled = false;
        tintOverlay.enabled = false;
    }


    //--------------------------------------------------------------------------------------------------------------


    public void SetSelected(bool isSelected)
    {
        tintOverlay.enabled = true;
        tintOverlay.color = new Color(0.6f, 0f, 0.9f, 0.5f);
    }

    public bool IsSlotFilled()
    {
        return !isSlotEmpty();
    }

    public bool isSlotEmpty()
    {
        return item == null;
    }
    //--------------------------------------------------------------------------------------------------------------

    // Start dragging the item
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsSlotFilled())
        {
            // Store the original position
            originalPosition = rectTransform.anchoredPosition;
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;

            // Create a visual copy of the item to follow the mouse
            draggedCloneVisual = Instantiate(gameObject, canvas.transform);
            draggedCloneVisual.GetComponent<CanvasGroup>().alpha = 0.6f;  // Make it semi-transparent
            draggedCloneVisual.GetComponent<RectTransform>().position = rectTransform.position;  // Start at the original position

            // Update the slot with the reduced item count
            if (item.count > 1)
            {
                item.count -= 1;
                UpdateSlot(item);
            }
            else
            {
                // Clear the slot if only one item is picked up
                //ClearSlot();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------------------

    // While dragging the item
    public void OnDrag(PointerEventData eventData)
    {

        // Update the position of the item visual to follow the mouse
        draggedCloneVisual.GetComponent<RectTransform>().position = eventData.position;

    }


    //--------------------------------------------------------------------------------------------------------------

    public void OnEndDrag(PointerEventData eventData)
    {

        Destroy(draggedCloneVisual);

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;


        //If the item wasn't dropped over a valid slot, reset its position
        if (eventData.pointerEnter == null || !IsValidDrop(eventData.pointerEnter))
        {
            rectTransform.anchoredPosition = originalPosition;
            if (item != null && item.count > 0)
            {
                item.count += 1;
                UpdateSlot(item);
            }
            else
            {
                item = null;
                ClearSlot();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------------------
    public virtual void OnDropIgnore(PointerEventData eventData)
    {

        //TODO
        // 1. Destroy dragged clone
        // 2. Update slot item
        // 3. Notify crafting manager for the item change (to validate and create craft item)
        // Validate:
        // 3.1 if valid create craft item
        // 3.2 if not valid: empty the crafting item slot



        Destroy(draggedCloneVisual);



        // Get the source slot from the event data
        ItemSlot sourceSlot = eventData.pointerDrag?.GetComponent<ItemSlot>();



        if (sourceSlot != null && sourceSlot.isSlotFilled())
        {

            UpdateSlot(sourceSlot.GetItem());
            //craftingManager.HandleItemDrop(sourceSlot.item, this);

            //TODO: createCraftItem();




            // Check if both crafting slots are occupied
            //    ItemSlot craftingSlot1 = craftingManager.GetCraftingSlot1();
            //    ItemSlot craftingSlot2 = craftingManager.GetCraftingSlot2();


            //    if (craftingSlot1 == null || craftingSlot2 == null)
            //    {
            //        Debug.LogError("One or both crafting slots are not assigned!");
            //        return;
            //    }


            //    if (craftingSlot1.isSlotEmpty() || craftingSlot2.isSlotEmpty())
            //    {
            //        Debug.Log("Both crafting slots must be occupied before crafting.");
            //        return;
            //    }


            //    bool validCombination = craftingManager.CheckForValidCraftingSlotsCombination(craftingSlot1.item, craftingSlot2.item);

            //    if (validCombination)
            //    {

            //        craftingManager.CreateCraftItem(craftingSlot1.item, craftingSlot2.item);

            //        craftingSlot1.ClearSlot();
            //        craftingSlot2.ClearSlot();


            //        craftingManager.ClearCraftingSlots();
            //        //craftingManager.OnDaggerDroppedIntoInventory();
            //        //inventoryManager.AddItemToInventory(craftingManager.craftedDagger.GetComponent<ItemData>());



            //    }
            //    else
            //    {
            //        // Log that the combination is not valid and return the items
            //        Debug.Log("Invalid combination. Returning items to the inventory.");
            //        sourceSlot.UpdateSlotIfNeeded(sourceSlot.item); // Return the item back to its original slot
            //    }
        }
    }


    //--------------------------------------------------------------------------------------------------------------

    private bool IsValidDrop(GameObject dropTarget)
    {
        // Check if the drop target is a valid slot (e.g., CraftingSlot)
        return dropTarget != null && dropTarget.GetComponent<ItemSlot>() != null;
    }


    public void OnDrop(PointerEventData eventData)

    {

        // 1. Destroy dragged clone
        Destroy(draggedCloneVisual);

        // 2. Update slot item
        UpdateSlotIfNeeded(eventData);

        // 3. Notify crafting manager for the item change (to validate and create craft item)
        craftingManager.HandleSlotItemChange(); 
    }
}