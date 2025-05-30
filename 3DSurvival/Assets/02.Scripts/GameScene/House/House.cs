using UnityEngine;

public class House : MonoBehaviour
{
    public float maxHP = 100f;
    private float currentHP;
    private bool isDestroyed = false;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float amount)
    {
        if (isDestroyed) return;

        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        if (currentHP <= 0f)
        {
            Die();
        }

        if (amount > 0f)
        {
            AlarmUI alarm = FindObjectOfType<AlarmUI>();
            if (alarm != null)
                alarm.Show("🏠 집이 공격받고 있습니다!");
        }
    }

    private void Die()
    {
        isDestroyed = true;
        Debug.Log($"{gameObject.name} 파괴됨!");
        GameManager.Instance?.NotifyHouseDestroyed(this);

        Destroy(gameObject);
    }
}
