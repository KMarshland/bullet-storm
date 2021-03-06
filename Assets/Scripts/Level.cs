﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : Spritable {

	static LevelInstance current;

	public enum LevelInstance {
		test
	}

	public enum Direction{
		positive,
		negative
	}

	static Dictionary<LevelInstance, Level> levels = new Dictionary<LevelInstance, Level>();

	static Dictionary<LevelInstance, string> sprites = new Dictionary<LevelInstance, string>(){
		{
			LevelInstance.test, "TestMap"
		}
	};

	static Dictionary<LevelInstance, string> names = new Dictionary<LevelInstance, string>(){
		{
			LevelInstance.test, "Test"
		}
	};

	static Dictionary<LevelInstance, Vector3[]> nodeSets = new Dictionary<LevelInstance, Vector3[]>(){
		{
			LevelInstance.test, new Vector3[]{
				new Vector3(-25f, -3f, 0),
				new Vector3(-17.25f, -2.25f, 0),
				new Vector3(-18f, 14f, 0),
				new Vector3(-6f, 14f, 0),
				new Vector3(-8f, -9f, 0),
				new Vector3(7.5f, -8f, 0),
				new Vector3(7f, 4f, 0),
				new Vector3(25, 3f, 0)
			}
		}
	};

	string levelName;

	Vector3[] nodes;
	Dictionary<float, BezierSegment> segments;
	List<float> orderedKeys;

	public float totalLength;

	public void init(LevelInstance inst){
		levels [inst] = this;

		//load the configuration stuff
		this.Costume = "Levels/" + sprites[inst];
		this.levelName = names[inst];
		nodes = nodeSets[inst];

		segments = new Dictionary<float, BezierSegment> ();
		orderedKeys = new List<float> ();

		//build the bezier segments from the nodes
		totalLength = 0;
		for (int i = 0; i < nodes.Length; i++) {
			Vector3 midpointBefore = i == 0 ? nodes[i] : (nodes[i-1] + nodes[i])/2f;
			Vector3 midpointAfter = i == nodes.Length - 1 ? nodes[i] : (nodes[i+1] + nodes[i])/2f;
			var bez = new BezierSegment(midpointBefore, nodes[i], midpointAfter);
			segments[totalLength] = bez;
			orderedKeys.Add(totalLength);
			totalLength += bez.Length;
		}

		sprite.sortingLayerName = "Map";
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmosSelected () {
		Gizmos.color = Color.red;

		float resolution = 5000f;
		for (int i = 0; i < resolution; i++) {
			Gizmos.DrawLine(pointAt(i/resolution), pointAt((i + 1f)/resolution));
		}
	}

	public Vector3 pointAlong(float distance){
		return pointAlong (distance, Direction.positive);
	}

	public Vector3 pointAlong(float distance, Direction direction){
		float percentage = distance/totalLength;

		if (direction == Direction.negative) {
			percentage = (1f - percentage);
		}
		
		return pointAt(percentage);
	}

	public Vector3 pointAt(float percentage, Direction direction){
		if (direction == Direction.positive) {
			return pointAt(percentage);
		} else if (direction == Direction.negative) {
			return pointAt(1f - percentage);
		}

		return new Vector3 (0, 0, 0);
	}

	public Vector3 pointAt(float percentage){

		float length = totalLength * percentage;
		float key = 0f;
		for (int i = orderedKeys.Count - 1; i >= 0; i--){
			if (orderedKeys[i] < length){
				key = orderedKeys[i];
				break;
			}
		}

		return segments [key].PointAlong (length - key);
	}

	public float TotalLength {
		get {
			return totalLength;
		}
	}

	struct BezierSegment {
		const int chunks = 1000;//how good your approximation is

		Vector3 p0, p1, p2;

		float length;

		public BezierSegment(Vector3 np0, Vector3 np1, Vector3 np2){
			p0 = np0;
			p1 = np1;
			p2 = np2;

			/// calculate the length of the curve
			/// 
			/// arc length for vector function r = integral from 0 to 1 of:
			///   (|r'| == √(r'x^2 + r'y^2 + r'z^2)) 
			/// 
			/// f(t) is always a cube (same value in each index)
			/// f(t)    = <(1 - t)^2*p0 + 2(t - t^2)*p1 + t^2*p2, .. , .. >
			/// f'(t)   = -2(1 - t)*p0 + 2(1 - 2t)*p1 + 2t*p2
			///         = 2*((t-1)*p0 - 2t*p1 + p1 + t*p2)
			/// f'(t)^2 = 4((t-1)*p0 - 2t*p1 + p1 + t*p2)^2
			/// 
			/// ∫√fx'(t)^2 + fy'(t)^2 + fz'(t)^2
			/// 

			//there's pretty much no way to do that integral in a decent timeframe. 
			//Approximate it with a midpoint reimann sum

			float columnWidth = 1f/chunks;

			float sum = 0;
			for (int chunk = 0; chunk < chunks; chunk++){
				sum += rootFPrime(columnWidth * chunk + columnWidth/2f).magnitude * columnWidth;
			}
			length = sum;
		}

		Vector3 rootFPrime(float t){
			return new Vector3(
				rootFPrime(t, p0.x, p1.x, p2.x),
				rootFPrime(t, p0.y, p1.y, p2.y),
				rootFPrime(t, p0.z, p1.z, p2.z)
			);
		}

		float rootFPrime(float t, float a, float b, float c){
			return 4*((t-1)*a - 2*t*b + b + t*c)*((t-1)*a - 2*t*b + b + t*c);
		}

		public float Length {
			get {
				return length;
			}
		}

		public Vector3 PointAlong(float lengthAlong){
			return PointAt (lengthAlong / Length);
		}

		public Vector3 PointAt(float percentage) {//t is the percentage of the way through the curve
			percentage = Mathf.Clamp01(percentage);
			float oneMinusT = 1f - percentage;
			return
				oneMinusT * oneMinusT * p0 +
					2f * oneMinusT * percentage * p1 +
					percentage * percentage * p2;
		}

	}

	public static void createAll(){
		var insts = System.Enum.GetValues(typeof(LevelInstance));
		foreach (LevelInstance inst in insts){
			Level.createLevel(inst);
		}
	}

	public static Level createLevel(LevelInstance inst){
		if (levels.ContainsKey(inst)) {
			return levels[inst];
		}

		Spritable sp = Spritable.createSpritable();
		sp.gameObject.AddComponent<Level>();
		Level lev = sp.GetComponent<Level>();
		lev.init (inst);

		lev.gameObject.SetActive (false);

		return lev;
	}

	public static Level getLevel(LevelInstance inst){
		return levels [inst];
	}

	public static LevelInstance Current {
		get {
			return current;
		} set {
			if (current != null){
				levels[current].gameObject.SetActive (false);
			}
			current = value;
			levels[current].gameObject.SetActive (true);
		}
	}
}
