using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class PlayerController : MonoBehaviour {

	public Transform playerTransform;
	public Transform rightGunBone;
	public Transform leftGunBone;
	public Arsenal[] arsenal;
	public float BrakeStrength;

	private Animator animator;
	private NavMeshAgent navAgent;
	private bool followPlayer = false;
	private bool shouldStopMoving = false;

	void Awake() 
	{
		animator = GetComponent<Animator> ();
		if (arsenal.Length > 0)
			SetArsenal (arsenal[0].name);

		navAgent = GetComponent<NavMeshAgent>();
	}

	public void SetArsenal(string name) {
		foreach (Arsenal hand in arsenal) {
			if (hand.name == name) {
				if (rightGunBone.childCount > 0)
					Destroy(rightGunBone.GetChild(0).gameObject);
				if (leftGunBone.childCount > 0)
					Destroy(leftGunBone.GetChild(0).gameObject);
				if (hand.rightGun != null) {
					GameObject newRightGun = (GameObject) Instantiate(hand.rightGun);
					newRightGun.transform.parent = rightGunBone;
					newRightGun.transform.localPosition = Vector3.zero;
					newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
					}
				if (hand.leftGun != null) {
					GameObject newLeftGun = (GameObject) Instantiate(hand.leftGun);
					newLeftGun.transform.parent = leftGunBone;
					newLeftGun.transform.localPosition = Vector3.zero;
					newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
				}
				animator.runtimeAnimatorController = hand.controller;
				return;
				}
		}
	}

    private void Update()
    {
        if (followPlayer)
        {
			navAgent.SetDestination(playerTransform.position);

			if (Vector3.SqrMagnitude(playerTransform.position - transform.position) < 15)
            {
				navAgent.velocity = Vector3.Lerp(navAgent.velocity, Vector3.zero, Time.deltaTime * BrakeStrength);
            }

			animator.SetFloat("Speed", navAgent.velocity.magnitude);
        }
        else if (shouldStopMoving)
        {
			navAgent.velocity = Vector3.Lerp(navAgent.velocity, Vector3.zero, Time.deltaTime * BrakeStrength);
            animator.SetFloat("Speed", navAgent.velocity.magnitude);

            if (navAgent.velocity == Vector3.zero)
            {
				shouldStopMoving = false;
            }

		}
	}

    public void FollowPlayer()
    {
		followPlayer = true;
    }

	public void StopFollow()
    {
		followPlayer = false;
		shouldStopMoving = true;
    }

	[System.Serializable]
	public struct Arsenal {
		public string name;
		public GameObject rightGun;
		public GameObject leftGun;
		public RuntimeAnimatorController controller;
	}
}
