public enum ALSTATE
{
    IDLE,
    WANDERING
}
// 소비 아이템 효과의 타입 (허기 회복, 체력 회복 등)
public enum CONSUMABLETYPE 
{ 
    HUNGER,
    HEALTH
}
// 아이템 타입 분류 (자원형, 장비형, 소비형 등)
public enum ITEMTYPE 
{ 
    RESOURCE,
    EQUIPABLE,
    CONSUMABLE
}