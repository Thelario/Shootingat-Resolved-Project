using UnityEngine;

namespace PabloLario.Managers
{
    public class AbilityPickupsManager : Singleton<AbilityPickupsManager>
    {
        private Assets _a;

        private void Start()
        {
            _a = Assets.Instance;
        }

        public GameObject GetRandomAbilityPickupPrefab()
        {
            int random = Random.Range(0, _a.pickupsArray.Length);
            return _a.pickupsArray[random].pickupPrefab;
        }
    }
}
