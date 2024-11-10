using EnhancedUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStaticData
{
    public static string[] ITEM_EXP_TYPE = new string[3] { StringConst.TAG_EXP, StringConst.TAG_EXP2, StringConst.TAG_EXP3 };
    public static Currency[] CurrencyInBag = new Currency[]
    {
        Currency.DESIGN_ARMOR,
        Currency.DESIGN_BELT,
        Currency.DESIGN_BOOTS,
        Currency.DESIGN_GLOVES,
        Currency.DESIGN_HELMET,
        Currency.DESIGN_WEAPON,
        Currency.TICKET_GACHA_BASIC,
        Currency.TICKET_GACHA_PREMIUM,
        Currency.TICKET_GACHA_SUPER,
    };
    public static List<SkillName> SkillWeapons = new List<SkillName>
    {
        SkillName.PISTOL,
        SkillName.SWORD,
        SkillName.L_KATANA,
        SkillName.KNIFE,
        SkillName.CLAW,
    };

}
