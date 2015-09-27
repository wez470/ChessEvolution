using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private const float ROT_DEAD_ZONE = 0.2f;

    public float Speed;
    public bool ShowInputDebug = false;
    public GameObject Bullet;
    public int MAX_HP;
    
	private SpriteRenderer playerSpriteRenderer;
	private SpriteRenderer shieldSpriteRenderer;
	private SpriteRenderer gunSpriteRenderer;
    private int hp;
    private Color color;

    private InputController input;
    private Weapon weapon;
    private Shield playerShield;

	public void SetColor(Color col){
		FindSpriteRenderers ();
		color = col;
		playerSpriteRenderer.color = col;
		gunSpriteRenderer.color = col;
		shieldSpriteRenderer.color = new Color(color.r, color.g, color.b, 0.35f);;
	}
	
	private void setPlayerColorForHP(){
		float fractionHP = (float)hp/(float)MAX_HP;
		fractionHP = (fractionHP/2f) + 0.5f;
		Color newColor = new Color(color.r*fractionHP, color.g*fractionHP, color.b*fractionHP, color.a);
		playerSpriteRenderer.color = newColor;
		gunSpriteRenderer.color = newColor;
	}

    void Start () {
    	hp = MAX_HP;
        playerShield = GetComponentInChildren<Shield>();
        weapon = Weapon.Default(this);
    }

	void FindSpriteRenderers ()
	{
		playerSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer> ();
		foreach (SpriteRenderer sr in this.gameObject.GetComponentsInChildren<SpriteRenderer> ()) {
			if (sr.transform.gameObject.tag.Equals ("Shield")) {
				shieldSpriteRenderer = sr;
			}
			else {
				gunSpriteRenderer = sr;
			}
		}
	}
    
    void Update () {
        checkFire();
        checkShield();
    }

    private void checkFire() {
        if(Input.GetAxis(input.GetFireWeapon()) > 0.1f) {
            weapon.Fire();
        }
    }

    private void checkShield() {
        float leftTrigger = Input.GetAxis(input.GetUseShield());
        bool isLeftTriggerDown = leftTrigger > 0.9f;
        playerShield.enabled(isLeftTriggerDown);
    }

    public void SetInput(InputController inputController) {
        input = inputController;
    }

    void FixedUpdate() {
        setMovement();
        setRotation();
    }

    void OnGUI() {
        if (ShowInputDebug) {
            GUI.contentColor = Color.black;
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height),
                  string.Format(string.Join("\n",
                                  new string[] {
                                      "X-axis movement = {0}",
                                      "Y-axis movement = {1}",
                                      "X-axis rotation = {2}",
                                      "Y-axis rotation = {3}",
                                      "Fire weapon = {4}",
                                      "Use shield = {5}"}),
                      Input.GetAxis(input.GetXAxisMovement()),
                      Input.GetAxis(input.GetYAxisMovement()),
                      Input.GetAxis(input.GetXAxisRotation()),
                      Input.GetAxis(input.GetYAxisRotation()),
                      Input.GetAxis(input.GetFireWeapon()),
                      Input.GetAxis(input.GetUseShield())));
        }
    }
    
	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Collision Entered");
		if (other.gameObject.tag == "Bullet") {
			Debug.Log ("BULLET COLLISION");
			this.hp--;
			setPlayerColorForHP();
			if (hp <= 0){
				KillAndRespawn();
			}
			Destroy(other.gameObject);
		}
	}
	
	private void KillAndRespawn(){
		Destroy ( this.gameObject );
		//TODO: RESPAWN
	}

    private void setMovement() {
        float speedX = Input.GetAxis(input.GetXAxisMovement()) * Speed;
        float speedY = -Input.GetAxis(input.GetYAxisMovement()) * Speed;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(speedX, speedY));
    }
    
    private void setRotation() {
        float rotX = Input.GetAxis(input.GetXAxisRotation());
        float rotY = Input.GetAxis(input.GetYAxisRotation());
        float angle = Mathf.Atan2(-rotY, rotX) * Mathf.Rad2Deg - 90f;

        if(Mathf.Abs(rotX) > ROT_DEAD_ZONE || Mathf.Abs(rotY) > ROT_DEAD_ZONE) {
            transform.rotation = Quaternion.Euler(0, 0, angle);
            GetComponent<Rigidbody2D>().angularVelocity = 0;
        }
    }
    
}