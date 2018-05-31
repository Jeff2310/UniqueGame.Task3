using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour {
	NavMeshAgent _agent;
	private SpriteRenderer _sprite;
	private SceneNode _targetNode;
	private bool _isMoving;
        
	void Start() {
		_agent = GetComponent<NavMeshAgent>();
		_sprite = GameObject.FindGameObjectWithTag("PlayerSprite").GetComponent<SpriteRenderer>();
		_isMoving = false;
	}
        
	void Update()
	{
		// stop moving
		if (_isMoving && _agent.remainingDistance < 0.0005f) 
		{
			_isMoving = false;
			if (_targetNode == null) return;
			_sprite.flipX = _targetNode.NodePosition.position.x > gameObject.transform.position.x;
		}
		// handle sprite flipping
		
		if (_isMoving)
		{
			_sprite.flipX = _agent.velocity.x > 0;
		}
		
		// handle input
		HandleInput();
	}
	
	private void HandleInput()
	{
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
			{
				if (hit.collider.gameObject.CompareTag("StaticNode") == true)
				{
					_targetNode = hit.collider.gameObject.GetComponent<SceneNode>();
					MoveToMouse(_targetNode.ArriveLocation);
				}
				else
				{
					_targetNode = null;
					MoveToMouse(hit.point);
				}
				_sprite.flipX = hit.point.x > gameObject.transform.position.x;
			}
		}
	}

	private void MoveToMouse(Vector3 destination)
	{
		Vector3 aimVector = destination - gameObject.transform.position;
		_agent.velocity = aimVector.normalized * _agent.speed / 2;
		_agent.destination = destination;
		_isMoving = true;
	}
}