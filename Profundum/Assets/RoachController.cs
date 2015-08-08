﻿using UnityEngine;
using System.Collections;

public class RoachController : MonoBehaviour {
	private double[,,] spawnMap; 

	public int resolution = 5;
	public GameObject roachPrefab;
	public int spawnDensity = 1;
	public int spawnResolution = 1;
	public int spawnDistance = 50;

	private int _x, _y, _z;
	private int _worldX, _worldY, _worldZ;
	private RoachLight[] lights;

	// Use this for initialization
	void Start () 
	{

		_x = (int)(GetComponent<Collider> ().bounds.size.x / resolution);
		_y = (int)(GetComponent<Collider> ().bounds.size.y / resolution);
		_z = (int)(GetComponent<Collider> ().bounds.size.z / resolution);

		_worldX = (int)(GetComponent<Collider> ().bounds.center.x - GetComponent<Collider> ().bounds.extents.x);
		_worldY = (int)(GetComponent<Collider> ().bounds.center.y - GetComponent<Collider> ().bounds.extents.y);
		_worldZ = (int)(GetComponent<Collider> ().bounds.center.z - GetComponent<Collider> ().bounds.extents.z);

		spawnMap = new double[_x, _y, _z];

		lights = FindObjectsOfType<RoachLight> ();
		Vector3 pos = new Vector3 ();
		Quaternion rot = new Quaternion ();
		for (int i =0; i<_x; i++) 
		{
			for (int j =0; j<_y; j++) 
			{
				for (int k =0; k<_z; k++) 
				{
					spawnMap[i, j, k] = 0;
					for(int l = 0; l<lights.Length; l++)
					{
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
					/*if(spawnMap[i, j, k] == true)
					{
						pos.x = (_worldX + i * resolution);
						pos.y = (_worldY + j * resolution);
						pos.z = (_worldZ + k * resolution);
						Instantiate(roachPrefab, pos, rot);
					}*/
				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
	public void LightLaunched(GameObject light)
	{
		Vector3 pos = new Vector3 ();
		int x, y, z;
		for (int i = 0; i < spawnDistance; i+= spawnResolution) 
		{
			pos = light.transform.position + light.transform.forward * i;
			//Instantiate(roachPrefab, pos, light.transform.rotation);

			pos.x+=Random.value * 10 - Random.value * 10;
			pos.y+=Random.value * 10 - Random.value * 10;
			pos.z+=Random.value * 10 - Random.value * 10;

			x = (int)((pos.x  - _worldX) / resolution);
			y = (int)((pos.y - _worldY) / resolution);
			 z = (int)((pos.z - _worldZ ) / resolution);



			if(x< 0 || x > _x || y < 0 || y > _y || z < 0 || z > _z) continue;

			if(spawnMap[x, y, z] != double.MaxValue)
			{
				/*pos.x+=Random.value * 8;
				pos.y+=Random.value * 8;
				pos.z+=Random.value * 8;*/

				Instantiate(roachPrefab, pos, light.transform.rotation);
			}

			/*pos = new Vector3(
				(float)(light.transform.position.x + i * Mathf.Cos(light.transform.rotation.x) * Mathf.Cos(light.transform.rotation.y)), 
				(float)(light.transform.position.y  - i * Mathf.Sin(light.transform.rotation.x)),
				(float)(light.transform.position.z+ i *Mathf.Cos(light.transform.rotation.x) * Mathf.Sin(light.transform.rotation.y)));
			Instantiate(roachPrefab, pos, light.transform.rotation);*/

			/*pos = new Vector3(
				0, 
				0,
				(float)(light.transform.position.z+ i *Mathf.Cos(light.transform.rotation.x) * Mathf.Sin(light.transform.rotation.y)));
			Instantiate(roachPrefab, pos, light.transform.rotation);*/

			//z axis rotation
			/*offset.x = offset.x * Mathf.Cos(theta_z) - offset.y * Mathf.Sin(theta_z);
			offset.y = offset.x * Mathf.Sin(theta_z) + offset.y * Mathf.Cos(theta_z);
			
			//x axis rotation
			offset.y = offset.y * Mathf.Cos (theta_x) - offset.z * Mathf.Sin(theta_x);
			offset.z = offset.y * Mathf.Sin(theta_x) + offset.z * Mathf.Cos(theta_x);
			
			//y axis rotation
			offset.z = offset.z * Mathf.Cos (theta_y) - offset.x * Mathf.Sin(theta_y);
			offset.x = offset.z * Mathf.Sin(theta_y) + offset.x * Mathf.Cos(theta_y);*/
		}

	}
}
