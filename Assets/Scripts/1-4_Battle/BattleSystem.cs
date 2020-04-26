using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

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
	// 敌人区域
	public int enemyZone = 0;
	// 位置
	public Transform currentActPlayerUnitPos;
	public Transform playerUnitPos_1;
	public Transform playerUnitPos_2;
	public Transform playerUnitPos_3;
	public Transform currentActEnemyUnitPos;
	public Transform enemyUnitPos_1;
	public Transform enemyUnitPos_2;
	public Transform enemyUnitPos_3;


	void Start()
	{
		Init();
		SortBySpeed();
		ToBattle();
	}

	void Init()
	{
		GeneratePlayerList();
		GenerateEnemyList();

		// 单元分配
		battleUnits = new List<GameObject>();
		playerUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
		enemyUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
		playerUnits.ForEach(p => battleUnits.Add(p));
		enemyUnits.ForEach(p => battleUnits.Add(p));
	}

	// 生成玩家列表
	void GeneratePlayerList()
	{
		string playerListStr = PlayerPrefs.GetString("PlayerList");
		string[] playerArr = playerListStr.Split(',');
		for (int i = 0; i < playerArr.Length; i++)
		{
			// 加载每一个对象
			GameObject prefab = Resources.Load("Role/Player/" + playerArr[i]) as GameObject;
			Vector3 pos = playerUnitPos_1.position;
			if (i == 0)
			{
				pos = playerUnitPos_2.position;
			}
			else if (i == 1)
			{
				pos = playerUnitPos_1.position;
			}
			else if (i == 2)
			{
				pos = playerUnitPos_3.position;
			}
			GameObject role = Instantiate(prefab, pos, Quaternion.identity);
			role.tag = "Player";
		}
	}

	// 生成敌人列表
	void GenerateEnemyList()
	{
		string prefix = "Assets/Resources/";
		string path = "Role/Enemy/" + enemyZone + "/";

		// 获取所有prefab
		List<string> prefabNameList = GetPrefabNameListFromPath(prefix + path);
		prefabNameList.ForEach(p => Debug.Log(p));

		int enemyCount = Random.Range(1, 4);
		for (int i = 0; i < enemyCount; i++)
		{
			// 加载每一个对象
			GameObject prefab = Resources.Load(path + prefabNameList[Random.Range(0, prefabNameList.Count - 1)]) as GameObject;
			Vector3 pos = enemyUnitPos_1.position;
			if (i == 0)
			{
				pos = enemyUnitPos_2.position;
			}
			else if (i == 1)
			{
				pos = enemyUnitPos_1.position;
			}
			else if (i == 2)
			{
				pos = enemyUnitPos_3.position;
			}
			GameObject role = Instantiate(prefab, pos, Quaternion.identity);
			role.tag = "Enemy";
		}
	}

	// 获取路径下的所有prefab的名称
	List<string> GetPrefabNameListFromPath(string path)
	{
		List<string> prefabList = new List<string>();
		string[] paths = Directory.GetFiles(path, "*.prefab");
		foreach (string prefabPath in paths)
		{
			string[] names = prefabPath.Split(new string[] { "/", "." }, StringSplitOptions.RemoveEmptyEntries);
			string name = names[names.Length - 2];
			prefabList.Add(name);
		}
		return prefabList;
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
