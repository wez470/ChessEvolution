public class PlayerXboxController : InputController {
    private int playerNum;
    
    public PlayerXboxController( int playerNum )
    {
        this.playerNum = playerNum;    // Never used here
    }
    
    string InputController.GetUseShield() {
        return "LB_" + playerNum;
    }
    
    string InputController.GetFireWeapon() {
        return "RB_" + playerNum;
    }
    
    string InputController.GetXAxisMovement() {
        return "L_XAxis_" + playerNum;
    }
    
    string InputController.GetYAxisMovement() {
        return "L_YAxis_" + playerNum;
    }
    
    string InputController.GetXAxisRotation() {
        return "R_XAxis_" + playerNum;
    }
    
    string InputController.GetYAxisRotation() {
        return "R_YAxis_" + playerNum;
    }
}
