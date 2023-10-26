using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] public List<ItemData> _items = new List<ItemData>();
    
    //References
    [SerializeField] InventoryUI _inventoryUI;

    #region Properties
    public List<ItemData> Items { get => _items; }
    #endregion

    private void Awake()
    {
        
    }

    public void Add(ItemData item)
    {
        _items.Add(item);
        _inventoryUI.UpdateInventoryUI();
    }

    public void Remove(ItemData item)
    {
        if (_items.Count > 0) 
        {
            _items.Remove(item);
            _inventoryUI.UpdateInventoryUI();
        }
    }
}
