using System;
using UnityEngine;

[Serializable]
public class ItemData
{
    [SerializeField] string _name;
    [SerializeField] string _description;
    [SerializeField] Sprite _sprite;

    #region Properties
    public string Name { get => _name; }
    public string Description { get => _description; }
    public Sprite Sprite { get => _sprite; }
    #endregion

    public void Init(ItemData itemData)
    {
        _name = itemData.Name;
        _description = itemData.Description;
        _sprite = itemData.Sprite;
    }
}
