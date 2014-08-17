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
				{AttributeType.price, 5f},
				{AttributeType.burstLength, 3f},
				{AttributeType.level, 3}
			}
		},

		{
			TurretType.machinegun, new Dictionary<AttributeType, float>(){
				{AttributeType.minDamage, 5f},
				{AttributeType.maxDamage, 5f},
				{AttributeType.range, 5f},
				{AttributeType.price, 5f},
				{AttributeType.level, 3}
			}
		},

		{
			TurretType.artillery, new Dictionary<AttributeType, float>(){
				{AttributeType.minDamage, 5f},
				{AttributeType.maxDamage, 5f},
				{AttributeType.frequency, 5f},
				{AttributeType.range, 5f},
				{AttributeType.price, 5f},
				{AttributeType.level, 4}
			}
		},

		{
			TurretType.missile, new Dictionary<AttributeType, float>(){
				{AttributeType.minDamage, 5f},
				{AttributeType.maxDamage, 5f},
				{AttributeType.frequency, 5f},
				{AttributeType.range, 5f},
				{AttributeType.price, 5f},
				{AttributeType.level, 4}
			}
		},

		{
			TurretType.blazeMachineGun, new Dictionary<AttributeType, float>(){
				{AttributeType.minDamage, 5f},
				{AttributeType.maxDamage, 5f},
				{AttributeType.frequency, 5f},
				{AttributeType.range, 5f},
				{AttributeType.price, 5f},
				{AttributeType.level, 5}
			}
		},

		{
			TurretType.photon, new Dictionary<AttributeType, float>(){
				{AttributeType.minDamage, 5f},
				{AttributeType.maxDamage, 5f},
				{AttributeType.frequency, 5f},
				{AttributeType.range, 5f},
				{AttributeType.price, 5f},
				{AttributeType.level, 5}
			}
		},
	};

	static Dictionary<TurretType, string> sprites = new Dictionary<TurretType, string>(){
		{
			TurretType.gun, "GunTowerBulletStorm"
		},
		{
			TurretType.artillery, "ArtilleryBulletStorm"
		},
		{
			TurretType.blazeMachineGun, "GunTowerBulletStorm"//TODO: Get graphic
		},
		{
			TurretType.flamethrower, "FlamethrowerBulletStorm"
		},
		{
			TurretType.gas, "PoisonGasBulletStorm"
		},
		{
			TurretType.glue, "GlueTowerBulletStorm"
		},
		{
			TurretType.machinegun, "GunTowerBulletStorm"//TODO: Get graphic
		},
		{
			TurretType.missile, "MissileBulletStorm"
		},
		{
			TurretType.photon, "GunTowerBulletStorm"//TODO: Get graphic
		},
		{
			TurretType.sword, "SwordTowerBulletStorm"
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
		this.Costume = sprites[type];
		this.team = team;
	}

	// Use this for initialization
	void Start () {
		Projectile.createProjectile(this, new Vector3(5f, 5f, 0f));
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

		this.projectiles.Add(Projectile.createProjectile(this, t.transform.position));

		ticksSinceFired = 0;
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

	public static void createTower(TurretType tt, Player team){
		Spritable sp = Spritable.createSpritable();
		sp.gameObject.AddComponent<Tower>();
		Tower turret = sp.GetComponent<Tower>();
		turret.init(tt, team);
	}
}
