using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePlacer : MonoBehaviour
{
    public GameObject treePrefab;
    private float _radius = 3f;
    private int _count = 200;
    AllFather _allFather;
    public int _chunkSeed;
    private System.Random randomGenerator; // Заменяем тип на System.Random

    void Start()
    {
        _allFather = GameObject.Find("AllFather").GetComponent<AllFather>();

        int count = _count; //ПОСТАВИТЬ счетчику количества начальное максимальное занчение
        for (int i = 0; count > 0 && i < 5000; i++) //цикл пока количество не закончится
        {
            Mesh mesh = GetComponent<MeshFilter>().mesh; //Получить меш с объекта
            Vector3 spawnPoint = GenerateRandomPointOnMesh(mesh, i); //Взять рандомную (Я НАДЕЮСЬ) точку на меше

            if (IsFloorClear(spawnPoint)) //Если там ниче нет
            {
                var obj = Instantiate(treePrefab, spawnPoint, treePrefab.transform.rotation); //Создать там клон
                obj.transform.SetParent(transform);
                Rotate(obj, i); //Повернуть СЛУЧАЙНО СЛУЧАЙНО СЛУЧАЙНО но оно не работает так зараза
                Scale(obj, i); //Масштабировать СЛУЧАЙНО но оно тоже не работает так
                count--; //Посчитать что это поставили
                //Debug.Log("Tree placed"); //Сказать об этом
                continue; //След итерация
            }
        }

        void Scale(GameObject obj, int pointIndex)
        {
            Random.InitState(GeneratePointSeed(pointIndex, 3)); //Применить сид к рандомайзеру
            float randomScale = 1f + Random.Range(0f, 1f); ; //Сгенерировать размер 
            obj.transform.localScale *= randomScale; //Применить размер А СУКА МОЖЕТ ЭТО ОНО?????????????????????
        }

        void Rotate(GameObject obj, int pointIndex)
        {
            Random.InitState(GeneratePointSeed(pointIndex, 2)); //Применить сид к рандомайзеру
            float randomRotation = Random.Range(0f, 360f); //Сгенерить вращение
            obj.transform.Rotate(Vector3.up, randomRotation, Space.World);
        }

        int GeneratePointSeed(int pointIndex, int type)
        {
            int subSeed = _allFather._seed * 999999 * type;
            subSeed += pointIndex * type * 999999;
            subSeed += _chunkSeed * type * 999999;
            return subSeed;
        }

        Vector3 GenerateRandomPointOnMesh(Mesh mesh, int pointIndex)
        {
            randomGenerator = new System.Random(GeneratePointSeed(pointIndex, 1));

            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;

            int triangleIndex = randomGenerator.Next(0, triangles.Length / 3);

            Vector3 vertex1 = vertices[triangles[triangleIndex * 3]];
            vertex1 = transform.TransformPoint(vertex1);
            Vector3 vertex2 = vertices[triangles[triangleIndex * 3 + 1]];
            vertex2 = transform.TransformPoint(vertex2);
            Vector3 vertex3 = vertices[triangles[triangleIndex * 3 + 2]];
            vertex3 = transform.TransformPoint(vertex3);

            float r1 = (float)randomGenerator.NextDouble();
            float r2 = (float)randomGenerator.NextDouble();

            if (r1 + r2 >= 1)
            {
                r1 = 1 - r1;
                r2 = 1 - r2;
            }

            Vector3 randomPointLocal = vertex1 + r1 * (vertex2 - vertex1) + r2 * (vertex3 - vertex1);

            Vector3 randomPointWorld = transform.position + randomPointLocal;

            return randomPointWorld;
        }

        bool IsFloorClear(Vector3 origin)
        {
            float angleStep = 60f;

            origin += Vector3.up * 1000;

            Vector3[] points = new Vector3[7];
            points[0] = origin;

            for (int i = 0; i < 6; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                var direction = new Vector3(
                   Mathf.Cos(angle), 0, Mathf.Sin(angle)
                );

                points[i + 1] = origin + direction * _radius;
            }

            for (int i = 0; i < 7; i++)
            {
                RaycastHit hit;
                if (Physics.Raycast(points[i], Vector3.down, out hit, 2000))
                {
                    if (hit.collider.gameObject != gameObject)
                    {
                        string hp = $"({hit.point.x}, {hit.point.y}, {hit.point.z})";
/*                        if (hit.collider.gameObject.name != null)
                            Debug.Log($"Tree bounced object {hit.collider.gameObject.name}. ({hp})");
                        else
                            Debug.Log($"Tree bounced NULL. ({hp})");*/

                        return false;
                    }
                }
                else
                {
                    //Debug.Log($"Tree missplaced.");
                    return false;
                }
            }

            return true;
        }
    }
}