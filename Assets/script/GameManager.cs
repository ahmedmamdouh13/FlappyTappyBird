﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

public delegate void GameDelegate();
public static event GameDelegate OnGameStarted;
public static event GameDelegate OnGameOverConfirmed;

public static GameManager Instance;

public GameObject startPage;
public GameObject gameOverPage;
public GameObject countdownPage;
public Text scoreText;

enum PageState{
	None,
	Start,
	GameOver,
	Countdown
}


int score = 0;

bool gameOver = true;

public bool GameOver {get{return gameOver;}}

public int Score{get {return score;}}

void Awake(){
	Instance = this;
}

void OnEnable(){
	CountdownText.OnCountdownFinished += OnCountdownFinished ;
	TapController.OnPlayerDied += OnPlayerDied;
	TapController.OnPlayerScored += OnPlayerScored;

}

void OnDisable(){
	CountdownText.OnCountdownFinished -= OnCountdownFinished ;
	TapController.OnPlayerDied -= OnPlayerDied;
	TapController.OnPlayerScored -= OnPlayerScored;
}



void OnPlayerDied(){
	gameOver=true;
	int savedScore=PlayerPrefs.GetInt("HighScore");
	if (score > savedScore){
		PlayerPrefs.SetInt("HighScore",score);
	}
	setPageState(PageState.GameOver);
}

void OnPlayerScored(){
	score++;
	scoreText.text=score.ToString();
}



void OnCountdownFinished(){
	setPageState(PageState.None);
	OnGameStarted();//event is sent to TapController
	score=0;
	gameOver=false;
}

void setPageState(PageState state){
	switch(state){

		case PageState.None:
startPage.SetActive(false);
gameOverPage.SetActive(false);
countdownPage.SetActive(false);
		break;

		case PageState.Start:
startPage.SetActive(true);
gameOverPage.SetActive(false);
countdownPage.SetActive(false);
		break;

		case PageState.GameOver:
startPage.SetActive(false);
gameOverPage.SetActive(true);
countdownPage.SetActive(false);
		break;

		case PageState.Countdown:
startPage.SetActive(false);
gameOverPage.SetActive(false);
countdownPage.SetActive(true);
		break;
	}
}

public void ConfirmedGameOver(){

OnGameOverConfirmed();//event is sent to TapController
scoreText.text="0";
setPageState(PageState.Start);


}

public void StartGame(){

setPageState(PageState.Countdown);

}


}
