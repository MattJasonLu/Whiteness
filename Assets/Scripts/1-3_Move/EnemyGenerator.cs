using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {

	public int minStep = 40;
	public int maxStep = 50;
	public int step;
	public int count = 0;
	Vector3 oldPos;
	Vector3 newPos;
	public float offset;
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
