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

    // Start is called before the first frame update
    void Start()
    {
        items = new Stack<Item>();

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

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            UseItem();
        }
    }

    
}
