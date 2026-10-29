using System;
using Boo.Lang.Runtime;
using UnityEngine;

[Serializable]
public class LevelObjectFactory
{
	public static float STATIC_OBJECT_DEPTH = 200f;

	public static float DYNAMIC_OBJECT_DEPTH = 2f;

	public static bool USE_CAPSULE_COLLIDERS;

	public static int RENDER_QUEUE;

	public static int LAYER;

	public static bool USE_ALPHA_SHADER = true;

	private static LevelObjectFactory instance;

	private LevelDataPrefabs prefabs;

	private LevelLayer currentLayerObject;

	private LevelObjectData currentObjectData;

	public static LevelObjectFactory getInstance()
	{
		if (instance == null)
		{
			instance = new LevelObjectFactory();
		}
		instance.prefabs = LevelDataPrefabs.getInstance();
		return instance;
	}

	public virtual LevelDataPrefabs getPrefabs()
	{
		return prefabs;
	}

	private static int SIGN(int layer)
	{
		return layer != 0 ? (layer < 0 ? -1 : (layer > 0 ? 1 : 0)) : 0;
	}

	public static void setRenderQueueForLayer(int layer)
	{
		RENDER_QUEUE = 3000 + layer * 10 + SIGN(layer) * 100;
	}

	public virtual LevelEntity create(LevelEntityData data, GameObject parent, ObjectListData objects)
	{
		LevelEntity levelEntity = null;
		object result;
		if (CommonData.EDITOR_ACTIVE || !data.checkDevice(1 << CommonData.getDeviceGenerationInt()))
		{
			if ((LevelLayer)parent.GetComponent(typeof(LevelLayer)) != null)
			{
				currentLayerObject = (LevelLayer)parent.GetComponent(typeof(LevelLayer));
			}
			if (objects == null)
			{
				Debug.LogError("OBJECTS IS NULL\n");
			}
			if (data == null)
			{
				Debug.LogError("DATA IS NULL\n");
			}
			switch (data.type)
			{
			case 1:
				levelEntity = createTrigger(data as LevelTriggerData, parent);
				if ((bool)levelEntity)
				{
					levelEntity.gameObject.active = true;
				}
				break;
			case 2:
				levelEntity = createObject(data as LevelObjectData, parent, objects);
				break;
			case 3:
				createJoint(data as LevelJointData, parent);
				break;
			case 5:
				levelEntity = createGroup(data as LevelGroupData, parent);
				break;
			case 6:
				levelEntity = createSpline(data as LevelSplineData, parent);
				break;
			}
			if (levelEntity != null)
			{
				levelEntity.deviceList = data.devices;
				levelEntity.referenceId = data.referenceId;
				if (data.scriptAttachment != null && data.scriptAttachment.Length > 0)
				{
					ObjectScript objectScript = levelEntity.gameObject.AddComponentByName(data.scriptAttachment) as ObjectScript;
					objectScript.entity = levelEntity;
					levelEntity.scriptParams = data.scriptParams;
					if (levelEntity is LevelObject)
					{
						(levelEntity as LevelObject).setFlag(4194304, true);
					}
				}
			}
			result = levelEntity;
		}
		else
		{
			result = null;
		}
		return (LevelEntity)result;
	}

	public virtual LevelTrigger createTrigger(LevelTriggerData triggerData, GameObject parent)
	{
		CustomProfiler.begin("LevelObjectFactory::Instantiate(triggerPrefab)");
		GameObject gameObject = ((GameObject)UnityEngine.Object.Instantiate(prefabs.triggerPrefab, triggerData.position, createRotation(triggerData.rotation))) as GameObject;
		CustomProfiler.end("LevelObjectFactory::Instantiate(triggerPrefab)");
		if (triggerData.triggerType == 2)
		{
			SphereCollider sphereCollider = gameObject.gameObject.AddComponent<SphereCollider>();
			sphereCollider.radius = 1f;
			sphereCollider.isTrigger = true;
		}
		else if (triggerData.triggerType == 1)
		{
			BoxCollider boxCollider = gameObject.gameObject.AddComponent<BoxCollider>();
			int num = 1;
			Vector3 size = boxCollider.size;
			size.x = num;
			boxCollider.size = size;
			int num3 = 1;
			Vector3 size2 = boxCollider.size;
			size2.y = num3;
			boxCollider.size = size2;
			boxCollider.isTrigger = true;
		}
		else
		{
			Debug.LogError("UNKNOWN TRIGGER TYPE");
		}
		object result;
		if (NFPortLegacyDevice.generation == NFPortLegacyDeviceGeneration.iPhone3GS)
		{
			if (triggerData.triggerScript == "TriggerParticles")
			{
				result = null;
			}
			else
			{
				if (triggerData.triggerScript != "TriggerActivateObject")
				{
					goto IL_017d;
				}
				result = null;
			}
		}
		else
		{
			if ((NFPortLegacyDevice.generation != NFPortLegacyDeviceGeneration.iPhone4 && NFPortLegacyDevice.generation != NFPortLegacyDeviceGeneration.iPodTouch4Gen) || triggerData.triggerScript != "TriggerParticles" || GameData.getActiveLevelPackIndex() != 4)
			{
				goto IL_017d;
			}
			result = null;
		}
		goto IL_01e2;
		IL_01e2:
		return (LevelTrigger)result;
		IL_017d:
		gameObject.transform.localScale = new Vector3(triggerData.size.x, triggerData.size.y, 100f);
		LevelTrigger levelTrigger = gameObject.AddComponentByName(triggerData.triggerScript) as LevelTrigger;
		levelTrigger.sourceParams = triggerData.triggerParam;
		levelTrigger.gameObject.active = true;
		gameObject.transform.parent = parent.transform;
		result = gameObject.GetComponent<LevelTrigger>();
		goto IL_01e2;
	}

