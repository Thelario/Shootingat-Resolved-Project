using UnityEngine;

namespace PabloLario.Managers
{
    public class ItemsManager : Singleton<ItemsManager>
    {
        Assets a;

        private void Start()
        {
            a = Assets.Instance;
        }

        public GameObject GetRandomItemPrefab()
        {
            int random = Random.Range(0, a.itemsArray.Length);
            return a.itemsArray[random].itemPrefab;
        }

        public GameObject GetItemFromName(string name)
        {
            foreach (Items i in a.itemsArray)
            {
                if (i.itemName == name)
                    return i.itemPrefab;
            }

            Debug.LogError("Item " + name + "Not Found! There might not be any item in the Assets.");
            return null;
        }
    }
}