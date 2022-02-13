using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// stats des armes du jeu
public class WeaponStats : MonoBehaviour
{
	// nombre total d'armes dans le jeu
	private static int nbWeapon = 5;

	// nombre total d'équipements dans le jeu
	private static int nbEquip = 3;

	// nombre de stats (commun aux items, armes et équipements)
	private static int nbStats = 3;

	// tableau contenant le nom de toutes les armes
	// l'index du nom de l'arme correspond à l'index des stats de cette arme dans 'weaponStats'
	private string[] weaponNames = new string[nbWeapon];

	/* tableau contenant les stats de toutes les armes
	* 1 : objet instancié
	*/
	private float[][] weaponStats = new float[nbWeapon][];

	// tableau contenant le nom de tous les équipements
	// l'index du nom de l'équipement correspond à l'index des stats de cet équipement dans 'equipStats'
	private string[] equipNames = new string[nbEquip];

	// tableau contenant les stats de tous les équipements
	private float[][] equipStats = new float[nbEquip][];

	public static WeaponStats instance;	// instance de la classe


	/* vérifie qu'il y a bien une seule instance de la classe
	* initialise les stats de toutes les armes
	*/
	private void Awake() {
		if (instance != null) {
			Debug.LogWarning("Il y a plus d'une instance de WeaponStats dans la scène.");
			return;
		}
		instance = this;

		// TODO set les stats de toutes les armes et équipements (à faire à chaque début de scène ?? -> opti)
		for(int i = 0; i < nbWeapon; ++i) {
			int ip1 = i + 1;
			float[] tab = {0, 0, 0};
			weaponNames[i] = ("weapon" + ip1.ToString());
			weaponStats[i] = new float[nbStats];
			weaponStats[i] = tab;
		}
		for(int i = 0; i < nbEquip; ++i) {
			int ip1 = i + 1;
			float[] tab = {0, 0, 0};
			equipNames[i] = ("equip" + ip1.ToString());
			equipStats[i] = new float[nbStats];
			equipStats[i] = tab;
		}
	}


	/* Renvoie les stats correspondant à l'arme 's'
	* Si aucune arme ne correspond à la string passée en argument :
	* debug et return null <- TODO perror au lieu de debug
	*/
	public float[] getWeaponStats(string s) {
		for(int i = 0; i < nbWeapon; ++i) {
			if (s.Equals(weaponNames[i])) {
				return weaponStats[i];
			}
		}
		Debug.Log("weapon " + s + " doesnt exist");
		return null;
	}


	/* Renvoie les stats correspondant à l'équipement 's'
	* Si aucun équipement ne correspond à la string passée en argument :
	* debug et return null <- TODO perror au lieu de debug
	*/
	public float[] getEquipStats(string s) {
		for(int i = 0; i < nbEquip; ++i) {
			if (s.Equals(equipNames[i])) {
				return equipStats[i];
			}
		}
		Debug.Log("equipement " + s + " doesnt exist");
		return null;
	}
}
