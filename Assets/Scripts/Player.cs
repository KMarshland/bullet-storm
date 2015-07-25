using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {

	public static List<Player> players = new List<Player>();

	public static Player human = new Player(Level.Direction.positive);
	public static Player computer = new Player(Level.Direction.negative);

	float money;
	float supply;

	float income;
	float supplyIncome;

	Level.Direction direction;

	void init(Level.Direction dir){
		players.Add(this);
		
		money = 100f;
		supply = 100f;
		direction = dir;
	}

	public Player(){
		init (Level.Direction.positive);
	}

	
	public Player(Level.Direction dir){
		init(dir);
	}

	public void update(){
		money += income;
		supply += supplyIncome;
	}


	public float Money {
		get {
			return money;
		} set {
			money = value;
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

	public Level.Direction Direction {
		get {
			return direction;
		}
	}

	public float SupplyIncome {
		get {
			return supplyIncome;
		}
	}

	public static void updateAll(){
		foreach (Player p in players) {
			p.update();
		}
	}

	public Tower CreateTower(Tower.TurretType turretType){
		return Tower.createTower(turretType, this);
	}

	public Troop CreateTroop(Troop.TroopType troopType){
		return CreateTroop (troopType, Level.Current);
	}

	public Troop CreateTroop(Troop.TroopType troopType, Level.LevelInstance level){
		return Troop.createTroop (troopType, this, level);
	}
}
