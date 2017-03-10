using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	[Header("Velocity")]
	public float flappyVelocity;

	[Header("Player states")]
	public bool isDead = false;

	private Animator animator;
	private Rigidbody2D rigidBody2d;
	public SpriteRenderer puff;
	private SpriteRenderer plane;
	string[] planeName = new string[4];

	// Use this for initialization
	void Start () {
		planeName[0] = "PlaneIdle";
		planeName[1] = "PlaneIdleBlue";
		planeName[2] = "PlaneIdleGreen";
		planeName[3] = "PlaneIdleYellow";
		animator = GetComponent<Animator> ();
		rigidBody2d = GetComponent<Rigidbody2D> ();
		puff = GetComponentsInChildren<SpriteRenderer> ()[1];

		int index = Random.Range(0, planeName.Length);
		animator.Play (planeName[index]);
		// Convertendo de coordenadas de Tela para Mundo
		Vector3 startPos = 
			Camera.main.ViewportToWorldPoint (new Vector3 (0.2f, 0.8f));

		startPos.z = -1.58f; // Para garantir visibilidade do FlappyBird

		transform.position = startPos;
	}
	

	void Update () {
		
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (isDead) {
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			} else {
				Boost ();
			}
		}

		if (isDead)
			return;

		Vector3 angles = transform.eulerAngles;
		angles.z = Mathf.Clamp (rigidBody2d.velocity.y * 4f, -90f, 45f);
		transform.eulerAngles = angles;
		if (puff.enabled) {
			
		}

	}



	void Boost() {
		animator.SetTrigger ("Boost");
		rigidBody2d.velocity = Vector2.up * flappyVelocity;
		if (!puff.enabled) {
			puff.enabled = true;
			StartCoroutine (hidePuff ());
		}

	}

	void OnCollisionEnter2D(Collision2D collision) {
		// Se for pipe entao mata o FlappyBird

		if (collision.collider.CompareTag ("Pipe")) {
			// Mooorreu :(
			isDead = true;
			// Deixa de colidir
			GetComponent<Collider2D> ().isTrigger = true;
		}
	}

	IEnumerator hidePuff()
	{

		yield return new WaitForSeconds(1);
		puff.enabled = false;
		//Do Function here...
	}
}
