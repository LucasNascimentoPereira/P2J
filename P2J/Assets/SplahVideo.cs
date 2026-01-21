using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class SplahVideo : MonoBehaviour
{
	[SerializeField] private GameObject panel;
	[SerializeField] private float vidoTime;
	[SerializeField] private VideoPlayer videoPlayer;

	private void Start()
	{
		StartCoroutine(SplahVideoEnd());
	}

	private IEnumerator SplahVideoEnd()
	{
		yield return new WaitForSeconds(vidoTime);
		videoPlayer.Stop();
		videoPlayer.enabled = false;
		panel.SetActive(false);
	}
}
