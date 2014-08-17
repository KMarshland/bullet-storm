using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	
	/*public enum Team {
		Player,
		Enemy
	}*/

	//public static Team[] teams = new Team[]{Team.Enemy, Team.Player};

	public GUISkin hudSkin;

	bool placeTowerGUIActive;
	Rect towerPlacementPosition;

	void init(){
		Troop.troops = new Dictionary<Player, List<Troop>>();
		foreach (Player t in Player.players){
			Troop.troops[t] = new List<Troop>();
		}
	}

	// Use this for initialization
	void Start () {
		init();

		Tower.createTower(Tower.TurretType.gun, Player.human);
		Troop.createTroop(Troop.TroopType.pistol, Player.computer).transform.position = new Vector3(5, 5, 0);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		foreach (Player p in Player.players){
			p.update();
		}

		if (Input.GetMouseButtonDown(0)){
			Vector3 worldPos = Camera.main.ViewportToWorldPoint(Input.mousePosition);
			Debug.Log(worldPos);
			startTowerPlacement(Input.mousePosition);
		}
	}

	void OnGUI(){
		showHUD();

		if (placeTowerGUIActive){
			placeTowerGUI();
		}
	}

	void showHUD(){
		GUI.skin = hudSkin;
		GUI.Toolbar(new Rect(0, 0, Screen.width, 25), -1, new string[]{
			"Money: " + Player.human.Money,
			"Income: " + Player.human.Income,
			"Supply: " + Player.human.Supply,
			"Supply Income: " + Player.human.SupplyIncome,
		});
	}

	void placeTowerGUI(){
		GUI.Box(towerPlacementPosition, "Place a tower");

		List<Tower.TurretType> options = new List<Tower.TurretType>();

		int x = 0;
		int y = 0;
		int h = 0;
		int w = 0;
		int p = 0;

		foreach (Tower.TurretType t in options){
			Dictionary<AttributeType, float> atts = Tower.attributesList[t];
			GUI.Label(new Rect(x, y, w, h), atts[Tower.AttributeType.price].ToString());
		}
	}

	void startTowerPlacement(Vector3 mousepos){
		placeTowerGUIActive = true;
		int w = 100;
		int h = 100;
		int p = 5;

		mousepos = new Vector3(mousepos.x, Screen.height - mousepos.y, 0f);

		int x = (int)((mousepos.x + w + p) > Screen.width ? Screen.width - (w + p) : mousepos.x);
		int y = (int)((mousepos.y + h + p) > Screen.height ? Screen.height - (h + p) : mousepos.y);

		towerPlacementPosition = new Rect(x, y, w, h);
		placeTowerGUIActive = true;
	}

	public static List<Player> enemiesOf(Player team){
		List<Player> res = new List<Player>();

		foreach (Player t in Player.players){
			if (t != team){
				res.Add(t);
			}
		}

		return res;
	}
}
