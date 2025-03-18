using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Line
{
    public GameObject start;
    public GameObject end;
    public GameObject line;
}

public class ConstellationManager : MonoBehaviour
{
    public int id;
    public string name_en;
    public string name_jp;
    public List<GameObject> stars = new List<GameObject>();

    public List<Line> lines = new List<Line>();

    public void addStar(GameObject star)
    {
        stars.Add(star);
    }

    public void addLine(GameObject start, GameObject end, GameObject line)
    {
        Line l = new Line();
        l.start = start;
        l.end = end;
        l.line = line;
        lines.Add(l);
    }

    public void Update()
    {
        // linesの中で、startとendが両方ともis_activeがtrueのものを取得
        List<Line> activeLines = lines.Where(line => line.start.GetComponent<StarManager>().is_active && line.end.GetComponent<StarManager>().is_active).ToList();
        foreach (Line line in activeLines)
        {
            line.line.SetActive(true);
        }
    }
}
