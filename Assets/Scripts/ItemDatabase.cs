using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Item Database", fileName = "ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] List<ItemData> _datas = new List<ItemData>();

    #region Properties
    public List<ItemData> Datas { get => _datas; set => _datas = value; }
    #endregion
}
