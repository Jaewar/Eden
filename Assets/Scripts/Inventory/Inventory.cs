using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    private static Inventory instance;

    public static Inventory Instance {
        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<Inventory>();
            }
            return Inventory.instance;
        }
    }

    private RectTransform inventoryRect;

    private float inventoryWidth, inventoryHeight;

    public int slots;
    public int rows;

    public float slotPaddingLeft, slotPaddingTop;

    public float slotSize;

    public GameObject slotPrefab;

    private static Slot from, to;

    private List<GameObject> allSlots;

    public GameObject iconPrefab;

    private static GameObject hoverObject;

    private static int emptySlots;

    public Canvas canvas;

    public EventSystem eventSystem;

    private static GameObject clicked;

    #region StackSplit
    /// <summary>
    /// References for stack splitting/moving
    /// </summary>
    public GameObject selectStackSize;
    public TextMeshProUGUI stackText;

    private int splitAmount;
    private int maxStackCount;
    private static Slot movingSlot;
    #endregion

    public static int EmptySlots { get => emptySlots; set => emptySlots = value; }

    // Start is called before the first frame update
    void Start()
    {
        CreateInventoryLayout();

        movingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0)) {
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null) {
                from.GetComponent<Image>().color = Color.white;
                from.ClearSlot();
                Destroy(GameObject.Find("Hover"));

                to = null;
                from = null;
                emptySlots++;
            } else if (!eventSystem.IsPointerOverGameObject(-1) && !movingSlot.IsEmpty) {
                movingSlot.ClearSlot();
                Destroy(GameObject.Find("Hover"));
            }

        }

        if (hoverObject != null) {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            position.Set(position.x, position.y - 5);
            hoverObject.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    private void CreateInventoryLayout() {
        allSlots = new List<GameObject>();

        emptySlots = slots;

        // creating width and height of the inventory background dynamically based on size.
        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
        inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        inventoryRect = GetComponent<RectTransform>();

        // setting the size of the inventory.
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

        int columns = slots / rows;

        for (int y = 0; y < rows; y++) {
            for (int x = 0; x < columns; x++) {
                // creating object prefab for each slot that is available.
                GameObject newSlot = Instantiate(slotPrefab);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                // setting new slot as child of Inventory Object
                newSlot.name = "Slot";
                newSlot.transform.SetParent(this.transform.parent);

                // setting slot position to correct value based on inventory size;
                slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));

                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                newSlot.transform.SetParent(this.transform);

                allSlots.Add(newSlot);
            }
        }
    }

    public bool AddItem(Item item) {
        if (item.maxSize == 1) {
            PlaceEmpty(item);
            return true;
        } else {
            foreach (GameObject slot in allSlots) {
                Slot tmp = slot.GetComponent<Slot>();
                if (!tmp.IsEmpty) {
                    if (tmp.CurrentItem.type == item.type && tmp.IsAvailable) {
                        if (!movingSlot.IsEmpty && clicked.GetComponent<Slot>() == tmp.GetComponent<Slot>()) {
                            continue;
                        } else {

                            tmp.AddItem(item); //Adds the item to the inventory
                            return true;
                        }
                    }
                }
            }
            if (emptySlots > 0) {
                PlaceEmpty(item);
            }
        }

        return false;
    }

    private void MoveInventory() {

    }

    private bool PlaceEmpty(Item item) {
        if (emptySlots > 0) {
            foreach (GameObject slot in allSlots) {
                Slot tmp = slot.GetComponent<Slot>();

                if (tmp.IsEmpty) {
                    tmp.AddItem(item);
                    emptySlots--;
                    return true;
                }
            }
        }
        return false;
    }

    public void MoveItem(GameObject clicked) 
    {
        Inventory.clicked = clicked;

        if (!movingSlot.IsEmpty) {
            Slot tmp = clicked.GetComponent<Slot>();

            if (tmp.IsEmpty) {
                tmp.AddItems(movingSlot.Items);
                movingSlot.Items.Clear();
                Destroy(GameObject.Find("Hover"));

            } else if (!tmp.IsEmpty && movingSlot.CurrentItem.type == tmp.CurrentItem.type && tmp.IsAvailable) {
                MergeStacks(movingSlot, tmp);
            }
        }
        else if (from == null && GameManager.instance.InventoryPanelActive() == true && !Input.GetKey(KeyCode.LeftShift)) 
        {
            if (!clicked.GetComponent<Slot>().IsEmpty && !GameObject.Find("Hover")) {
                from = clicked.GetComponent<Slot>();
                from.GetComponent<Image>().color = Color.gray;

                CreateHoverIcon();
            }
        } 
        else if (to == null && !Input.GetKey(KeyCode.LeftShift)) 
        {
            to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }
        if (to != null && from != null) {
            if (!to.IsEmpty && from.CurrentItem.type == to.CurrentItem.type && to.IsAvailable) {
                MergeStacks(from, to);
            } else {
                Stack<Item> tmpTo = new Stack<Item>(to.Items);
                to.AddItems(from.Items);


                if (tmpTo.Count == 0) {
                    from.ClearSlot();
                } else {
                    from.AddItems(tmpTo);
                }
            }

            from.GetComponent<Image>().color = Color.white;
            to = null;
            from = null;
            Destroy(GameObject.Find("Hover"));
        }
    }

    private void CreateHoverIcon() {
        hoverObject = (GameObject)Instantiate(iconPrefab); //Instantiates the hover object 

        hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite; //Sets the sprite on the hover object so that it reflects the object we are moing

        hoverObject.name = "Hover"; //Sets the name of the hover object
        //Creates references to the transforms
        RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
        RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

        ///Sets the size of the hoverobject so that it has the same size as the clicked object
        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

        //Sets the hoverobject's parent as the canvas, so that it is visible in the game
        hoverObject.transform.SetParent(GameObject.Find("Inventory Canvas").transform, true);

        //Sets the local scale to make usre that it has the correct size
        hoverObject.transform.localScale = clicked.gameObject.transform.localScale;

        hoverObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = movingSlot.Items.Count > 1 ? movingSlot.Items.Count.ToString() : string.Empty;
    }

    public void PutItemBack() {
        if (from != null) {
            Destroy(GameObject.Find("Hover"));
            from.GetComponent<Image>().color = Color.white;
            from = null;
        } else if (!movingSlot.IsEmpty) {
            Destroy(GameObject.Find("Hover"));

            //Puts the items back one by one
            foreach (Item item in movingSlot.Items) {
                clicked.GetComponent<Slot>().AddItem(item);
            }

            movingSlot.ClearSlot(); //Makes sure that the moving slot is empty
        }
        selectStackSize.SetActive(false);
    }

    public void SetStackInfo(int maxStackCount) {
        selectStackSize.SetActive(true);
        splitAmount = 0;
        this.maxStackCount = maxStackCount;
        stackText.text = splitAmount.ToString();
    }

    public void MergeStacks(Slot source, Slot destination) {
        int max = destination.CurrentItem.maxSize - destination.Items.Count;

        int count = source.Items.Count < max ? source.Items.Count : max;

        for (int i = 0; i < count; i++) {
            destination.AddItem(source.RemoveItem());
            hoverObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = movingSlot.Items.Count.ToString();

        }
        if (source.Items.Count == 0) {
            source.ClearSlot();
            Destroy(GameObject.Find("Hover"));
        }
    }

    public void ChangeStackText(int i) {
        splitAmount += i;

        if (splitAmount < 0) {
            splitAmount = 0;
        }
        if (splitAmount > maxStackCount) {
            splitAmount = maxStackCount;
        }
        stackText.text = splitAmount.ToString();
    }

    public void SplitStack() {
        selectStackSize.SetActive(false);
        if (splitAmount == maxStackCount) {
            MoveItem(clicked);
        } else if (splitAmount > 0) {
            movingSlot.Items = clicked.GetComponent<Slot>().RemoveItems(splitAmount);

            CreateHoverIcon();
        }
    }
}
