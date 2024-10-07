using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataIO
{
    public static List<Enemy> EnemyDatas = new List<Enemy>();

    public static string[] LinesReader(string filePath)
    {
        if (File.Exists(filePath))
        {
            return File.ReadAllLines(filePath);
        }
        else
        {
            Debug.LogError("CSV 파일 가져올 수 없음 | " + filePath);
            return null;
        }
    }
    public static void CsvParseEnemy(string filePath)
    {
        string[] csvLines = LinesReader(filePath);

        if (csvLines==null)
            return;

        // 추후 리플렉션을 이용하여 매핑하거나, Enemy 필드를 사전형으로 바꾸는 등으로 자동화 개선 여지 있음.
        for (int i = 1; i < csvLines.Length; i++)
        {
            string[] fields = csvLines[i].Split(',');

            if (fields.Length == 5)
            {
                Enemy enemy = new Enemy
                {
                    Name = fields[0],
                    Grade = fields[1],
                    Speed = float.Parse(fields[2]),
                    Health = int.Parse(fields[3]),
                    Description = fields[4]
                };
                enemy.SetOthers();

                EnemyDatas.Add(enemy);
            }
            else
            {
                Debug.LogError("정해진 방식의 CSV로 입력되지 않음. 줄 번호: " + i);
            }
        }
        Debug.Log("매핑 완료");
    }

}

public struct Enemy
{
    public GameObject GameObject { get; set; }
    public Sprite Thumbnail {  get; set; }
    public string Name { get; set; }
    public string Grade { get; set; }
    public float Speed { get; set; }
    public int Health { get; set; }
    public string Description { get; set; }
    public void SetOthers()
    {
        GameObject = Resources.Load<GameObject>($"Prefabs/{Name}");
        Thumbnail = GameObject.GetComponent<SpriteRenderer>().sprite;
    }
}