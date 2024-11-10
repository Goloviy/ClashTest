using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnerMananger : Singleton<SpawnerMananger>
{
    [SerializeField] GameObject prefabBirdCage;
    /// <summary>
    /// If level not contain data drop box . Defaul drop this item
    /// </summary>
    [SerializeField] DropBox[] dropBoxDefault;
    //MonsterContainerData curMonsterContainer;
    WaveMonsterData curWaveData = null;
    ChapterLevelData curChapterData;
    float lastTimeSpawn;
    public bool IsCombatBoss { get; private set; } = false;
    /// <summary>
    /// Monster only spawn after Player appear
    /// </summary>
    public GameObject mainCharacter;
    int miniBossCounter = 0;
    int bossCounter = 0;

    float bossTrackerInvertal = 1f;
    float lastTImeTrackBoss = 1f;
    float lastTImeTrackMiniBoss = 1f;

    /// <summary>
    /// Dictionary luu lai attack cua moi loai quai vat
    /// </summary>
    Dictionary<int, int> dictUnitAttack;

    bool isInit = false;
    bool isTimeout = false;
    private void Start()
    {
        dictUnitAttack = new Dictionary<int, int>();
        if (GameDynamicData.CurGameMode == GameMode.CAMPAIGN)
        {
            curChapterData = GameData.Instance.staticData.GetChapterLevel(GameDynamicData.SelectChapterLevel);
            if (curChapterData == null)
            {
                curChapterData = GameData.Instance.staticData.GetChapterLevel(GameConfigData.Instance.StartChapterLevel);
            }
            isInit = true;
        }
        else
        {

        }
    }
    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.BOSS_DIE, OnBossDie);
    }
    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.BOSS_DIE, OnBossDie);
    }
    public int GetAttack(int monsterId)
    {
        int attack = 1;
        if (dictUnitAttack.TryGetValue(monsterId, out attack))
        {
            return attack;
        }
        else
        {
            DebugCustom.Log("Dict Monster Spawn not available :" + monsterId) ;
            return attack;
        }
    }

    public DropBox[] GetDropBox(int idMonster)
    {
        if (curWaveData != null)
        {
            foreach (var _prefab in curWaveData.containers)
            {
                if (_prefab.monsterSetting.idMonster.Equals(idMonster))
                {
                    return _prefab.monsterSetting.dropBox;
                }
            }
        }
        if (curChapterData != null)
        {
            foreach (var miniBoss in curChapterData.miniBoss)
            {
                if (miniBoss.idMonster.Equals(idMonster))
                {
                    return miniBoss.dropBox;
                }
            }
        }
        if (curChapterData != null)
        {
            foreach (var boss in curChapterData.listBoss)
            {
                if (boss.idBoss.Equals(idMonster))
                {
                    return boss.dropBox;
                }
            }
        }
        return dropBoxDefault;
    }
    private void Timeout()
    {
        if (isTimeout)
        {
            return;
        }
        isTimeout = true;
        PoolManager.Pools[StringConst.POOL_OPPONENT_NAME].DespawnAll();
        PoolManager.Pools[StringConst.POOL_EXP_NAME].DespawnAll();
        if (GameDynamicData.CurGameMode == GameMode.CAMPAIGN)
        {
            EventDispatcher.Instance.PostEvent(EventID.CAMPAIGN_FINISH);
        }
        else
        {

        }
    }
    private void Update()
    {
        if (!isInit)
        {
            return;
        }
        if (InGameManager.Instance.TotalTimePlay - 1 > curChapterData.timePlay)
        {
            if (!isTimeout)
            {
                Timeout();
            }

            return;
        }
        if (mainCharacter == null)
        {
            mainCharacter = GameObject.FindGameObjectWithTag(StringConst.TAG_PLAYER);
            return;
        }
        BigBossTracker();
        if (!IsCombatBoss)
        {

            MiniBossTracker();
            WaveUpdate();
            WaveSpawn();
        }

    }
    #region boss spawn
    private void MiniBossTracker()
    {
        if (InGameManager.Instance.TotalTimePlay < lastTImeTrackMiniBoss + bossTrackerInvertal)
        {
            return;
        }
        else
        {
            lastTImeTrackMiniBoss = InGameManager.Instance.TotalTimePlay;
        }
        //call mini boss sequence , one by one 
        for (int i = 0; i < curChapterData.miniBoss.Length; i++)
        {
            if (i == miniBossCounter)
            {
                var miniBoss = curChapterData.miniBoss[i];
                if (InGameManager.Instance.TotalTimePlay >= miniBoss.timeSpawn)
                {
                    SpawnMiniBoss(miniBoss);
                    miniBossCounter++;
                }
            }

        }
    }


    private void BigBossTracker()
    {
        if (IsCombatBoss)
        {
            return;
        }
        //cd check
        if (InGameManager.Instance.TotalTimePlay < lastTImeTrackBoss + bossTrackerInvertal)
        {
            return;
        }
        else
        {
            lastTImeTrackBoss = InGameManager.Instance.TotalTimePlay;
        }
        //call boss sequence , one by one 
        for (int i = 0; i < curChapterData.listBoss.Length; i++)
        {
            if (i == bossCounter)
            {
                var boss = curChapterData.listBoss[i];
                if (InGameManager.Instance.TotalTimePlay >= boss.timeSpawn)
                {
                    bossCounter++;
                    IsCombatBoss = true;
                    SoundController.Instance.PlaySound(SOUND_TYPE.ALERT);
                    EventDispatcher.Instance.PostEvent(EventID.WARNING_BOSS_ASSAULT);
                    this.StartDelayAction(4f, () =>
                    {
                        PoolManager.Pools[StringConst.POOL_OPPONENT_NAME].DespawnAll();
                        SpawnBoss(boss);
                        EventDispatcher.Instance.PostEvent(EventID.BOSS_APPEAR);
                    });
                }
            }
        }
    }

    

    private void SpawnBoss(BossContainer bossContainer)
    {
        UpdateEnemyAttack(bossContainer.idBoss, bossContainer.atk);
        var charPos = mainCharacter.transform.position;
        var birdCagePos = charPos + Vector3.up * 3f;
        var birdCage = Instantiate(prefabBirdCage, birdCagePos, Quaternion.identity);
        GameDynamicData.atkMonster = bossContainer.atk;
        GameDynamicData.hpMonster = bossContainer.hp;
        GameDynamicData.mSpeedMonster = bossContainer.mSpeed;
        var originScale = bossContainer.PrefabBoss.transform.localScale;
        var tf = PoolManager.Pools[StringConst.POOL_OPPONENT_NAME].Spawn(bossContainer.PrefabBoss, birdCagePos, Quaternion.identity);
        //var tf = Instantiate(bossContainer.PrefabBoss, birdCagePos, Quaternion.identity).transform;
        tf.localScale = originScale * bossContainer.sizeMultiply;

    }
    private void OnBossDie(Component arg1, object arg2)
    {
        IsCombatBoss = false;
    }
    #endregion boss spawn
    private void WaveUpdate()
    {
        float timePlay = InGameManager.Instance.TotalTimePlay;
        if (curWaveData != null && curWaveData.timeEnd > timePlay)
        {
            return;
        }
        else
        {
            foreach (var waveData in curChapterData.waves)
            {
                if (timePlay >= waveData.timeStart && timePlay <= waveData.timeEnd)
                {
                    EventDispatcher.Instance.PostEvent(EventID.CHANGE_WAVE_MONSTER);

                    DebugCustom.Log("Change Wave ");
                    curWaveData = waveData;
                    if (curWaveData.isWarning)
                    {
                        SoundController.Instance.PlaySound(SOUND_TYPE.ALERT);
                        EventDispatcher.Instance.PostEvent(EventID.WARNING_SUDDEN_ATTACK);
                    }
                    //lastTimeSpawn = timePlay;
                    UpdateMonstersAttack();
                    return;
                }
            }
            curWaveData = null;
        }

    }
    private void UpdateEnemyAttack(int idMonster, int damage)
    {
        DebugCustom.LogFormat("idMonster {0}, damage {1}", idMonster, damage);
        //int idMonster = curWaveData.monsters[i].idMonster;
        //int levelMonster = curWaveData.monsters[i].level;
        //var enemy = GameData.Instance.staticData.enemyData.GetEnemyData(idMonster);
        //var eDamage = CharacterStatusHelper.CalculateMonsterDamage(enemy.unitStatus.Attack, levelMonster);
        int _damage;
        if (dictUnitAttack.TryGetValue(idMonster, out _damage))
        {
            //update new stat attack 
            dictUnitAttack[idMonster] = damage;
        }
        else
        {
            //add new monster attack
            dictUnitAttack.Add(idMonster, damage);
        }
    }
    private void UpdateMonstersAttack()
    {
        if (curWaveData != null)
        {
            for (int i = 0; i < curWaveData.containers.Length; i++)
            {
                int idMonster = curWaveData.containers[i].monsterSetting.idMonster;
                //int levelMonster = curWaveData.monsters[i].level;
                UpdateEnemyAttack(idMonster, curWaveData.containers[i].monsterSetting.atk);
            }
        }
    }
    private void WaveSpawn()
    {
        if (curWaveData != null)
        {
            if (InGameManager.Instance.TotalTimePlay - lastTimeSpawn >= curWaveData.timeInterval)
            {
                lastTimeSpawn = InGameManager.Instance.TotalTimePlay;
                //RandomMonster();
                foreach (var container in curWaveData.containers)
                {
                    SpawnMonsterByContainer(container);
                }
            }
        }
    }
    //private void RandomMonster()
    //{
    //    var rd = Random.Range(0, curWaveData.monsters.Length);
    //    curMonsterContainer = curWaveData.monsters[rd];
    //}
    private async void SpawnMonsterByContainer(MonsterContainerData monsterContainer)
    {
        int count = monsterContainer.spawnCount;
        float minDistance = curWaveData.minDistance;
        float deltaDistance = curWaveData.deltaDistance ;
        var originScale = monsterContainer.monsterSetting.PrefabMonster.transform.localScale;
        int maxSpawnPerFrame = 6;
        int spawnCount = 0;
        for (int i = 0; i < count; i++)
        {
            Vector3 dir = Random.insideUnitCircle;
            float length = minDistance + deltaDistance * Random.value;
            Vector3 pos = dir.normalized * length + mainCharacter.transform.position;
            GameDynamicData.atkMonster = monsterContainer.monsterSetting.atk;
            GameDynamicData.hpMonster = monsterContainer.monsterSetting.hp;
            GameDynamicData.mSpeedMonster = monsterContainer.monsterSetting.mSpeed;
            var tf = PoolManager.Pools[StringConst.POOL_OPPONENT_NAME].Spawn(monsterContainer.monsterSetting.PrefabMonster, pos, Quaternion.identity);
            if (tf)
            {
                tf.localScale = originScale;
                spawnCount++;
                if (spawnCount == maxSpawnPerFrame)
                {
                    spawnCount = 0;
                    await Task.Delay(33);
                }
            }           
        }
    }
    private void SpawnMiniBoss(MiniBossContainer miniBossContainer)
    {
        UpdateEnemyAttack(miniBossContainer.idMonster, miniBossContainer.atk);
        float minDistance = 7f;
        float deltaDistance = 1f;
        var originScale = miniBossContainer.PrefabMonster.transform.localScale;
        //calculate position
        Vector3 dir = Random.insideUnitCircle;
        float length = minDistance + deltaDistance * Random.value;
        Vector3 pos = dir.normalized * length + mainCharacter.transform.position;

        //
        GameDynamicData.atkMonster = miniBossContainer.atk;
        GameDynamicData.hpMonster = miniBossContainer.hp;
        GameDynamicData.mSpeedMonster = miniBossContainer.mSpeed;
        var tf = PoolManager.Pools[StringConst.POOL_OPPONENT_NAME].Spawn(miniBossContainer.PrefabMonster, pos, Quaternion.identity);
    }
}