	public virtual LevelObject createObject(LevelObjectData data, GameObject parent, ObjectListData objects)
	{
		ObjectData objectData = objects.getObject(data.objectId);
		LevelObject levelObject = null;
		ObjectPackData objectPackData = null;
		object result;
		if (data.objectId != 1170 && data.objectId != 1137 && data.objectId != 1025 && data.objectId != 1169 && data.objectId != 1136 && data.objectId != 1037 && data.objectId != 1168 && data.objectId != 1135 && data.objectId != 1058 && data.objectId != 1167 && data.objectId != 1134 && data.objectId != 1066 && (NFPortLegacyDevice.generation != NFPortLegacyDeviceGeneration.iPhone3GS || (data.objectId != 1016 && data.objectId != 1017 && data.objectId != 1053 && data.objectId != 1054 && data.objectId != 1102 && data.objectId != 1103 && data.objectId != 1088 && data.objectId != 1089 && data.objectId != 1090 && data.objectId != 1150 && data.objectId != 1151 && data.objectId != 1055 && data.objectId != 1107 && data.objectId != 1106 && data.objectId != 1096 && data.objectId != 1097)) && (CommonData.EDITOR_ACTIVE || !objectData.checkDevice(1 << CommonData.getDeviceGenerationInt())))
		{
			LevelObjectData levelObjectData = currentObjectData;
			currentObjectData = data;
			objectPackData = objects.getPackForObjectId(data.objectId);
			if (objectData != null)
			{
				if (objectData.getNumParts() == 0)
				{
					result = null;
				}
				else
				{
					bool flag = createObjectComponents(objectPackData, null, objectData, data, true);
					if (flag)
					{
						levelObject = createObjectInstance(data, objects, objectData);
						levelObject.objectId = data.objectId;
						levelObject.transform.parent = parent.transform;
						levelObject.transform.localScale = Vector3.one;
						levelObject.flags = data.flags;
					}
					bool flag2 = false;
					if (objectData.getNumParts() != 0)
					{
						flag2 = createObjectComponents(objectPackData, levelObject, objectData, data, false);
					}
					if (data.childList != null)
					{
						foreach (LevelEntityData levelEntityData in data.childList)
						{
							create(levelEntityData, levelObject.gameObject, objects);
						}
					}
					if (flag)
					{
						objectData.cloneRef = levelObject;
					}
					if (!flag2)
					{
						result = null;
					}
					else
					{
						levelObject.name += " (KEEP ME)";
						int num = 0;
						Vector3 localPosition = levelObject.transform.localPosition;
						localPosition.z = num;
						levelObject.transform.localPosition = localPosition;
						float x = Mathf.Abs(levelObject.transform.localScale.x);
						Vector3 localScale = levelObject.transform.localScale;
						localScale.x = x;
						levelObject.transform.localScale = localScale;
						float y = Mathf.Abs(levelObject.transform.localScale.y);
						Vector3 localScale2 = levelObject.transform.localScale;
						localScale2.y = y;
						levelObject.transform.localScale = localScale2;
						if ((data.flags & 0x80000) != 0)
						{
							float x2 = levelObject.transform.localScale.x * -1f;
							Vector3 localScale3 = levelObject.transform.localScale;
							localScale3.x = x2;
							levelObject.transform.localScale = localScale3;
						}
						if ((data.flags & 0x100000) != 0)
						{
							float y2 = levelObject.transform.localScale.y * -1f;
							Vector3 localScale4 = levelObject.transform.localScale;
							localScale4.y = y2;
							levelObject.transform.localScale = localScale4;
						}
						if (((LevelLayer)parent.GetComponent(typeof(LevelLayer))).checkFlag(1))
						{
							float x3 = levelObject.transform.localScale.x * 2f;
							Vector3 localScale5 = levelObject.transform.localScale;
							localScale5.x = x3;
							levelObject.transform.localScale = localScale5;
							float y3 = levelObject.transform.localScale.y * 2f;
							Vector3 localScale6 = levelObject.transform.localScale;
							localScale6.y = y3;
							levelObject.transform.localScale = localScale6;
						}
						levelObject.setZstep(data.zDepth);
						levelObject.savePosition(null);
						if ((data.flags & 0x20000) != 0)
						{
							levelObject.setFlag(4194304, true);
						}
						currentObjectData = levelObjectData;
						result = levelObject;
					}
				}
			}
			else
			{
				Debug.LogError("Object " + data.objectId + " not found");
				currentObjectData = levelObjectData;
				result = null;
			}
		}
		else
		{
			result = null;
		}
		return (LevelObject)result;
	}

