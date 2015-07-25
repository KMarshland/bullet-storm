using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {

	public static List<Player> players = new List<Player>();

	public static Player human = new Player();
	public static Player computer = new Player();

	float money;
	float supply;

	float income;
	float supplyIncome;

	public Player(){
		players.Add(this);

		money = 100f;
		supply = 100f;
	}

	public void update(){
		money += income;
		supply += supplyIncome;
	}


	public float Money {
		get {
			return money;
		}
	}

	public float Supply {
		get {
			return supply;
		}
	}

	public float Income {
		get {
			return income;
		}
	}

	public float SupplyIncome {
		get {
			return supplyIncome;
		}
	}

	public Tower CreateTower(Tower.TurretType turretType){
		return Tower.createTower(turretType, this);
	}

	public Troop CreateTroop(Troop.TroopType troopType){
		return Troop.createTroop (troopType, this);
	}
}
