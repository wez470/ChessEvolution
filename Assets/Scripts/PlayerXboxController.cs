public class PlayerXboxController : InputController {
	private int playerNum;
	
	public PlayerXboxController( int playerNum )
	{
		this.playerNum = playerNum;	// Never used here
	}
	
	string InputController.GetUseShield() {
		return "TriggersL_" + playerNum;
	}
	
	string InputController.GetFireWeapon() {
		return "TriggersR_" + playerNum;
	}
	
	string InputController.getXAxisMovement() {
		return "L_XAxis_" + playerNum;
	}
	
	string InputController.getYAxisMovement() {
		return "L_YAxis_" + playerNum;
	}
	
	string InputController.getXAxisRotation() {
		return "R_XAxis_" + playerNum;
	}
	
	string InputController.getYAxisRotation() {
		return "R_YAxis_" + playerNum;
	}
}
