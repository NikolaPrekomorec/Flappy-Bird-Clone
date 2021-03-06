using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapController : MonoBehaviour {

	public float tapForce = 1;
	public float smootTilt = 1;
	public Vector3 startPos;
	Rigidbody2D rb;
	Quaternion tapRotation;
	Quaternion dropRotation;

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnScored;
    public static event PlayerDelegate OnDied;
    GameManager GM;

    void Start () {
        GM = GameManager.Instance;
		rb = GetComponent<Rigidbody2D>();
        GM.gameOver = true;
        rb.simulated = false;
		tapRotation = Quaternion.Euler(0,0,40);
		dropRotation = Quaternion.Euler(0,0,-70);       
	}

    private void OnEnable()
    {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
        GameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
        GameManager.OnGameStarted -= OnGameStarted;
    }

    void OnGameStarted()
    {
        rb.velocity = Vector3.zero;
        rb.simulated = true;
        GM.gameOver = false;
    }

    void OnGameOverConfirmed()
    {
        rb.simulated = false;
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
        GM.gameOver = false;
    }

	void Update () {
        if (GM.gameOver == true)
        {
            rb.simulated = false;
            return;
        }

		if (Input.GetMouseButtonDown (0))
        {

            //Time.timeScale += 1;

            rb.velocity = Vector3.zero;
			transform.rotation = tapRotation;
			rb.AddForce (Vector2.up * tapForce, ForceMode2D.Force);
		}
        if (GM.gameOver == false)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, dropRotation, smootTilt*Time.deltaTime);
        }       
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Score") {

            OnScored();

		} 
		else {
            OnDied();

			rb.simulated = false;

		}

	}






}
