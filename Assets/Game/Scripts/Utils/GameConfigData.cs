using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigData : Singleton<GameConfigData>
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    [BoxGroup("Setting Character Starter")]
    public int StartAttack = 3;
    [BoxGroup("Setting Character Starter")]
    public int StartHp = 100;
    [BoxGroup("Setting Character Starter")]
    public int StartDefense = 0;
    [BoxGroup("Setting Character Starter")]
    public float StartMoveSpeed = 2;
    [BoxGroup("Setting Character Starter")]
    [Range(0,100)]
    public int StartCriticalRate = 0;
    [BoxGroup("Setting Character Starter")]
    public float StartCriticalDamage = 1.5f;
    [BoxGroup("Setting Character Starter")]
    public SkillName DefaultSkill = SkillName.PISTOL;
    [BoxGroup("Setting Character Starter")]
    public SkillName[] TestSkills = new SkillName[1] { SkillName.NONE};

    [BoxGroup("Setting Shop")]
    public long PriceDesign = 1000;

    [BoxGroup("Setting Ingame")]
    public int MaxLevel = 99;
    [BoxGroup("Setting Ingame")]
    public int[] ExpLevel = new int[99]
    {
        5,
        15,
        30,
        50,
        70,
        95,
        120,
        145,
        170,
        190,
        215,
        240,
        265,
        295,
        325,
        355,
        385,
        420,
        455,
        490,
        525,
        565,
        605,
        645,
        685,
        725,
        775,
        825,
        875,
        925,
        995,
        1065,
        1135,
        1205,
        1275,
        1355,
        1435,
        1515,
        1595,
        1675,
        1810,
        1945,
        2080,
        2215,
        2350,
        2485,
        2620,
        2755,
        2890,
        3025,
        3225,
        3425,
        3625,
        3825,
        4025,
        4225,
        4425,
        4625,
        4825,
        5025,
        5275,
        5525,
        5775,
        6025,
        6275,
        6525,
        6775,
        7025,
        7275,
        7525,
        7835,
        8145,
        8455,
        8765,
        9075,
        9385,
        9695,
        10005,
        10315,
        10625,
        11045,
        11465,
        11885,
        12305,
        12725,
        13145,
        13565,
        13985,
        14405,
        14825,
        15425,
        16025,
        16625,
        17225,
        17995,
        18765,
        19535,
        20305,
        21075,
    };
    [BoxGroup("Setting Ingame")]
    public int MultiplyExp = 10;

    [BoxGroup("Setting Ingame Item")]
    public int PackageGold = 1000;
    [BoxGroup("Setting Ingame Item")]
    public int PackageGold2 = 3000;
    [BoxGroup("Setting Ingame Item")]
    public int PackageGold3 = 9000;
    [BoxGroup("Setting Ingame Item")]
    public int BombMultiDamage = 50;

    [BoxGroup("Setting Game")]
    public int MaxChapter = 3;
    [BoxGroup("Setting Game")]
    public int TimeRecoverStamina = 5;
    [BoxGroup("Setting Game")]
    public int MaxStamina = 30;
    [BoxGroup("Setting Game")]
    public int UserLevelupRewardGem = 200;
    [BoxGroup("Setting Game")]
    public int TimeMineStaminaMinutes = 10;

    [BoxGroup("Setting Player Starter")]
    public int StartAccountLevel = 1;
    [BoxGroup("Setting Player Starter")]
    public long StartAccountGold = 1000000;
    [BoxGroup("Setting Player Starter")]
    public long StartAccountGem = 10000;
    [BoxGroup("Setting Player Starter")]
    public int StartChapterLevel = 1;
    [BoxGroup("Setting Patrol")]
    public int PatrolStartLevel = 2;
    [BoxGroup("Setting Patrol")]
    
    public int PatrolFirstRewardTimeMinutes = 480;
    [BoxGroup("Setting Patrol")]

    public int PatrolQuickPatrolTimeMinutes = 600;
    [BoxGroup("Setting Patrol")]

    public int PatrolMaxAfkTimeMinutes = 1000;
}
