using UnityEngine;

public class DropItem : MonoBehaviour
{
    public Item itemType;    
    public int amount = 1;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var inventory = other.GetComponent<PlayerCommonInventory>();
            if (inventory != null)
            {
                if (inventory.AddItem(itemType, amount))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}