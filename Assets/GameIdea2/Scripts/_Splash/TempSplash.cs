using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GravityWell.Splash; // Include DOTween namespace

public class TempSplash : MonoBehaviour
{
	public SplashScreen splashScreen;
	public CanvasGroup[] splashImages; // Assign 3 CanvasGroups in the inspector (each image should have a CanvasGroup component)
	public float fadeDuration = 1.0f; // How long each image and text will fade out
	public float delayBetweenSplashes = 0.5f; // Delay between images

	private void Start()
	{
		foreach (CanvasGroup imageCanvasGroup in splashImages)
		{
			imageCanvasGroup.alpha = 0.0f;
		}
		PlaySplashSequence();
	}

	private void PlaySplashSequence()
	{
		Sequence sequence = DOTween.Sequence(); // Create a sequence for the animations

		// Play fade for each image's CanvasGroup one after another
		foreach (CanvasGroup imageCanvasGroup in splashImages)
		{
			sequence.Append(imageCanvasGroup.DOFade(1, fadeDuration / 2)) // Fade in
				.AppendInterval(fadeDuration / 2) // Hold the fully visible state
				.Append(imageCanvasGroup.DOFade(0, fadeDuration)) // Fade out
				.AppendInterval(delayBetweenSplashes); // Delay before the next image
		}

		// After the images, fade in and fade out the text
		sequence.OnComplete(Complete); // Call Complete when the sequence finishes
	}

	private void Complete()
	{
		splashScreen.OnSplashComplete();
		// Logic when the splash sequence is complete
		Debug.Log("Splash sequence complete!");
	}
}