using UnityEngine;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text clarityText;
    [SerializeField] private TMP_Text bulletSpeedText;
    [SerializeField] private TMP_Text fireRateText;
    [SerializeField] private TMP_Text bulletDamageText;
    [SerializeField] private TMP_Text bulletRangeText;
    [SerializeField] private TMP_Text moveSpeedText;

    public void ModifyBulletSpeedText(float bulletSpeed) { bulletSpeedText.text = "Bullet Speed " + bulletSpeed; }

    public void ModifyBulletRangeText(float bulletRange) { bulletRangeText.text = "Range " + bulletRange; }

    public void ModifyPlayerSpeedText(float moveSpeed) { moveSpeedText.text = "Player Speed " + moveSpeed; }

    public void ModifyFireRateText(float fireRate) { fireRateText.text = "FireRate " + fireRate; }

    public void ModifyDamageText(int bulletDamage) { bulletDamageText.text = "Damage " + bulletDamage; }

    public void ModifyHealthText(int currentHealth, int maxHealth) { clarityText.text = "Clarity " + currentHealth + "/" + maxHealth; }
}
