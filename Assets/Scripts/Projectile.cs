using UnityEngine;
using System.Collections;

public class Projectile : Spritable {

	Tower sourceTower;

	Vector3 destination;
	Vector3 dir;

	static float extraRange = 1f;
	static float speed = 1f;

	void init(Tower t, Vector3 dest){
		sourceTower = t;
		destination = dest;

		dir = (destination - t.transform.position).normalized;

		BoxCollider b = this.GetComponent<BoxCollider>();
		b.size = new Vector3(0.25f, 0.25f, 0.25f);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(SourceTower.transform.position, this.transform.position) < SourceTower.Attributes[Tower.AttributeType.range] + extraRange){
			move ();
		} else {
			destroy();
		}
	}

	void move(){
		this.transform.position += dir * speed;
	}

	void destroy(){
		SourceTower.projectiles.Remove(this);

		GameObject.Destroy(this.gameObject);
	}

	void OnCollisionEnter(Collision c){
		Troop t = c.gameObject.GetComponent<Troop>();
		if (t != null){
			t.beDamagedBy(this);
		}
	}

	public void onTroopHit(Troop t){
		destroy();
	}

	public Tower SourceTower {
				get {
						return sourceTower;
				}
				set {
						sourceTower = value;
				}
	}

	public static Projectile createProjectile(Tower t, Vector3 dest){

		Spritable s = createSpritable();
		s.gameObject.AddComponent<Projectile>();
		Projectile p = s.GetComponent<Projectile>();

		p.transform.position = t.transform.position;
		p.init(t, dest);

		p.Costume = "RedCircle";

		return p;
	}
}
