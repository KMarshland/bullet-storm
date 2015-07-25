using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	
	/*public enum Team {
		Player,
		Enemy
	}*/

	//public static Team[] teams = new Team[]{Team.Enemy, Team.Player};

	private static Main instance;

	public GUISkin hudSkin;

	bool placeTowerGUIActive;
	Rect towerPlacementPosition;
	Vector3 towerWorldPos;

	void init(){
		instance = this;

		Troop.troops = new Dictionary<Player, List<Troop>>();
		foreach (Player t in Player.players){
			Troop.troops[t] = new List<Troop>();
		}
	}

	// Use this for initialization
	void Start () {
		init();

		//Player.human.CreateTower(Tower.TurretType.gun);
		//Player.computer.CreateTroop(Troop.TroopType.pistol).transform.position = new Vector3(5, 5, 0);

		Level.createLevel();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		foreach (Player p in Player.players){
			p.update();
		}

		if (!placeTowerGUIActive && Input.GetMouseButtonDown(0)){
			towerWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, Mathf.Abs(transform.position.z)));
			//Debug.Log(worldPos);
			//startTowerPlacement(Input.mousePosition);
		}

		setUIVariables();
	}

	void setUIVariables(){
		PowerUI.UI.Variables["money"] = "Money: " + Player.human.Money.ToString();
		PowerUI.UI.Variables["income"] = "Income: " + Player.human.Income;
		PowerUI.UI.Variables["supply"] = "Supply: " + Player.human.Supply;
		PowerUI.UI.Variables["supplyIncome"] = "Supply Income: " + Player.human.SupplyIncome;

		PowerUI.UI.Variables["towerPlacementMenu"] = "";

		Array options = Enum.GetValues(typeof(Tower.TurretType));
		
		foreach (Tower.TurretType t in options){
			Dictionary<Tower.AttributeType, float> atts = Tower.attributesList[t];
			if (Player.human.Money >= atts[Tower.AttributeType.price]){
				PowerUI.UI.Variables["towerPlacementMenu"] += "<div  onclick='finishTowerPlacement'>" + t.ToString() + "</div>";
			}
		}
	}

	void OnGUI(){
		/*showHUD();

		if (placeTowerGUIActive){
			placeTowerGUI();
		}*/
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

	public void placeTower(){
		Debug.Log("Fuck yeah");
	}

	void startTowerPlacement(Vector3 mousepos){
		placeTowerGUIActive = true;
		int w = 200;
		int h = 200;
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

	public static Main Instance
	{
		get 
		{
			return instance;
		}
	}
}
