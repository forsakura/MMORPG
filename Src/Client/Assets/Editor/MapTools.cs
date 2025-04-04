using Assets.Scripts.GameObject;
using Common.Data;
using GameServer.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MapTools : MonoBehaviour {
    [MenuItem("Map Tools/Export Teleporters")]
    public static void TeleporterDataInit()
    {
        DataManager.Instance.Load();
        Scene current = EditorSceneManager.GetActiveScene();
        string currentScene = current.name;
        if(current.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "请先保存当前场景", "确定");
            return;
        }

        //List<TeleporterObject> allTeleporters = new List<TeleporterObject>();

        foreach (var item in DataManager.Instance.Maps)
        {
            string sceneFile = "Assets/Levels/" + item.Value.Resource + ".unity";
            if (!System.IO.File.Exists(sceneFile))
            {
                Debug.LogWarningFormat("Scene {0} not existed", sceneFile);
                continue;
            }
            EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);

            TeleporterObject[] teleporters = GameObject.FindObjectsOfType<TeleporterObject>();
            foreach (var teleporter in teleporters)
            {
                if(!DataManager.Instance.Teleporters.ContainsKey(teleporter.ID))
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图：{0} 中配置的 Teleporter:[{1}] 不存在", item.Value.Name, teleporter.ID), "确定");
                    return;
                }

                TeleporterDefine def = DataManager.Instance.Teleporters[teleporter.ID];
                if(def.MapID != item.Value.ID)
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图： {0} 中配置的 Teleporter:[{1}] 不存在", item.Value.Name, def.ID), "确定");
                    return;
                }
                def.Position = GameObjectTool.WorldToLogicN(teleporter.transform.position);
                def.Direction = GameObjectTool.WorldToLogicN(teleporter.transform.forward);
            }
        }
        DataManager.Instance.SaveTeleporters();
        EditorSceneManager.OpenScene("Assets/Levels/" + currentScene + ".unity");
        EditorUtility.DisplayDialog("提示", "传送点导出完成", "确定");
    }

    [MenuItem("Map Tools/Export SpawnPoints")]
    public static void SpawnPointDataInit()
    {
        DataManager.Instance.Load();
        Scene current = EditorSceneManager.GetActiveScene();
        string currentScene = current.name;
        if(current.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "请先保存场景", "确定");
            return;
        }

        if(DataManager.Instance.SpawnPoints == null)
            DataManager.Instance.SpawnPoints = new Dictionary<int, Dictionary<int, SpawnPointDefine>>();
        foreach (var item in DataManager.Instance.Maps)
        {
            string sceneFile = "Assets/Levels/" + item.Value.Resource + ".unity";
            if(!System.IO.File.Exists(sceneFile))
            {
                Debug.LogWarningFormat("Scene {0} not existed", sceneFile);
                continue;
            }
            EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);

            SpawnPoint[] spawnPoints = GameObject.FindObjectsOfType<SpawnPoint>();
            if (!DataManager.Instance.SpawnPoints.ContainsKey(item.Value.ID))
            {
                DataManager.Instance.SpawnPoints[item.Value.ID] = new Dictionary<int, SpawnPointDefine>();
            }
            foreach (SpawnPoint spawnPoint in spawnPoints)
            {
                if (!DataManager.Instance.SpawnPoints[item.Value.ID].ContainsKey(spawnPoint.ID))
                {
                    DataManager.Instance.SpawnPoints[item.Value.ID][spawnPoint.ID] = new SpawnPointDefine();
                }
                SpawnPointDefine spawnPointDefine = DataManager.Instance.SpawnPoints[item.Value.ID][spawnPoint.ID];
                spawnPointDefine.ID = spawnPoint.ID;
                spawnPointDefine.MapID = item.Value.ID;
                spawnPointDefine.Position = GameObjectTool.WorldToLogicN(spawnPoint.transform.position);
                spawnPointDefine.Direction = GameObjectTool.WorldToLogicN(spawnPoint.transform.forward);
            }
        }
        DataManager.Instance.SaveSpawnPoints();
        EditorSceneManager.OpenScene("Assets/Levels/" + currentScene + ".unity", OpenSceneMode.Single);
        EditorUtility.DisplayDialog("提示", "刷怪点导出完成", "确定");
    }

    [MenuItem("Map Tools/GenerateNavData")]
    public static void GenerateNavData()
    {
        if(GameObject.Find("Root"))
        {
            GameObject.DestroyImmediate(GameObject.Find("Root"));
        }
        Material red = new Material(Shader.Find("Particles/Alpha Blended"));
        red.color = Color.red;
        red.SetColor("_TintColor", Color.red);
        red.enableInstancing = true;
        GameObject go = GameObject.Find("MinimapBoundingBox");
        if (go != null)
        {
            GameObject root = new GameObject("Root");
            BoxCollider bound = go.GetComponent<BoxCollider>();
            float step = 1f;
            for (float x = bound.bounds.min.x; x < bound.bounds.max.x; x += step) 
            {
                for(float z = bound.bounds.min.z; z < bound.bounds.max.z; z += step)
                {
                    for (float y = bound.bounds.min.y; y < bound.bounds.max.y + 5f; y += step)
                    {
                        var pos = new Vector3(x, y, z);
                        NavMeshHit hit;
                        if(NavMesh.SamplePosition(pos, out hit, 0.5f, NavMesh.AllAreas))
                        {
                            if(hit.hit)
                            {
                                var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                box.name = "Hit" + hit.mask;
                                box.GetComponent<MeshRenderer>().sharedMaterial = red;
                                box.transform.SetParent(root.transform, true);
                                box.transform.position = pos;
                                box.transform.localScale = Vector3.one * 0.9f;
                            }
                        }
                    }
                }
            }
        }
    }
}
