using NaughtyAttributes;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    [SerializeField, Expandable] ItemDatabase _itemDatabase;

    static DatabaseManager _instance;
    public static DatabaseManager Instance => _instance;

    #region Properties
    public ItemDatabase ItemDatabase { get => _itemDatabase; }
    #endregion

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
