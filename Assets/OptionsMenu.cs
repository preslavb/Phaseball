using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

	public Dropdown resolutionDropdown;

	Resolution[] resolutions;

	public RectTransform contentPanel;
	public ScrollRect scrollRect;


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

	public void Volume(float volume)
	{
		AudioListener.volume = volume;
	}

	public void SetFullscreen (bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;

	}

	public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        resolutionDropdown.transform.GetChild(3).transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition =
            (Vector2)resolutionDropdown.transform.GetChild(3).GetComponent<ScrollRect>().transform.InverseTransformPoint(resolutionDropdown.transform.GetChild(3).transform.GetChild(0).position)
            - (Vector2)resolutionDropdown.transform.GetChild(3).GetComponent<ScrollRect>().transform.InverseTransformPoint(target.position);
    }

}

