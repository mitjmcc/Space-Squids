using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CircleWipe : MonoBehaviour
{
	Image wipe;
	int phase = 0;
	float phaseTime = 0;
	float phaseBlitTime = 0.5F;
	float phaseFadeTime = 1.5F;

	GameController controller;
	public string levelToLoad;

	void Awake()
	{
		wipe = GetComponent<Image>();
		wipe.enabled = true;

		controller = transform.parent.gameObject.GetComponent<GameController>();
		if (controller != null)
			levelToLoad = controller.sceneToReturnTo;
	}

	void Update()
	{
		if (phase == 0)
		{
			phaseTime += Time.deltaTime;
			float t = Mathf.SmoothStep(0, 1, (phaseTime-phaseBlitTime)/phaseFadeTime);

			wipe.material.SetFloat("_Wipe",t);
			AudioListener.volume = t;
			
			if (phaseTime >= phaseBlitTime+phaseFadeTime)
			{
				phase = 1;
				phaseTime = phaseBlitTime+phaseFadeTime;
				wipe.enabled = false;
			}
		}

		if (phase == 2)
		{
			phaseTime -= Time.deltaTime;
			float t = Mathf.SmoothStep(0, 1, (phaseTime-phaseBlitTime)/phaseFadeTime);
			
			wipe.material.SetFloat("_Wipe",t);
			AudioListener.volume = t;

			if (phaseTime <= 0)
			{
				if (Application.CanStreamedLevelBeLoaded(levelToLoad))
					Application.LoadLevel(levelToLoad);
				else
					Application.LoadLevel(Application.loadedLevel);
			}
		}
	}

	public void fadeOut()
	{
		phase = 2;
		wipe.enabled = true;
	}

	public void setLevelToLoad(string level)
	{
		levelToLoad = level;
	}
}