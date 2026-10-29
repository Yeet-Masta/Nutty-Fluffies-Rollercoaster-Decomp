using System;
using Boo.Lang.Runtime;
using UnityEngine;

[Serializable]
public class VehicleFactory
{
	private static VehicleFactory instance;

	public static VehicleFactory getInstance()
	{
		if (RuntimeServices.EqualityOperator(instance, null))
		{
			instance = new VehicleFactory();
		}
		return instance;
	}

	public virtual Vehicle create(GameData.VehicleData data, GameObject parent, Vector2 position)
	{
		Vector3 position2 = new Vector3(position.x, position.y, 0f);
		int trainType = data.trainType;
		Vehicle vehicle = ((Vehicle)UnityEngine.Object.Instantiate(RollerCoasterPrefabs.getInstance().trains[trainType], position2, Quaternion.identity)) as Vehicle;
		float num = (0f - (vehicle.cartPrefab.GetComponent<Collider>() as CapsuleCollider).height) * 0.9f;
		vehicle.transform.parent = parent.transform;
		vehicle.carts = new Cart[data.cartList.Length + 1];
		position.x += num * (float)(data.cartList.Length + 1);
		vehicle.carts[0] = createCart(position, vehicle.driverCartPrefab, trainType);
		vehicle.carts[0].transform.parent = vehicle.transform;
		(vehicle.carts[0] as DriverCart).rocket.SetActiveRecursively(false);
		position.x -= num;
		int num2 = 0;
		for (num2 = 0; num2 < data.cartList.Length - 1; num2++)
		{
			Cart cart = createCart(position, vehicle.cartPrefab, trainType);
			createAnimal(data.cartList[num2].animalIndex, cart);
			cart.transform.parent = vehicle.transform;
			cart.jumpRocket.SetActiveRecursively(false);
			cart.magnet.SetActiveRecursively(false);
			position.x -= num;
			vehicle.carts[num2 + 1] = cart;
		}
		int num3 = vehicle.carts.Length - 1;
		vehicle.carts[num3] = createCart(position, vehicle.frontCartPrefab, trainType);
		vehicle.carts[num3].transform.parent = vehicle.transform;
		vehicle.carts[num3].jumpRocket.SetActiveRecursively(false);
		vehicle.carts[num3].magnet.SetActiveRecursively(false);
		createAnimal(data.cartList[num3 - 1].animalIndex, vehicle.carts[num3]);
		for (num2 = 0; num2 < vehicle.carts.Length - 1; num2++)
		{
			vehicle.carts[num2].indexInVehicle = num2;
			createConnectJoints(vehicle.carts[num2], vehicle.carts[num2 + 1], num2);
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
		int num2 = 1;
		Vector3 anchor = configurableJoint.anchor;
		anchor.x = num2;
		configurableJoint.anchor = anchor;
		float y = -0.5f;
		Vector3 anchor2 = configurableJoint.anchor;
		anchor2.y = y;
		configurableJoint.anchor = anchor2;
		int num5 = 0;
		Vector3 anchor3 = configurableJoint.anchor;
		anchor3.z = num5;
		configurableJoint.anchor = anchor3;
		int num7 = -1;
		Vector3 targetPosition = configurableJoint.targetPosition;
		targetPosition.x = num7;
		configurableJoint.targetPosition = targetPosition;
		float y2 = -0.5f;
		Vector3 targetPosition2 = configurableJoint.targetPosition;
		targetPosition2.y = y2;
		configurableJoint.targetPosition = targetPosition2;
		int num10 = 0;
		Vector3 targetPosition3 = configurableJoint.targetPosition;
		targetPosition3.z = num10;
		configurableJoint.targetPosition = targetPosition3;
		configurableJoint.xMotion = ConfigurableJointMotion.Limited;
		configurableJoint.yMotion = ConfigurableJointMotion.Locked;
		configurableJoint.zMotion = ConfigurableJointMotion.Free;
		configurableJoint.angularXMotion = ConfigurableJointMotion.Free;
		configurableJoint.angularYMotion = ConfigurableJointMotion.Free;
		configurableJoint.angularZMotion = ConfigurableJointMotion.Limited;
		float num12 = 0f;
		SoftJointLimit linearLimit = configurableJoint.linearLimit;
		linearLimit.limit = num12;
		configurableJoint.linearLimit = linearLimit;
		int num15 = 300;
		SoftJointLimitSpring linearLimit2 = configurableJoint.linearLimitSpring;
		linearLimit2.spring = num15;
		configurableJoint.linearLimitSpring = linearLimit2;
		int num18 = 30;
		SoftJointLimitSpring linearLimit3 = configurableJoint.linearLimitSpring;
		linearLimit3.damper = num18;
		configurableJoint.linearLimitSpring = linearLimit3;
		int num21 = 25;
		SoftJointLimit angularZLimit = configurableJoint.angularZLimit;
		angularZLimit.limit = num21;
		configurableJoint.angularZLimit = angularZLimit;
		int num24 = 100000;
		SoftJointLimitSpring angularZLimit2 = configurableJoint.angularYZLimitSpring;
		angularZLimit2.spring = num24;
		configurableJoint.angularYZLimitSpring = angularZLimit2;
		int num27 = 1000;
		SoftJointLimitSpring angularZLimit3 = configurableJoint.angularYZLimitSpring;
		angularZLimit3.damper = num27;
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
		return ((Cart)UnityEngine.Object.Instantiate(prefab, position2, Quaternion.identity)) as Cart;
	}

	private void addSpecials(int special, Vehicle vehicle)
	{
		if (special > -1)
		{
			Special special2 = ((Special)UnityEngine.Object.Instantiate(RollerCoasterPrefabs.getInstance().specials[special], Vector3.zero, Quaternion.identity)) as Special;
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
			GameObject gameObject = ((GameObject)UnityEngine.Object.Instantiate(RollerCoasterPrefabs.getInstance().toys[prefabIndex], Vector3.zero, Quaternion.identity)) as GameObject;
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
			int i = 0;
			Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(Rigidbody));
			for (int length = componentsInChildren.Length; i < length; i++)
			{
				(componentsInChildren[i] as Rigidbody).solverIterations = 1;
			}
		}
	}
}
