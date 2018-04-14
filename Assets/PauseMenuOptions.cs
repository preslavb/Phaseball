using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuOptions : MonoBehaviour {

	public Dropdown resolutionDropdown;

	Resolution[] resolutions;

	static float currentVolume = 1;

	public static float CurrentVolume
	{
		get
		{
			return currentVolume;
		}

		set
		{
			currentVolume = value;

			AudioListener.volume = currentVolume;
		}
	}

	void Start()
	{
		resolutions = Screen.resolutions;

		resolutionDropdown.ClearOptions ();

		List<string> options = new List<string> ();

		int currentResolutionIndex = 0;
		for (int i = 0; i < resolutions.Length; i++) 
		{
			string option = resolutions [i].width + "x" + resolutions [i].height;
			options.Add(option);

			if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
			{
				currentResolutionIndex = i;
			}


		}

		resolutionDropdown.AddOptions (options);
		resolutionDropdown.value = currentResolutionIndex;
		resolutionDropdown.RefreshShownValue ();

	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions [resolutionIndex];
		Screen.SetResolution (resolution.width, resolution.height, Screen.fullScreen);

	}

	public void Volume (float volume)
	{
		CurrentVolume = volume;
	}

	public void SetFullscreen (bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;

	}

}
