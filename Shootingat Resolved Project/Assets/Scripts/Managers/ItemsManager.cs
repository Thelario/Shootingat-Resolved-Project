using UnityEngine;

namespace PabloLario.Managers
{
    public class ItemsManager : Singleton<ItemsManager>
    {
        private Assets _a;

        private void Start()
        {
            _a = Assets.Instance;
        }

        public GameObject GetRandomItemPrefab()
        {
            int random = Random.Range(0, _a.itemsArray.Length);
            return _a.itemsArray[random].itemPrefab;
        }

        public GameObject GetItemFromName(string nName)
        {
            foreach (Items i in _a.itemsArray)
            {
                if (i.itemName == nName)
                    return i.itemPrefab;
            }

            Debug.LogError("Item " + nName + "Not Found! There might not be any item in the Assets.");
            return null;
        }
    }
}