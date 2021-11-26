using PabloLario.Characters.Core.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PabloLario
{
    public class DamageBossTest : MonoBehaviour
    {
        public BossStats boss;
        
        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                boss.GetComponent<IDamageable>().TakeDamage(1);
            }
        }
    }
}
