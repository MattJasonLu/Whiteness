using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleUnit : MonoBehaviour {
	// 单位编号
	public string unitId = "U001";
	// 等级
	public int level = 1;
	// 血量
	public int HP = 100;
	// 血量上限
	public int initHP = 100;
	// 魔法
	public int EP = 100;
	// 魔法上限
	public int initEP = 100;
	// 能量
	public int CP = 100;
	// 能量上限
	public int initCP = 100;
	// 攻击
	public int STR = 20;
	// 防御
	public int DEF = 10;
	// 法术强度
	public int ATS = 10;
	// 法术防御
	public int ADF = 10;
	// 速度
	public int SPD = 30;
	// 闪避
	public int DEX = 10;
	// 攻击范围，0为近战，1为远程
	public int RNG = 0;
	// 暴击
	public int CRT = 10;
	// 当前经验值
	public int EXP = 100;
	// 是否死亡
	public bool dead = false;
	
	/// <summary>
	/// 获取攻击值
	/// </summary>
	/// <returns></returns>
	public int GetAttackValue()
	{
		// TODO：需计算
		return 10;
	}

	/// <summary>
	/// 使角色受到伤害
	/// </summary>
	/// <param name="damage"></param>
	/// <returns>实际受到的伤害</returns>
	public int GetDamageValue(int damage)
	{
		// TODO：需计算
		return 10;
	}

}
