using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject toClone;
    public Vector3 localDirection = Vector3.forward;
    public float force = 5;
    public bool shoot;
    public bool timedShoot;
    public float timebetweenShots = 2;
    public float scatter = 5;
    private Coroutine shooterEnum;
    public bool randomRotation = false;
    // Start is called before the first frame update
    void Start() {
        if (localDirection != Vector3.zero) localDirection.Normalize();
        if (timedShoot) {
            shooterEnum = StartCoroutine("TimedShooter");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (shoot) {
            TakeShot();
            shoot = false;
        }

        if(timedShoot && shooterEnum == null) {
            shooterEnum = StartCoroutine("TimedShooter");
        }
    }

    IEnumerator TimedShooter() {
        while (timedShoot) {
            Debug.Log("in ienumb");
            TakeShot();
            yield return new WaitForSeconds(Mathf.Abs( timebetweenShots));
        }
        shooterEnum = null;
    }

    void TakeShot() {
        Debug.Log("in take shot");
        GameObject obj = GameObject.Instantiate(toClone);
        if (obj == null) Debug.Log("oh boyyy");
        obj.transform.position = this.transform.position;
        Rigidbody bod = obj.GetComponent<Rigidbody>();
        if (bod == null) bod = obj.GetComponentInChildren<Rigidbody>();
        if (bod != null) {
            Debug.Log("shot");
            Quaternion scatt = Scatter(this.transform.rotation, scatter);
            obj.transform.rotation = scatt;
            bod.AddForce(obj.transform.TransformDirection(localDirection) * force, ForceMode.Impulse);
            if (randomRotation) {
                obj.transform.rotation =  (Random.rotation);
            }
        } else Debug.Log("oh no");

    }
    

    protected virtual Quaternion Scatter(Quaternion baseRotation, float scatterStrength) {
        if (scatterStrength <= 0) return baseRotation;
        //rotate the base rotation just slightly
        return baseRotation * Quaternion.Euler(Random.Range(-scatterStrength, scatterStrength),
            0/*Random.Range(-scatterStrength, scatterStrength)*/,
            Random.Range(-scatterStrength, scatterStrength));
    }
}
