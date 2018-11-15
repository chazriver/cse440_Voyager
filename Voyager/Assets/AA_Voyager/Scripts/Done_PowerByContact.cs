using UnityEngine;
using System.Collections;

public class Done_PowerByContact : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	public GameObject Player;

	public int scoreValue;
	private Done_GameController gameController;

	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");


		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <Done_GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void OnTriggerEnter (Collider other)
	{

		GameObject PlayerObject = GameObject.FindGameObjectWithTag ("Player");

		if (other.tag == "Boundary" || other.tag == "Enemy")
		{
			return;
		}

		if (explosion != null)
		{
			Instantiate(explosion, transform.position, transform.rotation);
		}

		if (other.tag == "Player")
		{
		 GameObject myWarp =	Instantiate(playerExplosion, transform.position, Quaternion.identity) as GameObject;

		 myWarp.transform.rotation = Quaternion.Euler(0, 180, 0);
		// myWarp.transform.position = new Vector3(4.0f, -1.5f, 0.0f);
 	   myWarp.transform.parent = PlayerObject.transform;

		 GameObject.Find("Skybox_A").GetComponent<Done_BGScroller>().scrollSpeed += -75.0f;

		 Destroy (myWarp, 2);
		 StartCoroutine(delay());
		}

		gameController.AddScore(scoreValue);
	}


	IEnumerator delay()
	{
	    yield return wait();
	}

	IEnumerator wait()
	{
	    yield return new WaitForSeconds(3);
			GameObject.Find("Skybox_A").GetComponent<Done_BGScroller>().scrollSpeed += 75.0f;
	}

}
