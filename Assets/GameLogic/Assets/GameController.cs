 	using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
	public GameObject checkpointList;
	public int lapCount = 3;
	public AudioClip ambience;
	public AudioClip music;
	public string sceneToReturnTo;

	int[] curLap;
	int[] curCheckpoint;
	int[] curProgress;
	int[] boostProgress;
	int checkpointCount;
	GameObject checkpointLogic;

	Image redOverlay;
	Image blueOverlay;
	float overlayAlpha = 0.25F;

	WinnerCrownController winnerCrown;
	Image redLapPanel;
	Image blueLapPanel;
	Text redLapText;
	Text blueLapText;
	Text redCurrentLapText;
	Text blueCurrentLapText;

	Image redLose;
	Image blueLose;
	CircleWipe circleWipe;
	RectTransform levelEnd;
	Text[] levelEndText;

	int winner = -1;
	float winScale = 0;
	float winTime = -1;
	float winDelay = 7.5F;
	bool hasWiped;

	AudioController audioLogic;
	SquidController[] squids;

	void Awake()
	{
		// Initialize lap and checkpoint values

		curLap = new int[2] {0, 0};
		curCheckpoint = new int[2] {-1, -1};
		curProgress = new int[2] {-1, -1};
		boostProgress = new int[2] {0, 0};
		checkpointCount = checkpointList.transform.childCount;
		checkpointLogic = transform.Find("CheckpointLogic").gameObject;

		// Get all the UI elements

		redOverlay = transform.Find("UI/RedOverlay").gameObject.GetComponent<Image>();
		blueOverlay = transform.Find("UI/BlueOverlay").gameObject.GetComponent<Image>();
		redOverlay.enabled = true;
		blueOverlay.enabled = true;

		winnerCrown = transform.Find("UI/WinnerPanel/WinnerCrown").gameObject.GetComponent<WinnerCrownController>();
		redLapPanel = transform.Find("UI/RedLapPanel").gameObject.GetComponent<Image>();
		blueLapPanel = transform.Find("UI/BlueLapPanel").gameObject.GetComponent<Image>();
		redLapText = transform.Find("UI/RedLapPanel/LapText").gameObject.GetComponent<Text>();
		blueLapText = transform.Find("UI/BlueLapPanel/LapText").gameObject.GetComponent<Text>();
		redCurrentLapText = transform.Find("UI/RedLapPanel/CurrentLapText").gameObject.GetComponent<Text>();
		blueCurrentLapText = transform.Find("UI/BlueLapPanel/CurrentLapText").gameObject.GetComponent<Text>();

		redLose = transform.Find("UI/RedLose").gameObject.GetComponent<Image>();
		blueLose = transform.Find("UI/BlueLose").gameObject.GetComponent<Image>();
		circleWipe = transform.Find("UI/CircleWipe").gameObject.GetComponent<CircleWipe>();
		levelEnd = transform.Find("UI/LevelEnd").gameObject.GetComponent<RectTransform>();
		levelEndText = levelEnd.gameObject.GetComponentsInChildren<Text>();
		levelEnd.gameObject.SetActive(false);

		redLose.enabled = true;
		blueLose.enabled = true;
		redLose.CrossFadeAlpha(0F, 0F, true);
		blueLose.CrossFadeAlpha(0F, 0F, true);

		// Get the audio controller

		audioLogic = transform.Find("AudioLogic").gameObject.GetComponent<AudioController>();

		// Get the squids

		squids = transform.Find("Spawn").gameObject.GetComponentsInChildren<SquidController>();
	}

	void Start()
	{
		// Initialize the checkpoint logic

		checkpointLogic.SetActive(true);
		AdvanceSquid(0, false);
		AdvanceSquid(1, false);

		// Set the lap text to read the entered maximum laps

		redLapText.text = "Lap      / " + lapCount;
		blueLapText.text = "Lap      / " + lapCount;
	}
	
	void Update()
	{
		// Set the current lap text to read the current lap

		redCurrentLapText.text = "" + Mathf.Clamp(curLap[0], 1, lapCount);
		blueCurrentLapText.text = "" + Mathf.Clamp(curLap[1], 1, lapCount);

		// If there's a winner, scale up the winner text

		if (winner != -1)
		{
			winScale += (1-winScale)*0.025F;
			levelEnd.localRotation = Quaternion.Euler(0,0,(1-winScale)*30);
			levelEnd.localScale = new Vector3(winScale,winScale,1);
		}

		// If the win screen has been on long enough, begin the circle wipe

		if ((winTime != -1) && (Time.time > winTime+winDelay) && (!hasWiped))
		{
			circleWipe.fadeOut();
			hasWiped = true;
		}
	}

	// This method is called by the checkpoint logic through their script CheckpointController
	// when the squid they're assigned enters their collider - it does this:
	//		1) adds their progress to the counters
	//		2) plays the appropriate checkpoint sound
	//		3) moves the checkpoint logic to the next checkpoint (or deactivates it all if someone won)
	//		4) boosts the screen glow overlay of the squid that advanced
	//		5) tells the winner crown who's winning

	public void AdvanceSquid(int index, bool boost)
	{
		// Add the progress of the squid
		
		curProgress[index]++;
		if (boost)
			boostProgress[index]++;
		if (curCheckpoint[index] == 0)
			curLap[index]++;
		curCheckpoint[index]++;
		curCheckpoint[index] %= checkpointCount;

		// Play the checkpoint sound
		
		if (curProgress[index] > 0)
			audioLogic.playCheckpointSound(index, boost);

		// Deactivate all the checkpoint logic if this squid won, or move this squid's
		// checkpoint logic to its new position if there is no winner yet

		if (curLap[index] > lapCount)
			win(index);
		else
		{
			Transform logic = checkpointLogic.transform.GetChild(index);
			Transform newCheckpoint = checkpointList.transform.GetChild(curCheckpoint[index]);
			logic.position = newCheckpoint.position;
			logic.rotation = newCheckpoint.rotation;
		}

		// Boost the overlay of the squid that advanced

		float fadeTime = 0.75F;
		if (boost)
			fadeTime = 1.25F;
		if (index == 0)
		{
			redOverlay.CrossFadeAlpha(1F, 0F, false);
			redOverlay.CrossFadeAlpha(overlayAlpha, fadeTime, false);
		}
		else
		{
			blueOverlay.CrossFadeAlpha(1F, 0F, false);
			blueOverlay.CrossFadeAlpha(overlayAlpha, fadeTime, false);
		}
		
		// Tell the winner crown who's winning

		if (curProgress[index] == 0)
			winnerCrown.changeWinner(-1);
		else
		{
			if (curProgress[0] == curProgress[1])
				winnerCrown.changeWinner(-1);
			else if (curProgress[0] > curProgress[1])
				winnerCrown.changeWinner(0);
			else
				winnerCrown.changeWinner(1);
		}
	}

	// When a squid wins, all the checkpoint logic is turned off,
	// the lap panels are faded away, the winner screen is brought up
	// for the winner, the losing screen is brought up for the loser,
	// the loser loses control, and the end music is played

	void win(int index)
	{
		checkpointLogic.SetActive(false);

		float fadeTime = 0.125F;
		redLapPanel.CrossFadeAlpha(0F, fadeTime, false);
		blueLapPanel.CrossFadeAlpha(0F, fadeTime, false);
		redLapText.CrossFadeAlpha(0F, fadeTime, false);
		blueLapText.CrossFadeAlpha(0F, fadeTime, false);
		redCurrentLapText.CrossFadeAlpha(0F, fadeTime, false);
		blueCurrentLapText.CrossFadeAlpha(0F, fadeTime, false);

		winner = index;
		winTime = Time.time;
		levelEnd.gameObject.SetActive(true);
		levelEnd.localPosition = new Vector3(0,(index-0.5F)*Screen.height*-0.5F,0);
		levelEndText[0].text = ((index == 0) ? "Red " : "Blue ") + "Wins!";
		levelEndText[0].color = ((index == 0) ? Color.red : Color.blue);
		float boostAccuracy = ((float) boostProgress[index]/curProgress[index])*100F;
		levelEndText[1].text = "Boost Accuracy: " + boostAccuracy.ToString("F2") + "%";

		if (index == 0)
			blueLose.CrossFadeAlpha(0.8F, fadeTime, false);
		else
			redLose.CrossFadeAlpha(0.8F, fadeTime, false);
		squids[1-index].setControl(0);

		audioLogic.playEndMusic();
	}
}