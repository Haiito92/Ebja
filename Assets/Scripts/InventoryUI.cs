using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject _slotsParent;
    [SerializeField] GameObject _slotPrefab;

    [SerializeField] List<ItemSlot> _slots = new List<ItemSlot>();

    //References
    [SerializeField] Inventory _inventory;

    public void UpdateInventoryUI()
    {
        if (_slots.Count < _inventory.Items.Count)
        {
            for (int i = _slots.Count; i < _inventory.Items.Count; i++)
            {
                _slots.Add(Instantiate(_slotPrefab, _slotsParent.transform).GetComponent<ItemSlot>());
            }
        }
        for (int i = 0; i < _inventory.Items.Count; i++)
        {
            _slots[i].AddInSlot(_inventory.Items[i]);
        }
        if (_slots.Count > _inventory.Items.Count)
        {
            for (int i = _slots.Count; i > _inventory.Items.Count; i--)
            {
                Destroy(_slots[i -1].gameObject);
                _slots.RemoveAt(i -1);
            }
        }
    }
}
