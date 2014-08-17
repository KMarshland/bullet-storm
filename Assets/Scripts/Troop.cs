using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Troop : Spritable {

	//public static Dictionary<Player, List<Troop>> troops = new Dictionary<Player, List<Troop>>();

	//public static List<Troop> atroops;

	public static Dictionary<Player, List<Troop>> troops = new Dictionary<Player, List<Troop>>();

	public enum TroopType {
		pistol,
		flamethrower,
		rifle
	}

	private enum TroopAttributeType {
		range,
		damage,
		baseHealth,
		movementSpeed,
		fireFrequency,
		reloadSpeed,
		burstLength,
		spriteURI
	}

	static Dictionary<TroopType, Dictionary<TroopAttributeType, float>> attributesList = new Dictionary<TroopType, Dictionary<TroopAttributeType, float>>(){
		{
			TroopType.pistol, new Dictionary<TroopAttributeType, float>(){
				{TroopAttributeType.range, 5f},
				{TroopAttributeType.damage, 5f},
				{TroopAttributeType.baseHealth, 300f},
				{TroopAttributeType.movementSpeed, 50f},
				{TroopAttributeType.fireFrequency, 40f},
				{TroopAttributeType.burstLength, 40f},
				{TroopAttributeType.reloadSpeed, 1f}
			}
		}
	};

	Dictionary<TroopAttributeType, float> attributes = new Dictionary<TroopAttributeType, float>();

	public float health;
	Player team;
	
	public void init(TroopType type, Player team){
		attributes = attributesList[type];
		health = attributes[TroopAttributeType.baseHealth];
		this.team = team;

		Troop.troops[team].Add(this);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void beDamagedBy(Projectile p){
		float minD = p.SourceTower.Attributes[Tower.AttributeType.minDamage];
		float maxD = p.SourceTower.Attributes[Tower.AttributeType.maxDamage];
		float delta = maxD - minD;

		float dam = Random.value * delta + minD;

		this.Health -= dam;

		p.onTroopHit(this);
	}

	void die(){
		destroy();
	}

	void destroy(){
		Troop.troops[this.team].Remove(this);
		GameObject.Destroy(this.gameObject);
	}

	public float Health {
		get {
			return health;
		}
		set {
			health = value;
			if (health < 0f){
				die();
			}
		}
	}

	public Player Team {
		get {
			return team;
		}
	}

	public static Troop createTroop(Troop.TroopType type, Player team){
		
		Spritable s = createSpritable();
		s.gameObject.AddComponent<Troop>();
		Troop t = s.GetComponent<Troop>();

		t.init(type, team);
		
		t.Costume = "BlueCircle";
		
		return t;

	}

}
