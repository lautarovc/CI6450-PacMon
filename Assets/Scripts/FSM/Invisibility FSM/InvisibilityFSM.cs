using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibilityFSM : FSM {

	// Use this for initialization
	void Awake () {
		InvisibleState invisible = ScriptableObject.CreateInstance<InvisibleState>();

        VisibleState visible = ScriptableObject.CreateInstance<VisibleState>();

        PlayerJumpTransition playerJump = ScriptableObject.CreateInstance<PlayerJumpTransition>();
        playerJump.targetState = invisible;

        PlayerNotJumpTransition playerNotJump = ScriptableObject.CreateInstance<PlayerNotJumpTransition>();
        playerNotJump.targetState = visible;

        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];

        invisible.character = visible.character = transform;
        playerJump.player = playerNotJump.player = player.transform;

        visible.transitions.Add(playerJump);
        invisible.transitions.Add(playerNotJump);

        states = new List<State>();

        states.Add(visible);
        states.Add(invisible);
        initialState = visible;
    }
	
}
