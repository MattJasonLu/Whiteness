using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleUnit : MonoBehaviour {
	// 单位编号
	public string unitId = "U001";
	public int HP = 100;
	public int initHP = 100;
	public int EP = 100;
	public int initEP = 100;
	// 能量
	public int CP = 100;
	public int initCP = 100;
	public int STR = 20;
	public int DEF = 10;
	// 法术强度
	public int ATS = 10;
	// 法术防御
	public int ADF = 10;
	public int SPD = 30;
	// 灵巧，增加闪避
	public int DEX = 10;
	// 敏捷，增加速度
	public int AGL = 10;
	// 攻击范围，0为单个对象，1为多个
	public int RNG = 0;

	public bool dead = false;
	
	

}
