using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    HEALTH, AMMO
};

public class Item : MonoBehaviour
{

    public ItemType type;

    public Sprite spriteNeutral;
    public Sprite spriteHighlighted;

    public int maxSize;

    public void Use() {
        switch(type) {
            case ItemType.AMMO:
                Debug.Log("ammo");
                break;
            case ItemType.HEALTH:
                Debug.Log("health");
                break;
            default:
                Debug.Log("Unknown");
                break;
        }
    }
}
