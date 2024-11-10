using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameMode
{
    NONE = 0,
    CAMPAIGN = 1,
    DAILY_QUEST = 2,
    CHALLENGES = 3
}
public enum ChallengeLevel
{
    NONE,
    HARD,
    VERY_HARD,
    HELL
}
public enum EnemyBehaviorState
{
    NONE =0,
    START_LONG_ACTION = 1,
    SHORT_ACTION = 2,
    END_LONG_ACTION = 3,
    FINISH = 4
}
public enum SortInventoryType
{
    NONE = 0,
    ENCHANT_LEVEL = 1,
    TYPE_EQUIPMENT = 2,
    RARITY = 3
}

public enum MergeType
{
    NONE,
    SAME_ITEM_AND_RARITY,
    ONLY_SAME_RARITY,
    SAME_TYPE_AND_RARITY
}
public enum SlotMergeType
{
    NONE,
    MAIN,
    SUB_1,
    SUB_2
}
public enum SkillType
{
    NONE,
    PASSIVE,
    ACTIVE
}
public enum GradeSkill
{
    NONE = 0,
    INCREASE_HP = 1,
    INCREASE_ATK = 2,
    INCREASE_CRIT_RATE = 3,
    INCREASE_CRIT_DAMAGE = 4,
    INCREASE_DEFENSE = 5,
    INCREASE_MOVE_SPEED = 6,
    INCREASE_COOL_DOWN = 7,

    LEARN_ONE_SKILL = 50,
}
public enum BuffType
{
    NONE,
    MAGNET,
    CRITICAL_RATE,
    ATTACK_DAMAGE,
    BULLET_SPEED,
    BULLET_SCALE,
    CHARACTER_SPEED,
    HIT_POINT_MAX,
    RENGEN_PER_SECONDS,
    REDUCE_TIME_CD,
    HEAL,
    REDUCE_DAMAGE,
    EXP,
    EXTRA_TIME_SKILL,
    CRITICAL_DAMAGE,
    GOLD
}
public enum SkillName
{
    NONE,

    PISTOL = 1,

    CHAIN_LIGHTNING = 2,
    BALL = 3,
    DRILL = 4,
    DANCING_SWORD = 5,
    GRAVITY_FIELD = 6,
    GRENADE_LAUNCHER = 7,
    BOOMERANG = 8,
    BOTTLE_GAS = 9,
    KNIFE = 10,

    BRICKS = 13,
    SHOTGUN = 14,
    SWORD = 15,
    L_KATANA = 16,
    YOYO = 17,

    //skill support
    EXP_BONUS = 51,
    DAMAGE_BONUS = 52,
    BULLET_SPEED_BONUS = 53,
    BULLET_RANGE_BONUS = 54,
    HP_MAX_BONUS = 55,
    RENGEN_BONUS = 56,
    COOLDOWN_BONUS = 57,
    GOLD_BONUS = 58,
    MAGNET_RANGE_BONUS = 59,
    DEFENSE_BONUS = 60,
    ARM_ARMOR = 61,
    SNEAKERS = 62,


    //Skill Upgrade
    DEFAULT_MEAT = 100,
    PISTOL_S = 101,
    KNIFE_S = 102,

    CHAIN_LIGHTNING_S = 104,
    BALL_S = 105,
    DRILL_S = 106,
    DANCING_SWORD_S = 107,
    GRAVITY_FIELD_S = 108,
    GRENADE_LAUNCHER_S = 109,
    BOOMERANG_S = 110,
    BOTTLE_GAS_S = 111,
    BRICKS_S = 112,
    SHOTGUN_S = 113,
    SWORD_S = 114,
    YOYO_S = 115,
    SONIC_BOOM = 116,
    SONIC_BOOM_S = 117,
    STORM = 118,
    STORM_S = 119,
    LASER = 120,
    LASER_S = 121,
    RAIN_ICE = 122,
    RAIN_FIRE = 123,
    MEGA_METEOR = 124,
    BALISTA = 125,
    BALISTA_S = 126,
    CLAW = 127,
    CLAW_S = 128,
    LASER_S_2 = 129,
    STORM_S_TICK = 130,
    FIRE_BALL = 131,
	L_KATANA_S = 132,
}

public enum GroupSkill
{
    NONE = 0,
    PASSIVE = 1,
    ACTIVE = 2
}
public enum DropItem
{
    NONE,
    EXP,
    MEAT,
    BOMB,
    MAGNET,
    GOLD
}
public enum OpenPopupAnim
{
    NONE,
    ZOOM_IN
}
public enum ClosePopupAnim
{
    NONE,
    ZOOM_OUT
}
public enum EdgeScreenType
{
    NONE,
    TOP,
        LEFT,
        RIGHT,
        BOTTOM
}
public enum ItemType
{
    NONE,
    WEAPON = 1,
    GLOVES = 2,
    HELMET = 3,
    ARMOR = 4,
    BOOTS = 5,
    BELT = 6,
}
public enum MaterialType
{
    NONE,
    DESIGN_MATERIAL,
    UPGRADE_MATERIAL
}

public enum Rarity
{
    Common = 0,
    Uncommon = 1,
    Rare = 2,
    Elite = 3,
    Elite2 = 4,
    Elite3 = 5,
    Legendary = 6,
    Legendary2 = 7,
    Legendary3 = 8,
    Legendary4 = 9,
    Mythical = 10
}
public enum GachaType
{
    NONE = 0,
    COMMON = 1,
    PREMIUM = 2,
	BANNER = 3
}
public enum Currency
{
    NONE = 0,
    GOLD = 1,
    GEM = 2,
    VID = 3,
    ACC_EXP = 4,
    REVIVE = 5,
    STAMINA = 6,

    DESIGN_WEAPON = 80,
    DESIGN_GLOVES = 81,
    DESIGN_HELMET = 82,
    DESIGN_ARMOR = 83,
    DESIGN_BOOTS = 84,
    DESIGN_BELT = 85,
    RANDOM_DESIGN = 86,
    RANDOM_EQUIPMENT = 87,

    TICKET_GACHA_BASIC = 100,
    TICKET_GACHA_PREMIUM = 101,
    TICKET_GACHA_SUPER = 102,

    TOKEN_CHALLENGE = 200,

    CASH = 300
}

public enum PopupType
{
    NONE,
    SKILL,

    //Home Popup
    SHOP,
    HOME,
    BAG,
    EQUIP_INFOR,
    ITEM_INFOR,
    MERGE,
    CAMPAIGN,
    TRAILS,
    TALENTS,
    OPEN_GACHA_EQUIPMENT,
    SELECT_CHAPTER,
    PATROL,
    GROUP_ITEM_REWARD,
    BUY_STAMINA,

    //InGame Popup
    LEARN_SKILL = 50,
    GACHA_UI = 51,
    REVIVE = 52,
    EndGamePlayScene = 53,
    PAUSE = 54,
    WARNING = 55,
    GACHA_UI_RV = 56
}