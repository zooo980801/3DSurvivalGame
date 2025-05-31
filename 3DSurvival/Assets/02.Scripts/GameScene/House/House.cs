using UnityEngine;

public class House : MonoBehaviour
{
    public string prefabId = "house_basic";
    public float maxHP = 100f;
    [SerializeField] private float currentHP;
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
                alarm.Show("집이 공격받고 있습니다!");
        }
    }

    private void Die()
    {
        isDestroyed = true;
        Debug.Log($"{gameObject.name} 파괴됨!");
        GameManager.Instance?.NotifyHouseDestroyed(this);

        Destroy(gameObject);
    }
    public void WriteSave(SavedHouse data)
    {
        data.prefabId = prefabId;
        data.hp = currentHP;
        data.posX = transform.position.x;
        data.posY = transform.position.y;
        data.posZ = transform.position.z;
        data.isDestroyed = isDestroyed;
    }

    public void LoadFromSave(SavedHouse data)
    {
        transform.position = new Vector3(data.posX, data.posY, data.posZ);
        currentHP = data.hp;
        isDestroyed = data.isDestroyed;

        if (isDestroyed)
        {
            Destroy(gameObject); // 이미 파괴된 경우 복원 안 함
        }
    }
}