	public virtual LevelObject createObjectInstance(LevelObjectData data, ObjectListData objectData, ObjectData odata)
	{
		GameObject gameObject = null;
		gameObject = (odata.flags & ObjectData.FLAG_IS_OPTIMIZED) == 0 ? prefabs.objectPrefab.gameObject : prefabs.objectWithTexturePrefab.gameObject;
		if (gameObject == null)
		{
			Debug.LogError("object prefab is null");
		}
		CustomProfiler.begin("LevelObjectFactory::Instantiate(objectPrefab)");
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject, data.position, Quaternion.identity);
		CustomProfiler.end("LevelObjectFactory::Instantiate(objectPrefab)");
		float rotation = data.rotation;
		Vector3 localEulerAngles = gameObject2.transform.localEulerAngles;
		localEulerAngles.z = rotation;
		gameObject2.transform.localEulerAngles = localEulerAngles;
		gameObject2.transform.localScale = Vector3.one;
		if ((data.flags & 0x20000) != 0)
		{
			Rigidbody rigidbody = gameObject2.gameObject.AddComponent<Rigidbody>();
			rigidbody.constraints = (RigidbodyConstraints)56;
			rigidbody.mass = odata.mass;
			if ((data.flags & 0x40000) != 0)
			{
				rigidbody.Sleep();
			}
		}
		return gameObject2.GetComponent<LevelObject>();
	}

	public virtual bool createObjectComponents(ObjectPackData pack, LevelObject @object, ObjectData objectData, LevelObjectData data, bool simulate)
	{
		bool result = false;
		bool flag = (objectData.flags & ObjectData.FLAG_IS_OPTIMIZED) != 0;
		bool flag2 = (data.flags & 0x20000) != 0;
		float num = 0f;
		bool flag3 = (data.flags & 0x10000) != 0;
		if (!flag3)
		{
			flag3 = (data.flags & 0x800000) != 0;
		}
		if (!flag3)
		{
			flag3 = CommonData.EDITOR_ACTIVE;
		}
		bool flag4 = flag3;
		foreach (ObjectPartData objectPartData in objectData.getParts())
		{
			if (flag4)
			{
				if (!simulate)
				{
					if (objectPartData is ObjectPartBoxCollisionData)
					{
						createBoxCollision(@object ? @object.gameObject : null, objectPartData as ObjectPartBoxCollisionData, flag2);
					}
					else if (objectPartData is ObjectPartSphereCollisionData)
					{
						if (!USE_CAPSULE_COLLIDERS || flag2)
						{
							createSphereCollision(@object ? @object.gameObject : null, objectPartData as ObjectPartSphereCollisionData, flag2);
						}
						else
						{
							createCapsuleCollision(@object ? @object.gameObject : null, objectPartData as ObjectPartSphereCollisionData, flag2);
						}
					}
				}
				result = true;
			}
			if (objectPartData is ObjectPartTextureData)
			{
				if (createTexture(pack, @object ? @object.gameObject : null, objectPartData as ObjectPartTextureData, flag, num, objectData, data, simulate))
				{
					result = true;
				}
				num += 0.05f;
			}
			else if (objectPartData is ObjectPartSphereTriggerData)
			{
				if (!simulate)
				{
					createTriggerPart(@object ? @object.gameObject : null, objectPartData as ObjectPartSphereTriggerData);
				}
				result = true;
			}
			else if (objectPartData is ObjectPartBoxTriggerData)
			{
				if (!simulate)
				{
					createTriggerPart(@object ? @object.gameObject : null, objectPartData as ObjectPartBoxTriggerData);
				}
				result = true;
			}
		}
		if (flag && !simulate && @object != null)
		{
			fixOptimizedScales(@object);
		}
		return result;
	}

	public virtual void createJoint(LevelJointData jointData, GameObject target)
	{
		Joint joint = null;
		switch (jointData.type)
		{
		case 1:
			joint = target.AddComponent<FixedJoint>();
			break;
		case 2:
			joint = target.AddComponent<SpringJoint>();
			break;
		case 3:
			joint = target.AddComponent<HingeJoint>();
			break;
		default:
			Debug.LogError("illegal joint type");
			break;
		}
		LevelJoint levelJoint = target.AddComponent<LevelJoint>() as LevelJoint;
		levelJoint.connectReferenceId = jointData.connectionRef;
		joint.axis = Vector3.forward;
		float x = jointData.anchor.x;
		Vector3 anchor = joint.anchor;
		anchor.x = x;
		joint.anchor = anchor;
		float y = jointData.anchor.y;
		Vector3 anchor2 = joint.anchor;
		anchor2.y = y;
		joint.anchor = anchor2;
	}

	public virtual LevelGroup createGroup(LevelGroupData data, GameObject parent)
	{
		return ((LevelGroup)UnityEngine.Object.Instantiate(prefabs.groupPrefab, data.position, createRotation(data.rotation))) as LevelGroup;
	}

	public virtual Quaternion createRotation(float rotation)
	{
		return Quaternion.AngleAxis(rotation, Vector3.forward);
	}

	public virtual bool createTexture(ObjectPackData pack, GameObject parent, ObjectPartTextureData data, bool toParent, float zAdd, ObjectData objectData, LevelObjectData levelObjectData, bool simulate)
	{
		bool flag = false;
		bool result;
		if (data.checkFlag(ObjectPartData.Flags.TEXTURE_EDITOR_ONLY) && !CommonData.EDITOR_ACTIVE)
		{
			result = flag;
		}
		else
		{
			GameObject gameObject = null;
			bool flag2 = !CommonData.EDITOR_ACTIVE;
			foreach (ObjectPartData objectPartData in objectData.getParts())
			{
				if (objectPartData.type == 5 || objectPartData.type == 6)
				{
					flag2 = false;
				}
			}
			if (currentObjectData.scriptAttachment != null && currentObjectData.scriptAttachment.Length > 0)
			{
				flag2 = false;
			}
			if ((currentObjectData.flags & 0x20000) != 0)
			{
				flag2 = false;
			}
			if (data.allowStretch)
			{
				flag2 = false;
			}
			if (!flag2)
			{
				if (!simulate)
				{
					if (toParent)
					{
						gameObject = parent;
					}
					else
					{
						CustomProfiler.begin("LevelObjectFactory::Instantiate(texturePrefab)");
						gameObject = (GameObject)UnityEngine.Object.Instantiate(prefabs.objectTexturePrefab, Vector3.zero, Quaternion.identity);
						CustomProfiler.end("LevelObjectFactory::Instantiate(texturePrefab)");
						gameObject.transform.parent = parent.transform;
						float x = data.origin.x;
						Vector3 localPosition = gameObject.transform.localPosition;
						localPosition.x = x;
						gameObject.transform.localPosition = localPosition;
						float y = data.origin.y;
						Vector3 localPosition2 = gameObject.transform.localPosition;
						localPosition2.y = y;
						gameObject.transform.localPosition = localPosition2;
						Vector3 localPosition3 = gameObject.transform.localPosition;
						localPosition3.z = zAdd;
						gameObject.transform.localPosition = localPosition3;
						float z = parent.transform.eulerAngles.z + data.rotation;
						Vector3 eulerAngles = gameObject.transform.eulerAngles;
						eulerAngles.z = z;
						gameObject.transform.eulerAngles = eulerAngles;
					}
				}
				flag = true;
			}
			else
			{
				flag = false;
			}
			LevelObject levelObject = null;
			if (!simulate)
			{
				levelObject = parent ? ((LevelObject)parent.GetComponent(typeof(LevelObject))) : null;
				TextureManager.UV uV = new TextureManager.UV();
				Texture2D texture2D = TextureManager.getInstance().createTexture(pack == null ? string.Empty : pack.fullPath, TextureManager.TextureType.OBJECT, data.texture, uV);
				if (gameObject != null)
				{
					USE_ALPHA_SHADER = !data.allowStretch;
					gameObject.GetComponent<Renderer>().sharedMaterial = MaterialManager.getInstance().getMaterialForUniqueID(texture2D, LAYER, USE_ALPHA_SHADER);
					gameObject.GetComponent<Renderer>().sharedMaterial.renderQueue = RENDER_QUEUE;
					USE_ALPHA_SHADER = true;
				}
				int repeat = levelObjectData.getRepeat(ObjectData.FLAG_X_REPEAT_MASK);
				int repeat2 = levelObjectData.getRepeat(ObjectData.FLAG_Y_REPEAT_MASK);
				if (repeat > 0 || repeat2 > 0)
				{
					texture2D.wrapMode = TextureWrapMode.Repeat;
				}
				Mesh mesh = null;
				if (gameObject != null)
				{
					mesh = new Mesh();
					(((MeshFilter)gameObject.GetComponent(typeof(MeshFilter))) as MeshFilter).mesh = mesh;
				}
				Vector3[] array = new Vector3[4]
				{
					new Vector3(-0.5f, -0.5f),
					new Vector3(0.5f + (float)repeat, -0.5f),
					new Vector3(-0.5f, 0.5f + (float)repeat2),
					new Vector3(0.5f + (float)repeat, 0.5f + (float)repeat2)
				};
				Vector2[] array2 = new Vector2[4]
				{
					new Vector2(uV.u1, 1f - uV.v2 * (float)(1 + repeat2)),
					new Vector2(uV.u2 * (float)(1 + repeat), 1f - uV.v2 * (float)(1 + repeat2)),
					new Vector2(uV.u1, 1f - uV.v1),
					new Vector2(uV.u2 * (float)(1 + repeat), 1f - uV.v1)
				};
				int[] array3 = new int[6] { 2, 1, 0, 3, 1, 2 };
				if (gameObject != null)
				{
					mesh.vertices = array;
					mesh.uv = array2;
					mesh.triangles = array3;
					applyLayerColorTo(gameObject);
					float x2 = data.stretchedScale.x * data.textureSourceSize.x / 256f;
					Vector3 localScale = gameObject.transform.localScale;
					localScale.x = x2;
					gameObject.transform.localScale = localScale;
					float y2 = data.stretchedScale.y * data.textureSourceSize.y / 256f;
					Vector3 localScale2 = gameObject.transform.localScale;
					localScale2.y = y2;
					gameObject.transform.localScale = localScale2;
					float x3 = gameObject.transform.localScale.x;
					Vector3 localScale3 = gameObject.transform.localScale;
					localScale3.z = x3;
					gameObject.transform.localScale = localScale3;
					if (data.checkFlag(ObjectPartData.Flags.TEXTURE_FLIP_X))
					{
						float x4 = 0f - Mathf.Abs(gameObject.transform.localScale.x);
						Vector3 localScale4 = gameObject.transform.localScale;
						localScale4.x = x4;
						gameObject.transform.localScale = localScale4;
					}
					if (data.checkFlag(ObjectPartData.Flags.TEXTURE_FLIP_Y))
					{
						float y3 = 0f - Mathf.Abs(gameObject.transform.localScale.y);
						Vector3 localScale5 = gameObject.transform.localScale;
						localScale5.y = y3;
						gameObject.transform.localScale = localScale5;
					}
				}
				else
				{
					if ((bool)parent && (bool)(MeshFilter)parent.GetComponent(typeof(MeshFilter)))
					{
						UnityEngine.Object.Destroy((MeshFilter)parent.GetComponent(typeof(MeshFilter)));
					}
					if ((bool)parent && (bool)(MeshRenderer)parent.GetComponent(typeof(MeshRenderer)))
					{
						UnityEngine.Object.Destroy((MeshRenderer)parent.GetComponent(typeof(MeshRenderer)));
					}
					for (int i = 0; i < array.Length; i++)
					{
						array[i].x = array[i].x * (data.stretchedScale.x * data.textureSourceSize.x / 256f);
						array[i].y = array[i].y * (data.stretchedScale.y * data.textureSourceSize.y / 256f);
						if (data.checkFlag(ObjectPartData.Flags.TEXTURE_FLIP_X))
						{
							array[i].x = array[i].x * -1f;
						}
						if (data.checkFlag(ObjectPartData.Flags.TEXTURE_FLIP_Y))
						{
							array[i].y = array[i].y * -1f;
						}
						Vector3 vector19 = array[i];
						float f = data.rotation * ((float)Math.PI / 180f);
						array[i].x = vector19.x * Mathf.Cos(f) - vector19.y * Mathf.Sin(f);
						array[i].y = vector19.y * Mathf.Cos(f) + vector19.x * Mathf.Sin(f);
						array[i].x = array[i].x + data.origin.x;
						array[i].y = array[i].y + data.origin.y;
						if (currentLayerObject.checkFlag(1))
						{
							array[i].x = array[i].x * 2f;
							array[i].y = array[i].y * 2f;
						}
						if ((currentObjectData.flags & 0x80000) != 0)
						{
							array[i].x = array[i].x * -1f;
						}
						if ((currentObjectData.flags & 0x100000) != 0)
						{
							array[i].y = array[i].y * -1f;
						}
						Vector3 vector20 = array[i];
						float f2 = levelObjectData.rotation * ((float)Math.PI / 180f);
						array[i].x = vector20.x * Mathf.Cos(f2) - vector20.y * Mathf.Sin(f2);
						array[i].y = vector20.y * Mathf.Cos(f2) + vector20.x * Mathf.Sin(f2);
						array[i].x = array[i].x + levelObjectData.position.x;
						array[i].y = array[i].y + levelObjectData.position.y;
						array[i].z = zAdd;
					}
					currentLayerObject.addTextureMesh(texture2D, currentObjectData.zDepth, zAdd, RENDER_QUEUE, array, array2, array3);
				}
			}
			result = flag;
		}
		return result;
	}

	public static void applyLayerColorTo(GameObject obj)
	{
		LevelLayer levelLayer = null;
		Transform parent = obj.transform.parent;
		while (levelLayer == null && parent != null)
		{
			levelLayer = ((LevelLayer)parent.GetComponent(typeof(LevelLayer))) as LevelLayer;
			parent = parent.parent;
		}
		if (levelLayer != null)
		{
			return;
		}
		Level level = (Level)UnityEngine.Object.FindObjectOfType(typeof(Level));
		Component[] componentsInChildren = level.GetComponentsInChildren(typeof(LevelLayer), true);
		int i = 0;
		Component[] array = componentsInChildren;
		for (int length = array.Length; i < length; i++)
		{
			if ((array[i] as LevelLayer).layer == 0)
			{
				levelLayer = array[i] as LevelLayer;
			}
		}
	}

	public virtual void createTriggerPart(GameObject parent, ObjectPartTriggerData trig)
	{
		CustomProfiler.begin("LevelObjectFactory::Instantiate(triggerPrefab:part)");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(prefabs.triggerPrefab, Vector3.zero, Quaternion.identity);
		CustomProfiler.end("LevelObjectFactory::Instantiate(triggerPrefab:part)");
		if (trig is ObjectPartSphereTriggerData)
		{
			SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
			sphereCollider.radius = (trig as ObjectPartSphereTriggerData).radius;
			sphereCollider.material = prefabs.physicMaterials[0];
			sphereCollider.isTrigger = true;
		}
		if (trig is ObjectPartBoxTriggerData)
		{
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
			float x = (trig as ObjectPartBoxTriggerData).size.x;
			Vector3 size = boxCollider.size;
			size.x = x;
			boxCollider.size = size;
			float y = (trig as ObjectPartBoxTriggerData).size.y;
			Vector3 size2 = boxCollider.size;
			size2.y = y;
			boxCollider.size = size2;
			float sTATIC_OBJECT_DEPTH = STATIC_OBJECT_DEPTH;
			Vector3 size3 = boxCollider.size;
			size3.z = sTATIC_OBJECT_DEPTH;
			boxCollider.size = size3;
			boxCollider.material = prefabs.physicMaterials[0];
			boxCollider.isTrigger = true;
		}
		gameObject.transform.parent = parent.transform;
		float x2 = trig.origin.x;
		Vector3 localPosition = gameObject.transform.localPosition;
		localPosition.x = x2;
		gameObject.transform.localPosition = localPosition;
		float y2 = trig.origin.y;
		Vector3 localPosition2 = gameObject.transform.localPosition;
		localPosition2.y = y2;
		gameObject.transform.localPosition = localPosition2;
		float z = 0f;
		Vector3 localPosition3 = gameObject.transform.localPosition;
		localPosition3.z = z;
		gameObject.transform.localPosition = localPosition3;
		LevelTrigger levelTrigger = gameObject.AddComponentByName(trig.script) as LevelTrigger;
		levelTrigger.sourceParams = trig.@params;
		float z2 = parent.transform.eulerAngles.z + trig.rotation;
		Vector3 eulerAngles = gameObject.transform.eulerAngles;
		eulerAngles.z = z2;
		gameObject.transform.eulerAngles = eulerAngles;
	}

	public virtual void createBoxCollision(GameObject parent, ObjectPartBoxCollisionData box, bool dynamic)
	{
		GameObject gameObject = null;
		if (box.rotation == 0f && !hasCollider(parent))
		{
			gameObject = parent;
		}
		else
		{
			CustomProfiler.begin("LevelObjectFactory::Instantiate(objectColliderPrefab)");
			gameObject = (GameObject)UnityEngine.Object.Instantiate(prefabs.objectColliderPrefab, Vector3.zero, Quaternion.identity);
			CustomProfiler.end("LevelObjectFactory::Instantiate(objectColliderPrefab)");
		}
		BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
		float x = box.size.x;
		Vector3 size = boxCollider.size;
		size.x = x;
		boxCollider.size = size;
		float y = box.size.y;
		Vector3 size2 = boxCollider.size;
		size2.y = y;
		boxCollider.size = size2;
		if (dynamic)
		{
			float sTATIC_OBJECT_DEPTH = STATIC_OBJECT_DEPTH;
			Vector3 size3 = boxCollider.size;
			size3.z = sTATIC_OBJECT_DEPTH;
			boxCollider.size = size3;
		}
		else
		{
			float sTATIC_OBJECT_DEPTH2 = STATIC_OBJECT_DEPTH;
			Vector3 size4 = boxCollider.size;
			size4.z = sTATIC_OBJECT_DEPTH2;
			boxCollider.size = size4;
		}
		boxCollider.material = prefabs.physicMaterials[0];
		gameObject.layer = currentObjectData.checkFlag(8388608) ? 0 : 12;
		gameObject.transform.parent = parent.transform;
		if (gameObject != parent)
		{
			float z = parent.transform.eulerAngles.z + box.rotation;
			Vector3 eulerAngles = gameObject.transform.eulerAngles;
			eulerAngles.z = z;
			gameObject.transform.eulerAngles = eulerAngles;
			float x2 = box.origin.x;
			Vector3 localPosition = gameObject.transform.localPosition;
			localPosition.x = x2;
			gameObject.transform.localPosition = localPosition;
			float y2 = box.origin.y;
			Vector3 localPosition2 = gameObject.transform.localPosition;
			localPosition2.y = y2;
			gameObject.transform.localPosition = localPosition2;
			float z2 = 0f;
			Vector3 localPosition3 = gameObject.transform.localPosition;
			localPosition3.z = z2;
			gameObject.transform.localPosition = localPosition3;
		}
		else
		{
			float x3 = box.origin.x;
			Vector3 center = boxCollider.center;
			center.x = x3;
			boxCollider.center = center;
			float y3 = box.origin.y;
			Vector3 center2 = boxCollider.center;
			center2.y = y3;
			boxCollider.center = center2;
		}
	}

	public virtual void createSphereCollision(GameObject parent, ObjectPartSphereCollisionData sphere, bool dynamic)
	{
		GameObject gameObject = null;
		if (!hasCollider(parent))
		{
			gameObject = parent;
		}
		else
		{
			CustomProfiler.begin("LevelObjectFactory::Instantiate(objectColliderPrefab)");
			gameObject = (GameObject)UnityEngine.Object.Instantiate(prefabs.objectColliderPrefab, Vector3.zero, Quaternion.identity);
			CustomProfiler.end("LevelObjectFactory::Instantiate(objectColliderPrefab)");
		}
		SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
		sphereCollider.radius = sphere.radius;
		sphereCollider.material = prefabs.physicMaterials[0];
		gameObject.layer = currentObjectData.checkFlag(8388608) ? 0 : 12;
		gameObject.transform.parent = parent.transform;
		if (gameObject != parent)
		{
			float z = parent.transform.eulerAngles.z;
			Vector3 eulerAngles = gameObject.transform.eulerAngles;
			eulerAngles.z = z;
			gameObject.transform.eulerAngles = eulerAngles;
			float x = sphere.origin.x;
			Vector3 localPosition = gameObject.transform.localPosition;
			localPosition.x = x;
			gameObject.transform.localPosition = localPosition;
			float y = sphere.origin.y;
			Vector3 localPosition2 = gameObject.transform.localPosition;
			localPosition2.y = y;
			gameObject.transform.localPosition = localPosition2;
			float z2 = 0f;
			Vector3 localPosition3 = gameObject.transform.localPosition;
			localPosition3.z = z2;
			gameObject.transform.localPosition = localPosition3;
		}
		else
		{
			float x2 = sphere.origin.x;
			Vector3 center = sphereCollider.center;
			center.x = x2;
			sphereCollider.center = center;
			float y2 = sphere.origin.y;
			Vector3 center2 = sphereCollider.center;
			center2.y = y2;
			sphereCollider.center = center2;
		}
	}

	public virtual void createCapsuleCollision(GameObject parent, ObjectPartSphereCollisionData sphere, bool dynamic)
	{
		GameObject gameObject = null;
		if (!hasCollider(parent))
		{
			gameObject = parent;
		}
		else
		{
			CustomProfiler.begin("LevelObjectFactory::Instantiate(objectColliderPrefab)");
			gameObject = (GameObject)UnityEngine.Object.Instantiate(prefabs.objectColliderPrefab, Vector3.zero, Quaternion.identity);
			CustomProfiler.end("LevelObjectFactory::Instantiate(objectColliderPrefab)");
		}
		CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
		capsuleCollider.radius = sphere.radius;
		float num = 0f;
		num = dynamic ? DYNAMIC_OBJECT_DEPTH : STATIC_OBJECT_DEPTH;
		num -= capsuleCollider.radius * 2f;
		if (!(num >= 0f))
		{
			num = 0f;
		}
		capsuleCollider.height = num;
		capsuleCollider.material = prefabs.physicMaterials[0];
		gameObject.layer = currentObjectData.checkFlag(8388608) ? 0 : 12;
		gameObject.transform.parent = parent.transform;
		if (gameObject != parent)
		{
			float z = parent.transform.eulerAngles.z;
			Vector3 eulerAngles = gameObject.transform.eulerAngles;
			eulerAngles.z = z;
			gameObject.transform.eulerAngles = eulerAngles;
			float x = sphere.origin.x;
			Vector3 localPosition = gameObject.transform.localPosition;
			localPosition.x = x;
			gameObject.transform.localPosition = localPosition;
			float y = sphere.origin.y;
			Vector3 localPosition2 = gameObject.transform.localPosition;
			localPosition2.y = y;
			gameObject.transform.localPosition = localPosition2;
			float z2 = 0f;
			Vector3 localPosition3 = gameObject.transform.localPosition;
			localPosition3.z = z2;
			gameObject.transform.localPosition = localPosition3;
		}
		else
		{
			float x2 = sphere.origin.x;
			Vector3 center = capsuleCollider.center;
			center.x = x2;
			capsuleCollider.center = center;
			float y2 = sphere.origin.y;
			Vector3 center2 = capsuleCollider.center;
			center2.y = y2;
			capsuleCollider.center = center2;
		}
	}

	public virtual GameObject createSplineSection(GameObject parent)
	{
		CustomProfiler.begin("LevelObjectFactory::Instantiate(splineSectionPrefab)");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(prefabs.splineSectionPrefab, Vector3.zero, Quaternion.identity);
		CustomProfiler.end("LevelObjectFactory::Instantiate(splineSectionPrefab)");
		gameObject.transform.parent = parent.transform;
		gameObject.transform.localPosition = Vector3.zero;
		return gameObject;
	}

	public virtual LevelSpline createSpline(LevelSplineData data, GameObject parent)
	{
		data.height = 1.55859f;
		CustomProfiler.begin("LevelObjectFactory::Instantiate(splinePrefab)");
		LevelSpline levelSpline = ((LevelSpline)UnityEngine.Object.Instantiate(prefabs.splinePrefab, data.position, Quaternion.identity)) as LevelSpline;
		CustomProfiler.end("LevelObjectFactory::Instantiate(splinePrefab)");
		levelSpline.transform.parent = parent.transform;
		float rotation = data.rotation;
		Vector3 localEulerAngles = levelSpline.transform.localEulerAngles;
		localEulerAngles.z = rotation;
		levelSpline.transform.localEulerAngles = localEulerAngles;
		levelSpline.transform.localScale = Vector3.one;
		levelSpline.create(data.spline, data.height);
		if (CommonData.EDITOR_ACTIVE)
		{
			levelSpline.spline = data.spline;
		}
		levelSpline.setTexture(data.texture);
		applyLayerColorTo(levelSpline.gameObject);
		return levelSpline;
	}

	private bool hasCollider(GameObject entity)
	{
		return entity.GetComponent<BoxCollider>() != null || entity.GetComponent<SphereCollider>() != null;
	}

	private void fixOptimizedScales(LevelObject @object)
	{
		float num = 1f / @object.transform.localScale.x;
		float num2 = 1f / @object.transform.localScale.y;
		Transform transform = @object.gameObject.transform;
		foreach (Transform transform2 in transform)
		{
			float x = num;
			Vector3 localScale = transform2.localScale;
			localScale.x = x;
			transform2.localScale = localScale;
			float y = num2;
			Vector3 localScale2 = transform2.localScale;
			localScale2.y = y;
			transform2.localScale = localScale2;
			float z = 1f;
			Vector3 localScale3 = transform2.localScale;
			localScale3.z = z;
			transform2.localScale = localScale3;
		}
		Component[] components = @object.GetComponents(typeof(Collider));
		if (components == null)
		{
			return;
		}
		int i = 0;
		Component[] array = components;
		for (int length = array.Length; i < length; i++)
		{
			if (array[i] is BoxCollider)
			{
				BoxCollider boxCollider = array[i] as BoxCollider;
				float x2 = boxCollider.size.x * num;
				Vector3 size = boxCollider.size;
				size.x = x2;
				boxCollider.size = size;
				float y2 = boxCollider.size.y * num2;
				Vector3 size2 = boxCollider.size;
				size2.y = y2;
				boxCollider.size = size2;
				float x3 = boxCollider.center.x * num;
				Vector3 center = boxCollider.center;
				center.x = x3;
				boxCollider.center = center;
				float y3 = boxCollider.center.y * num2;
				Vector3 center2 = boxCollider.center;
				center2.y = y3;
				boxCollider.center = center2;
			}
			else if (array[i] is SphereCollider)
			{
				SphereCollider sphereCollider = array[i] as SphereCollider;
				sphereCollider.radius *= num;
				float x4 = sphereCollider.center.x * num;
				Vector3 center3 = sphereCollider.center;
				center3.x = x4;
				sphereCollider.center = center3;
				float y4 = sphereCollider.center.y * num2;
				Vector3 center4 = sphereCollider.center;
				center4.y = y4;
				sphereCollider.center = center4;
			}
			else if (array[i] is CapsuleCollider)
			{
				CapsuleCollider capsuleCollider = array[i] as CapsuleCollider;
				capsuleCollider.radius *= num;
				float x5 = capsuleCollider.center.x * num;
				Vector3 center5 = capsuleCollider.center;
				center5.x = x5;
				capsuleCollider.center = center5;
				float y5 = capsuleCollider.center.y * num2;
				Vector3 center6 = capsuleCollider.center;
				center6.y = y5;
				capsuleCollider.center = center6;
			}
		}
	}
}
