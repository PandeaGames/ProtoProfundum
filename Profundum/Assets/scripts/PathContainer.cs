using UnityEngine;
using System.Collections;

public class PathContainer : MonoBehaviour {
	public int loop = 1;
	public bool closedLoop = true;
	public GameObject[] objects;

	private PathNode[] _nodes;
	private PathNode _head;
	private PathNode _tail;
	private PathObject[] _pathObjects;
	private PathGroup _pathGroup;

	// Use this for initialization
	void Awake () {
		_pathObjects = new PathObject[objects.Length];
		_nodes = GetComponentsInChildren<PathNode> ();

		if (_nodes.Length > 1) {
			_head = _nodes[0];
			_tail = _nodes[_nodes.Length - 1];
			for (int i=0; i<_nodes.Length; i++) {
				if(closedLoop)
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

		for(int i=0;i<objects.Length;i++)
		{
			if(!objects[i].GetComponent<PathObject>())
			{
				objects[i].AddComponent<PathObject>();
			}
			_pathObjects[i] = objects[i].GetComponent<PathObject>();
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
	public void Seek(float pos, float targetTime)
	{
		PathNode node;
		PathObject obj;
		float time, tmpTime;
		PathNode next;
		bool looping = false;
		bool reversing = false;
        float localTargetTime = 0f;
		int nodeIterator;
		int jStop;
		for (int i =0; i<_pathObjects.Length; i++) 
		{
			
			obj = _pathObjects[i];
            time = 0;
            localTargetTime = targetTime + obj.timeOffset;

            for (int k = 0; k < loop; k++)
			{
				nodeIterator = 1;
				reversing = false;
				jStop = 0;
				for (int j =0; !closedLoop ? j>=jStop:j< _nodes.Length; j+=nodeIterator) 
				{
					if(!closedLoop && j == _nodes.Length - 1 && !reversing)
					{
						//if it is a closed loop, then we never want to reverse. ONly close the loop. 
						//if it is NOT a closed loop, object neds to reverse through the path. 
						reversing = true;
						j = _nodes.Length;
						nodeIterator = -1;
						jStop = 1;
						continue;
					}
					node = _nodes[j];
					next = reversing ? node.prev:node.next;
					time+=node.stopTime;
					if(time> localTargetTime)
					{
						//stop object on path node. he needs to wait. 
						obj.SetPosition(node.transform.position);
						break;
					}
                    if(next == null)
                    {
                        Debug.Log(node);
                    }
					tmpTime = Vector3.Distance(node.transform.position, next.transform.position) / obj.speed;
					if(time + tmpTime> localTargetTime)
					{
						//position object between nodes
						Vector3 delta =  next.transform.position - node.transform.position;
						float seekPosition = (localTargetTime - time)/tmpTime;
						Vector3 deltaScaled = delta * seekPosition;
						Vector3 finalPos = node.transform.position + deltaScaled;
						obj.SetPosition(finalPos);
						time += tmpTime;
						break;
					}
					time += tmpTime;

				}
				if(time > localTargetTime)
				{
					//this means we have found our place within this loop. break out and stop searching
					break;
				}
			}
		}
	}
	public PathGroup pathGroup{
		get {return _pathGroup;}
		set { _pathGroup = value;}
	}
}
