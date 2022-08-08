using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* gère le déplacement du piège
*/
public class trap_mouvement : MonoBehaviour
{

    public float speed;             // vitesse de déplacement du piège
    public Transform[] waypoints;   // points définissant le chemin parcouru par le piège

    private Transform target;       // transform du point vers lequel le piège se dirige actuellement
    private  int destPoint = 0;     // indice du point vers lequel le piège se dirige


    // init premier point
    void Start() {
        target = waypoints[0];
    }


    /* déplace le piège vers le point courant
    * passe au point suivant si distance < 0.3
    */
    void Update() {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if(Vector3.Distance(transform.position, target.position) < 0.3f) {
            destPoint = ++destPoint % waypoints.Length;
            target = waypoints[destPoint];
        }
    }
}
