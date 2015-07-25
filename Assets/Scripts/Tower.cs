using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : Spritable {
	
	public enum TurretType {
		gun,
		sword,
		glue,
		gas,
		flamethrower,
		machinegun,
		artillery,
		missile,
		blazeMachineGun,
		photon
	};

	public enum AttributeType {
		minDamage,
		maxDamage,
		frequency,
		range,
		price,
		burstLength,
		level
	};

	public static Dictionary<TurretType, Dictionary<AttributeType, float>> attributesList = new Dictionary<TurretType, Dictionary<AttributeType, float>>(){
		{
			TurretType.gun, new Dictionary<AttributeType, float>(){
				{AttributeType.minDamage, 5f},
				{AttributeType.maxDamage, 5f},
				{AttributeType.frequency, 3f},
				{AttributeType.range, 50f},
				{AttributeType.price, 40f},
				{AttributeType.level, 1}
			}
		},
		
		{
			TurretType.sword, new Dictionary<AttributeType, float>(){
				{AttributeType.minDamage, 10f},
				{AttributeType.maxDamage, 15f},
				{AttributeType.frequency, 2f},
				{AttributeType.range, 2f},
				{AttributeType.price, 45f},
				{AttributeType.level, 1}
			}
		},
		
		{
			TurretType.glue, new Dictionary<AttributeType, float>(){
				{AttributeType.minDamage, 5f},
				{AttributeType.maxDamage, 5f},
				{AttributeType.frequency, 5f},
				{AttributeType.range, 5f},
				{AttributeType.price, 5f},
				{AttributeType.level, 2}
			}
		},
		
		{
			TurretType.gas, new Dictionary<AttributeType, float>(){
				{AttributeType.minDamage, 5f},
				{AttributeType.maxDamage, 5f},
				{AttributeType.frequency, 5f},
				{AttributeType.range, 5f},
				{AttributeType.price, 5f},
				{AttributeType.level, 2}
			}
		},
		
		{
			TurretType.flamethrower, new Dictionary<AttributeType, float>(){
				{AttributeType.minDamage, 5f},
				{AttributeType.maxDamage, 5f},
				{AttributeType.frequency, 5f},
				{AttributeType.range, 5f},
				{AttributeType.price, 70f},
				{AttributeType.burstLength, 3f},
				{AttributeType.level, 3}
			}
		},
		
		{
			TurretType.machinegun, new Dictionary<AttributeType, float>(){
				{AttributeType.minDamage, 4f},
				{AttributeType.maxDamage, 6f},
				{AttributeType.range, 55f},
				{AttributeType.price, 85f},
				{AttributeType.level, 3}
			}
		},
		
		{
			TurretType.artillery, new Dictionary<AttributeType, float>(){
				{AttributeType.minDamage, 13f},
				{AttributeType.maxDamage, 18f},
				{AttributeType.frequency, 5f},
				{AttributeType.range, 75f},
				{AttributeType.price, 90f},
				{AttributeType.level, 4}
			}
		},
		
		{
			TurretType.missile, new Dictionary<AttributeType, float>(){
				{AttributeType.minDamage, 20f},
				{AttributeType.maxDamage, 20f},
				{AttributeType.frequency, 4f},
				{AttributeType.range, 70f},
				{AttributeType.price, 90f},
				{AttributeType.level, 4}
			}
		},
		
		{
			TurretType.blazeMachineGun, new Dictionary<AttributeType, float>(){
				{AttributeType.minDamage, 10f},
				{AttributeType.maxDamage, 20f},
				{AttributeType.frequency, 0.08f},
				{AttributeType.range, 65f},
				{AttributeType.price, 100f},
				{AttributeType.level, 5}
			}
		},
		
		{
			TurretType.photon, new Dictionary<AttributeType, float>(){
				{AttributeType.minDamage, 15f},
				{AttributeType.maxDamage, 30f},
				{AttributeType.frequency, 0.5f},
				{AttributeType.range, 5f},
				{AttributeType.price, 125f},
				{AttributeType.level, 5}
			}
		}
	};

	static Dictionary<TurretType, string> sprites = new Dictionary<TurretType, string>(){
		{
			TurretType.gun, "GunTower"
		},
		{
			TurretType.artillery, "ArtilleryTower"
		},
		{
			TurretType.blazeMachineGun, "BlazeMachineGun"
		},
		{
			TurretType.flamethrower, "Flamethrower"
		},
		{
			TurretType.gas, "PoisonGasTower"
		},
		{
			TurretType.glue, "GlueTower"
		},
		{
			TurretType.machinegun, "MachineGunTower"
		},
		{
			TurretType.missile, "MissileTower"
		},
		{
			TurretType.photon, "PhotonCannon"
		},
		{
			TurretType.sword, "SwordTower"
		}

	};

	Player team;

	TurretType turretType;
	Dictionary<AttributeType, float> attributes = new Dictionary<AttributeType, float>();
	public List<Projectile> projectiles = new List<Projectile>();

	Troop target;
	int ticksSinceFired;

	public void init(TurretType type, Player team){
		turretType = type;
		attributes = attributesList[type];
		this.Costume = "Towers/" + sprites[type];
		this.team = team;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (canFire){
			pickTarget();
			fireAt(target);
		} else {
			ticksSinceFired ++;
		}
	}

	bool canFire {
		get {
			return ticksSinceFired/5f > this.attributes[AttributeType.frequency];
		}
	}

	void pickTarget(){

		if (target != null){//if the existing target is close enough, don't switch
			if (Vector3.Distance(this.transform.position, target.transform.position) < this.Attributes[AttributeType.range]){
				return;
			}
		}

		//reset the target
		target = null;

		//make a list of all valid enemy troops. TODO: cache this
		List<Player> enemyTeams = Main.enemiesOf(this.team);

		List<Troop> enemies = new List<Troop>();
		foreach (Player t in enemyTeams){
			enemies.AddRange(Troop.troops[t]);
		}


//		foreach (Troop t in Troop.troops){
			/*foreach (Player te in enemyTeams){
				if (t.Team == te){
					enemies.Add(t);
				}
			}*/
//		}

		//pick the best one. Currently just the furthest still in range. 
		float maxDistance = -1f;

		foreach (Troop t in enemies){
			float dist = Vector3.Distance(this.transform.position, t.transform.position);
			if (dist < this.Attributes[AttributeType.range] && dist > maxDistance){
				maxDistance = dist;
				target = t;
			}
		}
	}

	void fireAt(Troop t){
		if (t == null){
			return;
		}

		this.projectiles.Add(createProjectile(t.transform.position));

		ticksSinceFired = 0;
	}

	Projectile createProjectile(Vector3 dest){
		return Projectile.createProjectile(this, dest);
	}

	public TurretType TurretType2 {
		get {
			return turretType;
		}
	}

	public Dictionary<AttributeType, float> Attributes {
		get {
			return attributes;
		}
	}

	public static Tower createTower(TurretType tt, Player team){
		Spritable sp = Spritable.createSpritable();
		sp.gameObject.AddComponent<Tower>();
		Tower turret = sp.GetComponent<Tower>();
		turret.init(tt, team);
		turret.name = tt.ToString() + " Turret";
		return turret;
	}
}
