using UnityEngine;
using System.Collections;

public class PathContainer : MonoBehaviour {
	public bool loop = true;

	private PathNode[] _nodes;
	// Use this for initialization
	void Awake () {
		_nodes = GetComponentsInChildren<PathNode> ();

		if (_nodes.Length > 1) {
			for (int i=0; i<_nodes.Length; i++) {
				if(loop)
				{
					if(i+1 == _nodes.Length)
					{
						_nodes[i].next = _nodes[0];
					}else{
						_nodes[i].next = _nodes[i+1];
					}
					if(i==0)
					{
						_nodes[i].prev = _nodes[_nodes.Length - 1];
					}else{
						_nodes[i].prev = _nodes[i-1];
					}
				}else{
					if(i+1 == _nodes.Length)
					{
						_nodes[i].next = _nodes[i-1];
					}else{
						_nodes[i].next = _nodes[i+1];
					}
					if(i==0)
					{
						_nodes[i].prev = _nodes[i+1];
					}else{
						_nodes[i].prev = _nodes[i-1];
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnDrawGizmos() {
		if (_nodes == null)
			return;
		Gizmos.color = Color.green;
		for (int i=1; i<_nodes.Length; i++) {
			Gizmos.DrawLine (_nodes[i-1].transform.position, nodes[i].transform.position);
		}
		for (int i=0; i<_nodes.Length; i++) 
		{
			Gizmos.DrawSphere (_nodes[i].transform.position, 0.3f);
		}
	}
	public PathNode[] nodes{
		get {return _nodes;}
	}
	public PathNode GetClosestPath(Vector3 target)
	{
		if (_nodes == null || _nodes.Length == 0)
			return null;

		PathNode closestNode = _nodes [0];
		float dist = Vector3.Distance (closestNode.transform.position, target);
		float tmpDist;
		for (int i=1; i<_nodes.Length; i++) {
			tmpDist = Vector3.Distance(_nodes[i].transform.position, target);
			if(tmpDist < dist)
			{
				dist = tmpDist;
				closestNode = _nodes[i];
			}
		}
		return closestNode;
	}
	public PathNode GetFirstNode()
	{
		if (_nodes == null || _nodes.Length == 0)
			return null;

		return _nodes [0];
	}
}
