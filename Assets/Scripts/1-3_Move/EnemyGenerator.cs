using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyGenerator : MonoBehaviour {

	public int minStep = 40;
	public int maxStep = 50;
	int step;
	int count = 0;
	Vector3 oldPos;
	Vector3 newPos;
	float offset;
	GameObject player;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Start()
	{
		oldPos = player.transform.position;
		step = Random.Range(minStep, maxStep);
	}
	float i = 0;
	void LateUpdate()
	{
		if (i >= 1)
		{
			newPos = player.transform.position;
			offset = (newPos - oldPos).magnitude;
			count += (int)(offset);
			if (count >= step)
			{
				Debug.Log("遭遇敌人！");
				string playerListStr = "U001,U001,U001";
				PlayerPrefs.SetString("PlayerList", playerListStr);
				SceneManager.LoadScene(2);
				count = 0;
				step = Random.Range(minStep, maxStep);
			}
			else
			{
				oldPos = newPos;
			}
			i = 0;
		}
		i += Time.deltaTime;
	}
}
