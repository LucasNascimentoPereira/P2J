using UnityEngine;
using System.Collections;

public class SplahVideo : MonoBehaviour
{
	[SerializeField] private GameObject panel;
	[SerializeField] private float vidoTime;

	private void Start()
	{
		StartCoroutine(SplahVideoEnd());
	}

	private IEnumerator SplahVideoEnd()
	{
		yield return new WaitForSeconds(vidoTime);
		panel.SetActive(false);
	}
}
