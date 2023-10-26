using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //////////////////////
    /// Test Variables ///
    //////////////////////

    [SerializeField] Inventory inventory;

    //////////////////////
    /// Test Functions ///
    //////////////////////

    public void SpawnItem(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            ItemData item = new ItemData();
            int index = Random.Range(0, DatabaseManager.Instance.ItemDatabase.Datas.Count);
            item.Init(DatabaseManager.Instance.ItemDatabase.Datas[index]);
            inventory.Add(item);
        }
    }

    public void RemoveItem(InputAction.CallbackContext ctx)
    {
        if (ctx.started && inventory.Items.Count > 0)
        {
            inventory.Remove(inventory.Items[0]);
        }
    }
}
