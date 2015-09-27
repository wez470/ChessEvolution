using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private const float ROT_DEAD_ZONE = 0.2f;

    public float Speed;
    public bool ShowInputDebug = false;
    public GameObject Bullet;

    private InputController input;
    private Weapon weapon;
    private Shield playerShield;

	public void SetColor(Color color){
		SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();
		sr.color = color;
		SpriteRenderer[] childSrs = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer srs in childSrs){
			if (srs.transform.gameObject.tag.Equals("Shield")){
				srs.color = new Color(color.r, color.g, color.b, 0.35f);
			}
			else {
				srs.color = color;
			}
		}
	}

    void Start () {
        playerShield = GetComponentInChildren<Shield>();
        weapon = Weapon.Random(this, 5.0f);
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