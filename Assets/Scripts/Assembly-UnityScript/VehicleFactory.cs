using System;
using UnityEngine;

[Serializable]
public class VehicleFactory
{
	private static VehicleFactory instance;

	public static VehicleFactory getInstance()
	{
		if (instance == null)
		{
			instance = new VehicleFactory();
		}
		return instance;
	}

	public virtual Vehicle create(GameData.VehicleData data, GameObject parent, Vector2 position)
	{
		Vector3 position2 = new Vector3(position.x, position.y, 0f);
		int trainType = data.trainType;
		Vehicle vehicle = UnityEngine.Object.Instantiate(RollerCoasterPrefabs.getInstance().trains[trainType], position2, Quaternion.identity);
		float spacing = (0f - (vehicle.cartPrefab.GetComponent<Collider>() as CapsuleCollider).height) * 0.9f;
		vehicle.transform.parent = parent.transform;
		vehicle.carts = new Cart[data.cartList.Length + 1];
		position.x += spacing * (float)(data.cartList.Length + 1);
		vehicle.carts[0] = createCart(position, vehicle.driverCartPrefab, trainType);
		vehicle.carts[0].transform.parent = vehicle.transform;
		(vehicle.carts[0] as DriverCart).rocket.SetActiveRecursively(false);
		position.x -= spacing;
		int i = 0;
		for (i = 0; i < data.cartList.Length - 1; i++)
		{
			Cart cart = createCart(position, vehicle.cartPrefab, trainType);
			createAnimal(data.cartList[i].animalIndex, cart);
			cart.transform.parent = vehicle.transform;
			cart.jumpRocket.SetActiveRecursively(false);
			cart.magnet.SetActiveRecursively(false);
			position.x -= spacing;
			vehicle.carts[i + 1] = cart;
		}
		int frontIndex = vehicle.carts.Length - 1;
		vehicle.carts[frontIndex] = createCart(position, vehicle.frontCartPrefab, trainType);
		vehicle.carts[frontIndex].transform.parent = vehicle.transform;
		vehicle.carts[frontIndex].jumpRocket.SetActiveRecursively(false);
		vehicle.carts[frontIndex].magnet.SetActiveRecursively(false);
		createAnimal(data.cartList[frontIndex - 1].animalIndex, vehicle.carts[frontIndex]);
		for (i = 0; i < vehicle.carts.Length - 1; i++)
		{
			vehicle.carts[i].indexInVehicle = i;
			createConnectJoints(vehicle.carts[i], vehicle.carts[i + 1], i);
		}
		addSpecials(data.specialCart, vehicle);
		vehicle.prepare();
		(vehicle.carts[0] as DriverCart).calculateAverageDownpull();
		vehicle.carts[0].animalIndex = vehicle.carts[1].animalIndex;
		return vehicle;
	}

	private void createConnectJoints(Cart cart1, Cart cart2, int num)
	{
		ConfigurableJoint configurableJoint = (ConfigurableJoint)cart1.gameObject.AddComponent(typeof(ConfigurableJoint));
		Vector3 anchor = configurableJoint.anchor;
		anchor.x = 1;
		configurableJoint.anchor = anchor;
		Vector3 anchor2 = configurableJoint.anchor;
		anchor2.y = -0.5f;
		configurableJoint.anchor = anchor2;
		Vector3 anchor3 = configurableJoint.anchor;
		anchor3.z = 0;
		configurableJoint.anchor = anchor3;
		Vector3 targetPosition = configurableJoint.targetPosition;
		targetPosition.x = -1;
		configurableJoint.targetPosition = targetPosition;
		Vector3 targetPosition2 = configurableJoint.targetPosition;
		targetPosition2.y = -0.5f;
		configurableJoint.targetPosition = targetPosition2;
		Vector3 targetPosition3 = configurableJoint.targetPosition;
		targetPosition3.z = 0;
		configurableJoint.targetPosition = targetPosition3;
		configurableJoint.xMotion = ConfigurableJointMotion.Limited;
		configurableJoint.yMotion = ConfigurableJointMotion.Locked;
		configurableJoint.zMotion = ConfigurableJointMotion.Free;
		configurableJoint.angularXMotion = ConfigurableJointMotion.Free;
		configurableJoint.angularYMotion = ConfigurableJointMotion.Free;
		configurableJoint.angularZMotion = ConfigurableJointMotion.Limited;
		SoftJointLimit linearLimit = configurableJoint.linearLimit;
		linearLimit.limit = 0f;
		configurableJoint.linearLimit = linearLimit;
		SoftJointLimitSpring linearLimit2 = configurableJoint.linearLimitSpring;
		linearLimit2.spring = 300;
		configurableJoint.linearLimitSpring = linearLimit2;
		SoftJointLimitSpring linearLimit3 = configurableJoint.linearLimitSpring;
		linearLimit3.damper = 30;
		configurableJoint.linearLimitSpring = linearLimit3;
		SoftJointLimit angularZLimit = configurableJoint.angularZLimit;
		angularZLimit.limit = 25;
		configurableJoint.angularZLimit = angularZLimit;
		SoftJointLimitSpring angularZLimit2 = configurableJoint.angularYZLimitSpring;
		angularZLimit2.spring = 100000;
		configurableJoint.angularYZLimitSpring = angularZLimit2;
		SoftJointLimitSpring angularZLimit3 = configurableJoint.angularYZLimitSpring;
		angularZLimit3.damper = 1000;
		configurableJoint.angularYZLimitSpring = angularZLimit3;
		configurableJoint.connectedBody = cart2.GetComponent<Rigidbody>();
		cart1.joint = configurableJoint;
	}

	private Cart createCart(GameData.CartData data, Vector2 position, Cart prefab, int themeIndex)
	{
		Cart cart = createCart(position, prefab, themeIndex);
		createAnimal(data.animalIndex, cart);
		return cart;
	}

	private Cart createCart(Vector2 position, Cart prefab, int themeIndex)
	{
		Vector3 position2 = new Vector3(position.x, position.y, 0f);
		return UnityEngine.Object.Instantiate(prefab, position2, Quaternion.identity);
	}

	private void addSpecials(int special, Vehicle vehicle)
	{
		if (special > -1)
		{
			Special special2 = UnityEngine.Object.Instantiate(RollerCoasterPrefabs.getInstance().specials[special], Vector3.zero, Quaternion.identity);
			special2.transform.parent = vehicle.transform;
			special2.transform.localPosition = Vector3.zero;
			special2.trainRef = vehicle;
			special2.prepareGraphcics();
			vehicle.special = special2;
		}
	}

	private void createAnimal(int prefabIndex, Cart cart)
	{
		if (prefabIndex != GameData.ANIMAL_NONE)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(RollerCoasterPrefabs.getInstance().toys[prefabIndex], Vector3.zero, Quaternion.identity);
			gameObject.transform.parent = cart.animalRoot;
			gameObject.transform.localPosition = Vector3.zero;
			Joint component = gameObject.GetComponent<Joint>();
			component.connectedBody = cart.GetComponent<Rigidbody>();
			cart.toyJoint = component;
			cart.animalIndex = prefabIndex;
			cart.itemTr = gameObject.transform;
			cart.animal = gameObject.GetComponent<Animal>();
			cart.animal.cartRef = cart;
			cart.heartValue = GameData.ANIMAL_HEART_VALUE[prefabIndex];
			foreach (Component body in gameObject.GetComponentsInChildren(typeof(Rigidbody)))
			{
				(body as Rigidbody).solverIterations = 1;
			}
		}
	}
}
