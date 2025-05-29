// House.cs
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
    }

    private void Die()
    {
        isDestroyed = true;
        Debug.Log($"{gameObject.name} 파괴됨!");
        GameManager.Instance?.NotifyHouseDestroyed(this);

        Destroy(gameObject);
    }
}
