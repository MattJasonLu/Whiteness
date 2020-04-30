﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	public static LevelLoader _instance;
	public Animator transition;
	public float transitionTime = 1f;

	void Awake()
	{
		_instance = this;
	}

	public void	LoadNextLevel()
	{
		StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
	}

	public void LoadPreviousLevel()
	{
		StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
	}

	IEnumerator LoadLevel(int levelIndex)
	{
		transition.SetTrigger("Start");
		yield return new WaitForSeconds(transitionTime);
		SceneManager.LoadScene(levelIndex);
	}


}
