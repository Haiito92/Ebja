using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    ItemData _itemData;

    //References
    [SerializeField] Image _image;

    #region Properties
    public ItemData ItemData { get => _itemData; set => _itemData = value; }
    public Image Image { get => _image; set => _image = value; }
    #endregion

    public void AddInSlot(ItemData itemData)
    {
        if( itemData != null) 
        { 
            _itemData = itemData;
            _image.sprite = _itemData.Sprite;
        }
    }
}
