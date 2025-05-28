public enum ALSTATE
{
    IDLE,
    WANDERING
}
// 아이템 타입 분류 (자원, 장비, 소비, 건축물)
public enum ITEMTYPE 
{ 
    RESOURCE,
    EQUIPABLE,
    CONSUMABLE,
    BUILDING
}
// 소비 아이템 효과의 타입 (배고픔, 목마름, 경험치)
public enum CONSUMABLETYPE 
{ 
    HUNGER,
    THIRST,
    EXP
}