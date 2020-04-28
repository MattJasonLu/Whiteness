using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleSystem : MonoBehaviour {

	public static BattleSystem _instance;

	private BattleSystem()
	{
		_instance = this;
	}

	// 敌人区域
	public int enemyZone = 0;
	public int unitMoveSpeed = 5;
	// 位置
	public Transform currentActPlayerUnitPos;
	public Transform playerUnitPos_1;
	public Transform playerUnitPos_2;
	public Transform playerUnitPos_3;
	public Transform currentActEnemyUnitPos;
	public Transform enemyUnitPos_1;
	public Transform enemyUnitPos_2;
	public Transform enemyUnitPos_3;
	// 选项面板
	public GameObject basicPanel;
	// 伤害信息
	public GameObject hint;

	public Canvas canvas;
	public Camera UICamera;

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
	// 是否等待玩家选择技能
	private bool isWaitForPlayerToChooseSkill;
	// 是否等待玩家选择目标
	private bool isWaitForPlayerToChooseTarget;
	// 是否单元跑向目标
	private bool isUnitRunningToTarget;
	// 是否单元跑回原地
	private bool isUnitRunningBack;
	// 鼠标射线
	private Ray targetChooseRay;
	// 射线碰撞物体
	private RaycastHit targetHit;
	private Vector3 currentActUnitInitialPosition;
	private Vector3 currentActUnitTargetPosition;
	private Vector3 currentactUnitStopPosition;

	void Awake()
	{
		basicPanel.SetActive(false);
	}

	void Start()
	{
		Init();
		SortBySpeed();
		ToBattle();
	}

	void Update()
	{
		if (isWaitForPlayerToChooseSkill)
		{
			basicPanel.SetActive(true);
		}

		if (isWaitForPlayerToChooseTarget)
		{
			basicPanel.SetActive(false);
			targetChooseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(targetChooseRay, out targetHit))
			{
				if (Input.GetMouseButtonDown(0) && targetHit.collider.gameObject.tag == "Enemy")
				{
					currentActTargetUnit = targetHit.collider.gameObject;
					isWaitForPlayerToChooseTarget = false;
					currentActTargetUnitStatus = currentActTargetUnit.GetComponent<RoleUnit>();
					//如果是远程单位直接在这里LaunchAttack，就不需要RunToTarget
					if (currentActUnit.GetComponent<RoleUnit>().RNG == 1)
					{
						LaunchAttack();
					}
					else
					{
						RunToTarget();
					}
				}
			}
		}

		if (isUnitRunningToTarget)
		{
			float distanceToTarget = Vector3.Distance(currentActUnitTargetPosition, currentActUnit.transform.position);           //到目标的距离，需要实时计算
            //避免靠近目标时抖动
			if (distanceToTarget > 1)
			{
				if (currentActUnit.tag == "Player")
				{
					currentActUnit.GetComponentInChildren<Animator>().SetInteger("Horizontal", -1);
				}
				else if (currentActUnit.tag == "Enemy")
				{
					currentActUnit.GetComponentInChildren<Animator>().SetInteger("Horizontal", 1);
				}
				//Time.deltaTime保证速度单位是每秒
				currentActUnit.transform.localPosition = Vector3.MoveTowards(currentActUnit.transform.localPosition, currentActUnitTargetPosition, unitMoveSpeed * Time.deltaTime);
			}
			else
			{
				//停止移动
				currentActUnit.GetComponentInChildren<Animator>().SetInteger("Horizontal", 0);
				//关闭移动状态
				isUnitRunningToTarget = false;
				//记录停下的位置
				currentactUnitStopPosition = currentActUnit.transform.position;
				//开始攻击
				LaunchAttack();
			}

		}

		if (isUnitRunningBack)
		{
			//离初始位置的距离
			float distanceToInitial = Vector3.Distance(currentActUnit.transform.position, currentActUnitInitialPosition);           
			if (distanceToInitial > 0.3)
			{
				if (currentActUnit.tag == "Player")
				{
					currentActUnit.GetComponentInChildren<Animator>().SetInteger("Horizontal", 1);
				}
				else if (currentActUnit.tag == "Enemy")
				{
					currentActUnit.GetComponentInChildren<Animator>().SetInteger("Horizontal", -1);
				}
				currentActUnit.transform.localPosition = Vector3.MoveTowards(currentActUnit.transform.localPosition, currentActUnitInitialPosition, unitMoveSpeed * Time.deltaTime);
			}
			else
			{
				//停止移动
				currentActUnit.GetComponentInChildren<Animator>().SetInteger("Horizontal", 0);
				currentActUnit.GetComponentInChildren<Animator>().SetTrigger("Idle");
				//关闭移动状态
				isUnitRunningBack = false;
				//修正到初始位置和朝向
				currentActUnit.transform.position = currentActUnitInitialPosition;
				//攻击单位回原位后行动结束，到下一个单位
				ToBattle();
			}
		}
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
			role.GetComponent<RoleUnit>().SPD = 200;
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
		battleUnits.Sort((x, y) => y.GetComponent<RoleUnit>().SPD.CompareTo(x.GetComponent<RoleUnit>().SPD));
	}

	void ToBattle()
	{
		remainPlayerUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
		remainEnemyUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
		if (remainPlayerUnits.Count == 0)
		{
			// 失败
			Debug.Log("我方全灭，战斗失败");
		}
		else if (remainEnemyUnits.Count == 0)
		{
			// 成功
			Debug.Log("敌人全灭，战斗胜利");
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
				//if (currentActUnit.tag == "Player")
				//{
				//	currentActUnit.transform.position = currentActPlayerUnitPos.position;
				//}
				//else if (currentActUnit.tag == "Enemy")
				//{
				//	currentActUnit.transform.position = currentActEnemyUnitPos.position;
				//}
				FindTarget();
			}
			else
			{
				ToBattle();
			}
		}
	}

	void FindTarget()
	{
		if (currentActUnit.tag == "Enemy")
		{
			//如果行动单位是怪物则从存活玩家对象中随机一个目标
			int targetIndex = Random.Range(0, remainPlayerUnits.Count);
			currentActTargetUnit = remainPlayerUnits[targetIndex];

			//如果是远程单位直接在这里LaunchAttack，就不需要RunToTarget
			if (currentActUnit.GetComponent<RoleUnit>().RNG == 1)
			{
				LaunchAttack();
			}
			else
			{
				RunToTarget();
			}
		}
		else if (currentActUnit.tag == "Player")
		{
			isWaitForPlayerToChooseSkill = true;
		}
	}

	void RunToTarget()
	{
		currentActUnitInitialPosition = currentActUnit.transform.position;         //保存移动前的位置和朝向，因为跑回来还要用
		currentActUnitTargetPosition = currentActTargetUnit.transform.position;         //目标的位置
																						//开启移动状态
		isUnitRunningToTarget = true;
        // 移动的控制放到Update里，因为要每一帧判断离目标的距离
	}

	/// <summary>
	/// 发动攻击
	/// </summary>
	void LaunchAttack()
	{
		StartCoroutine("WaitForAttack");
	}

	/// <summary>
	/// 播放攻击动画，延时
	/// </summary>
	/// <returns></returns>
	IEnumerator WaitForAttack()
	{
		// 获取行动角色的攻击值
		int attackVal = currentActUnitStatus.GetAttackValue();
		ShowHint(currentActUnit, "发动攻击");
		yield return new WaitForSeconds(1);
		int receiveVal = currentActTargetUnitStatus.GetDamageValue(attackVal);
		ShowHint(currentActTargetUnit, receiveVal.ToString());
		yield return new WaitForSeconds(0.5f);
		// 使角色返回
		isUnitRunningBack = true;

	}

	// 按钮事件触发
	public void OnAttack()
	{
		isWaitForPlayerToChooseSkill = false;
		isWaitForPlayerToChooseTarget = true;
	}

	/// <summary>
	/// 显示伤害信息
	/// </summary>
	/// <param name="str"></param>
	void ShowHint(GameObject target, string str)
	{
		// 世界坐标转屏幕坐标
		Vector3 unitPos = Camera.main.WorldToScreenPoint(target.transform.position);
		Vector3 guiPos;
		RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.GetComponent<RectTransform>(), unitPos, UICamera, out guiPos);
		GameObject hintGO = Instantiate(hint);
		hintGO.GetComponent<Text>().text = "-" + str;
		hintGO.transform.SetParent(canvas.transform, false);
		hintGO.transform.position = guiPos + new Vector3(0, 0.7f, 0);
		// 销毁
		Destroy(hintGO, 0.5f);
	}
}
