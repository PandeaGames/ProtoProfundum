using UnityEngine;
using System.Collections;

public class RoachController : MonoBehaviour {
	private double[,,] spawnMap; 
	private double[,,] litMap; 

	public int resolution = 5;
	public GameObject roachPrefab;
	public int spawnDensity = 1;
	public int spawnResolution = 1;
	public int spawnDistance = 50;
	public int zoneResetTime = 5;

	private int _x, _y, _z;
	private int _worldX, _worldY, _worldZ;
	private RoachLight[] lights;

	private Game _game;

	// Use this for initialization
	void Start () 
	{
		_game = FindObjectOfType<Game> ();

		AreaCenter center = FindObjectOfType<AreaCenter> ();

		if (center) {
			transform.position = center.transform.position;
		}

		_setupDataField ();
	}
	private void _setupDataField()
	{
		_x = (int)(GetComponent<Collider> ().bounds.size.x / resolution);
		_y = (int)(GetComponent<Collider> ().bounds.size.y / resolution);
		_z = (int)(GetComponent<Collider> ().bounds.size.z / resolution);
		
		_worldX = (int)(GetComponent<Collider> ().bounds.center.x - GetComponent<Collider> ().bounds.extents.x);
		_worldY = (int)(GetComponent<Collider> ().bounds.center.y - GetComponent<Collider> ().bounds.extents.y);
		_worldZ = (int)(GetComponent<Collider> ().bounds.center.z - GetComponent<Collider> ().bounds.extents.z);
		
		spawnMap = new double[_x, _y, _z];
		litMap = new double[_x, _y, _z];
		
		lights = FindObjectsOfType<RoachLight> ();
		for (int i =0; i<_x; i++) 
		{
			for (int j =0; j<_y; j++) 
			{
				for (int k =0; k<_z; k++) 
				{
                    spawnMap[i, j, k] = 0;
                    litMap[i, j, k] = 0;
					for(int l = 0; l<lights.Length; l++)
					{
						if(Mathf.Sqrt(
							(lights[l].gameObject.transform.position.x - (_worldX + i * resolution)) * (lights[l].gameObject.transform.position.x - (_worldX + i * resolution)) +
							(lights[l].gameObject.transform.position.y - (_worldY + j * resolution)) * (lights[l].gameObject.transform.position.y - (_worldY + j * resolution)) +
							(lights[l].gameObject.transform.position.z - (_worldZ + k * resolution)) * (lights[l].gameObject.transform.position.z - (_worldZ + k * resolution))
						) < lights[l].radius && lights[l].emitsLight)
						{
							litMap[i, j, k] = double.MaxValue;
							spawnMap[i, j, k] = double.MaxValue;
							break;
						}
						if(Mathf.Sqrt(
							(lights[l].gameObject.transform.position.x - (_worldX + i * resolution)) * (lights[l].gameObject.transform.position.x - (_worldX + i * resolution)) +
							(lights[l].gameObject.transform.position.y - (_worldY + j * resolution)) * (lights[l].gameObject.transform.position.y - (_worldY + j * resolution)) +
							(lights[l].gameObject.transform.position.z - (_worldZ + k * resolution)) * (lights[l].gameObject.transform.position.z - (_worldZ + k * resolution))
							) < lights[l].radius)
						{
							spawnMap[i, j, k] = double.MaxValue;
							break;
						} 
					}
				}
			}
		}
	}
	void OnDrawGizmos() {
		if (spawnMap == null)
			return;
		Gizmos.color = Color.green;
		for (int i = 0; i < _x; i++) {
			for (int j = 0; j < _y; j++) {
				for (int k = 0; k < _z; k++) {
					if (litMap [i, j, k] != 0) {
						Gizmos.color = Color.red;
						/*Gizmos.DrawSphere (
							new Vector3 (
								i * resolution + (GetComponent<Collider> ().bounds.center.x - GetComponent<Collider> ().bounds.size.x / 2), 
								j * resolution + (GetComponent<Collider> ().bounds.center.y - GetComponent<Collider> ().bounds.size.y / 2), 
								k * resolution + (GetComponent<Collider> ().bounds.center.z - GetComponent<Collider> ().bounds.size.z / 2))
							, 0.3f);*/
					} else {
						
					}
				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
	public bool isLit(Vector3 point)
	{
		int x, y, z;

		x = (int)((point.x  - _worldX) / resolution);
		y = (int)((point.y - _worldY) / resolution);
		z = (int)((point.z - _worldZ ) / resolution);

		if(x< 0 || x > _x || y < 0 || y > _y || z < 0 || z > _z) return false;

		return litMap [x, y, z] == double.MaxValue;
	}
	public bool canSpawn(Vector3 point)
	{
		int x, y, z;

		x = (int)((point.x  - _worldX) / resolution);
		y = (int)((point.y - _worldY) / resolution);
		z = (int)((point.z - _worldZ ) / resolution);

		if(x< 0 || x > _x || y < 0 || y > _y || z < 0 || z > _z) return false;

		return spawnMap [x, y, z] == double.MaxValue;
	}
	public void LightLaunched(GameObject light)
	{
		int roachCount= 0;
		Vector3 pos = new Vector3 ();
		int x, y, z;
		GameObject firstRoach = null;
		for (int i = 0; i < spawnDistance; i+= spawnResolution) 
		{
			pos = light.transform.position + light.transform.forward * i;

			pos.x+=Random.value * 10 - Random.value * 10;
			pos.y+=Random.value * 10 - Random.value * 10;
			pos.z+=Random.value * 10 - Random.value * 10;

			x = (int)((pos.x  - _worldX) / resolution);
			y = (int)((pos.y - _worldY) / resolution);
			z = (int)((pos.z - _worldZ ) / resolution);

			if(x< 0 || x > _x || y < 0 || y > _y || z < 0 || z > _z) continue;

			if(spawnMap[x, y, z] != double.MaxValue && (spawnMap[x, y, z] < Time.time || spawnMap[x, y, z] == Time.time + zoneResetTime))
			{
				roachCount+=1;
				spawnMap[x, y, z] = Time.time + zoneResetTime;
				setAreaValues(new Vector3(x, y, z), spawnMap[x, y, z], 3);
				firstRoach = (GameObject)Instantiate(roachPrefab, pos, light.transform.rotation);
				if (_game != null) {
					firstRoach.gameObject.transform.parent = _game.gameObject.transform;
				}
				firstRoach.GetComponent<RoachAI> ().lightSpawned = true;
			}
		}
		if (firstRoach != null) {
			if (roachCount < 20) {
				AkSoundEngine.PostEvent("Roach_Swarm_Low", firstRoach);
			} else if (roachCount < 40) {
				AkSoundEngine.PostEvent("Roach_Swarm_Medium", firstRoach);
			} else {
				AkSoundEngine.PostEvent("Roach_Swarm_High", firstRoach);
			}
		}

	}
	private void setAreaValues(Vector3 point, double value, int radius)
	{
		for (int x = (int)point.x - radius; x< (int)point.x+radius; x++) 
		{
			for (int y = (int)point.y - radius; y< (int)point.y+radius; y++) 
			{
				for (int z = (int)point.z - radius; z< (int)point.z+radius; z++) 
				{
					if(x< 0 || x > _x || y < 0 || y > _y || z < 0 || z > _z) continue;
					if(spawnMap[x, y, z] == double.MaxValue) return;
					spawnMap[x, y, z] = value;
				}
			}
		}
	}
	void ResetSceneData()
	{
		AreaCenter center = FindObjectOfType<AreaCenter> ();
		transform.position = center.transform.position;
		_setupDataField ();
	}
}
