using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private const float ROT_DEAD_ZONE = 0.2f;
	private const float DEFAULT_RESPAWN_TIME = 7.0f;

    public float Speed;
    public bool ShowInputDebug = false;
    public GameObject Bullet;
    public int MAX_HP;
    public Color Color;
    public ParticleSystem DeathExplosion;
    public int PlayerNum;

	private SpriteRenderer playerSpriteRenderer;
	private SpriteRenderer shieldSpriteRenderer;
    private int hp;
    private float lastMorphTime;
	private bool shieldOn;
	private float lastDeathTime;
	private bool wasDead;
    private InputController input;
    private Weapon weapon;
    private Shield playerShield;

	public void SetColor(Color col){
		FindSpriteRenderers ();
		Color = col;
		playerSpriteRenderer.color = col;
		shieldSpriteRenderer.color = new Color(Color.r, Color.g, Color.b, 0.35f);
	}

    public void SetPlayerNum(int playerNum) {
        PlayerNum = playerNum;    
    }
	
	private void setPlayerColorForHP(){
		float fractionHP = (float)hp/(float)MAX_HP;
		Color newColor = new Color(Color.r*fractionHP, Color.g*fractionHP, Color.b*fractionHP, Color.a);
		playerSpriteRenderer.color = newColor;
	}

    void Start() {
		shieldOn = false;
		lastDeathTime = Time.time - DEFAULT_RESPAWN_TIME;
		wasDead = false;
    	hp = MAX_HP;
        playerShield = GetComponentInChildren<Shield>();
        weapon = Weapon.Default();
    }

	void FindSpriteRenderers ()
	{
		playerSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer> ();
		foreach (SpriteRenderer sr in this.gameObject.GetComponentsInChildren<SpriteRenderer> ()) {
			if (sr.transform.gameObject.tag.Equals ("Shield")) {
				shieldSpriteRenderer = sr;
			}
		}
	}
    
    void Update () {
		if (isDead ()) {
			playerShield.enabled(false);
			return;
		} else if (wasDead) {
			// Respawn
			wasDead = false;
			GetComponent<BoxCollider2D>().enabled = true;
			GetComponent<SpriteRenderer>().enabled = true;
			hp = MAX_HP;
			setPlayerColorForHP();
		}
        if ((Time.time - lastMorphTime) > 10.0f) {
            lastMorphTime = Time.time;
            weapon = Weapon.Morph(weapon, Weapon.Random(0.5f));
        }
		setRotation();
        checkFire();
        checkShield();
		setMovement();
    }

	public bool isDead(){
		if (lastDeathTime + DEFAULT_RESPAWN_TIME >= Time.time) {
			return true;
		} else {
			return false;
		}
	}

    private void checkFire() {
        if(Input.GetAxis(input.GetFireWeapon()) > 0.1f) {
            weapon.Fire(this);
        }
    }

    private void checkShield() {
        float leftTrigger = Input.GetAxis(input.GetUseShield());
        bool isLeftTriggerDown = leftTrigger > 0.9f;
        shieldOn = playerShield.enabled(isLeftTriggerDown);
    }

    public void SetInput(InputController inputController) {
        input = inputController;
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
		if (other.gameObject.tag == "Bullet" && !shieldOn) {
			this.hp--;
			setPlayerColorForHP();
			if (hp <= 0){
                int shooter = other.gameObject.GetComponent<Bullet>().Owner.PlayerNum;
                ScoreController scoreController = GameObject.FindGameObjectWithTag(
                    "ScoreController").GetComponent<ScoreController>();
                scoreController.IncreaseScore(shooter);
				KillAndRespawn();
			}
		}
	}
	
	private void KillAndRespawn(){
		lastDeathTime = Time.time;
		wasDead = true;
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<SpriteRenderer>().enabled = false;
		DeathExplosion.startColor = Color;
		DeathExplosion.transform.position = transform.position;
		ParticleSystem[] subSystems = DeathExplosion.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem system in subSystems){
			Debug.Log(system.name);
			system.startColor = Color;
		}
		Instantiate ( DeathExplosion );
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