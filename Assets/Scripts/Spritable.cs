using UnityEngine;
using System.Collections;

public class Spritable : MonoBehaviour {

	string costume;

	Texture2D spriteImg;

	SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		sprite = this.gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string Costume {
		get {
			return costume;
		}
		set {
			costume = value;
			if (costume.StartsWith("~")){
				costume = costume.TrimStart(new char[]{'~'});
			} else if (!costume.StartsWith("Sprites/")){
				costume = "Sprites/" + costume;
			}

			this.SpriteImg = Resources.Load(costume) as Texture2D;
		}
	}

	public Texture2D SpriteImg {
		get {
			return spriteImg;
		}
		set {
			spriteImg = value;
			sprite = (sprite == null)? this.gameObject.GetComponent<SpriteRenderer>() : sprite;
			sprite.sprite = Sprite.Create(value, new Rect(0,0,value.width,value.height), new Vector2(0.5f, 0.5f),10);
		}
	}

	public static Spritable createSpritable(){
		GameObject obj = GameObject.Instantiate(Resources.Load("Prefabs/Sprite")) as GameObject;
		return obj.GetComponent<Spritable>();
	}
}
