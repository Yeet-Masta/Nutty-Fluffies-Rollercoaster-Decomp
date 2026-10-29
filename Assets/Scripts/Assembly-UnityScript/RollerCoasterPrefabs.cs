using UnityEngine;

public class RollerCoasterPrefabs : LevelDataPrefabs
{
	public Vehicle[] trains;

	public Cart[] carts;

	public GameObject[] toys;

	public Special[] specials;

	public new static RollerCoasterPrefabs getInstance()
	{
		if (LevelDataPrefabs.instance == null)
		{
			Debug.LogError("ADD RollerCoasterPrefabs prefab TO YOUR SCENE! :(\n");
		}
		return LevelDataPrefabs.instance as RollerCoasterPrefabs;
	}
}
