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
	// 当前行动单元目标
	private GameObject currentActTargetUnit;

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

	void SortBySpeed()
	{

	}

	void ToBattle()
	{

	}
}
