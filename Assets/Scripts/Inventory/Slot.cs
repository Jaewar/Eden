using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{

    private Stack<Item> items;

    public TextMeshProUGUI stackText;

    public Sprite slotEmpty;
    public Sprite slotHighlight;

    public bool IsEmpty {
        get { return items.Count == 0; }
    }

    public bool IsAvailable {
        get {return CurrentItem.maxSize > items.Count; }
    }

    public Item CurrentItem {
        get { return items.Peek(); }
    }

    public Stack<Item> Items {
        get { return items; }
        set { items = value; }
    }

    void Awake() {
        items = new Stack<Item>();
    }

    // Start is called before the first frame update
    void Start()
    {

        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform textRect = stackText.GetComponent<RectTransform>();

        int textScaleFactor = (int)(slotRect.sizeDelta.x * 0.60);

        stackText.fontSizeMax = textScaleFactor;
        stackText.fontSizeMin = textScaleFactor;

        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(Item item) {
        items.Push(item);

        if (items.Count > 1) {
            stackText.text = items.Count.ToString();
        }

        ChangeSprite(item.spriteNeutral, item.spriteHighlighted);
    }

    public void AddItems(Stack<Item> items) {
        this.items = new Stack<Item>(items);
        stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
        ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);
    }

    private void ChangeSprite(Sprite neutral, Sprite highlight) {

        GetComponent<Image>().sprite = neutral;

        SpriteState st = new SpriteState();

        st.highlightedSprite = highlight;
        st.pressedSprite = neutral;

        GetComponent<Button>().spriteState = st;

    }

    private void UseItem() {
        if (!IsEmpty) {
            items.Pop().Use();

            stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

            if (IsEmpty) {
                ChangeSprite(slotEmpty, slotHighlight);
                Inventory.EmptySlots++;
            }
        }
    }

    public void ClearSlot() {
        items.Clear();
        ChangeSprite(slotEmpty, slotHighlight);
        stackText.text = string.Empty;
    }

    public Stack<Item> RemoveItems(int amount) {
        Stack<Item> tmp = new Stack<Item>();

        for (int i = 0; i < amount; i++) {
            tmp.Push(items.Pop());
        }

        stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

        return tmp;
    }

    public Item RemoveItem() {
        Item tmp;
        tmp = items.Pop();

        stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

        return tmp;
    }

    public void OnPointerClick(PointerEventData eventData) {
        //If the right mousebutton was clicked, and we aren't moving an item and the inventory is visible
        if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") && GameManager.instance.InventoryPanelActive() == true) {
            //Uses an item on the slot
            UseItem();
        }
        //Checks if we need to show the split stack dialog , this is only done if we shiftclick a slot and we aren't moving an item
        else if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift) && !IsEmpty && !GameObject.Find("Hover")) {
            //The dialogs spawnposition
            Vector2 position;

            //Translates the mouse position to onscreen coords so that we can spawn the dialog at the correct position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Inventory.Instance.canvas.transform as RectTransform, Input.mousePosition, Inventory.Instance.canvas.worldCamera, out position);

            //Shows the dialog
            Inventory.Instance.selectStackSize.SetActive(true);

            //Sets the position
            Inventory.Instance.selectStackSize.transform.position = Inventory.Instance.canvas.transform.TransformPoint(position);

            //Tell the inventory the item count on the selected slot
            Inventory.Instance.SetStackInfo(items.Count);
        }
    }

    
}
