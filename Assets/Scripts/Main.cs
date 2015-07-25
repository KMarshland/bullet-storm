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

	bool placingTower;
	Tower.TurretType currentTowerType;

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

		initializeUI ();

		//Player.human.CreateTower(Tower.TurretType.gun);
		//Player.computer.CreateTroop(Troop.TroopType.pistol).transform.position = new Vector3(5, 5, 0);

		Level.createAll ();

		Level.Current = Level.LevelInstance.test;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		foreach (Player p in Player.players){
			p.update();
		}

		if (placingTower){

			if (Input.GetMouseButtonDown(0)){
				Vector3 towerWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(
					Input.mousePosition.x,
					Input.mousePosition.y, 
					Mathf.Abs(transform.position.z)
				));
				finishPlacingTower(towerWorldPos);
			}
		}

		updateUI();
	}

	void initializeUI(){
		Array options = Enum.GetValues(typeof(Tower.TurretType));
		
		foreach (Tower.TurretType t in options){

			var parent = PowerUI.UI.document.getElementById("tower-list");
			var el = PowerUI.UI.document.createElement("div");

			el.innerHTML = t.ToString().ToUpper();

			el.className = "select-option";
			el.parentNode = parent;

			Tower.TurretType currentType = t;

			el.OnClick += delegate(PowerUI.UIEvent uiEvent) {
				Dictionary<Tower.AttributeType, float> atts = Tower.attributesList[currentType];
				if (atts[Tower.AttributeType.price] <= Player.human.Money){
					startTowerPlacement(currentType);
				}
			};

			parent.appendInnerHTML("<br />");
		}
		PowerUI.UI.document.getElementsByClassName ("inner-sidebar-container")[0].style.height = (Screen.height - 60) + "px";
	}

	void updateUI(){
		PowerUI.UI.Variables["money"] = Player.human.Money.ToString();
		PowerUI.UI.Variables["income"] =  Player.human.Income.ToString();
		PowerUI.UI.Variables["supply"] = Player.human.Supply.ToString();
		PowerUI.UI.Variables["supplyIncome"] = Player.human.SupplyIncome.ToString();

		PowerUI.UI.Variables["sidebarHeight"] = (200).ToString();
	}

	void OnGUI(){
		if (placingTower) {

			Texture2D image = Resources.Load ("Sprites/" + Tower.getSpriteUrl (currentTowerType)) as Texture2D;

			GUI.Label (
				new Rect (
					Input.mousePosition.x - (image.width)/2f, 
					(Screen.height - Input.mousePosition.y) - (image.height)/2f, 
					image.width, image.height
				), 
				image
			);
		}
	}

	void startTowerPlacement(Tower.TurretType turretType){
		currentTowerType = turretType;
		placingTower = true;
	}

	void finishPlacingTower(Vector3 position){
		placingTower = false;
		Dictionary<Tower.AttributeType, float> atts = Tower.attributesList[currentTowerType];
		if (atts[Tower.AttributeType.price] <= Player.human.Money){
			Player.human.CreateTower (currentTowerType).transform.position = position;
			Player.human.Money -= atts[Tower.AttributeType.price];
		}

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
