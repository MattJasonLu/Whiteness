using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour {

	// 所有参战单元
	private List<GameObject> battleUnits;
	// 所有玩家单元
	private List<GameObject> playerUnits;
	// 所有敌人单元
	private List<GameObject> enemyUnits;
	// 剩余玩家单元
	private List<GameObject> remainPlayerUnits;
	// 剩余敌人单元
	private List<GameObject> remainEnemyUnits;
	// 当前行动单元
	private GameObject currentActUnit;
	private RoleUnit currentActUnitStatus;
	// 当前行动单元目标
	private GameObject currentActTargetUnit;
	private RoleUnit currentActTargetUnitStatus;

	void Start()
	{
		Init();
		SortBySpeed();
		ToBattle();
	}

	private void Init()
	{
		// 单元分配
		battleUnits = new List<GameObject>();
		playerUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
		enemyUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
		playerUnits.ForEach(p => battleUnits.Add(p));
		enemyUnits.ForEach(p => battleUnits.Add(p));
	}

	// 速度降序排列
	void SortBySpeed()
	{
		battleUnits.Sort((x, y) => x.GetComponent<RoleUnit>().SPD.CompareTo(y.GetComponent<RoleUnit>().SPD));
	}

	void ToBattle()
	{
		remainPlayerUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
		remainEnemyUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
		if (remainPlayerUnits.Count == 0)
		{
			// 失败
		}
		else if (remainEnemyUnits.Count == 0)
		{
			// 成功
		}
		else
		{
			// 战斗队列第一位出栈
			currentActUnit = battleUnits[0];
			currentActUnitStatus = currentActUnit.GetComponent<RoleUnit>();
			battleUnits.RemoveAt(0);
			battleUnits.Add(currentActUnit);
			if (!currentActUnitStatus.dead)
			{

			}
		}
	}
}
