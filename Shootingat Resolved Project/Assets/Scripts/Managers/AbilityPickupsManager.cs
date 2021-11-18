using UnityEngine;

namespace PabloLario.Managers
{
    public class AbilityPickupsManager : Singleton<AbilityPickupsManager>
    {
        private Assets a;

        private void Start()
        {
            a = Assets.Instance;
        }

        public GameObject GetRandomAbilityPickupPrefab()
        {
            int random = Random.Range(0, a.pickupsArray.Length);
            return a.pickupsArray[random].pickupPrefab;
        }
    }
}
