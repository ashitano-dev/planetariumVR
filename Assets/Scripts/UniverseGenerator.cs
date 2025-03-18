using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UniverseGenerator : MonoBehaviour
{
    TextAsset constellationCSV;
    TextAsset constellationLineCSV;
    TextAsset constellationsStarCSV;
    Constellation[] constellations;
    ConstellationLine[] constellationLines;
    ConstellationStar[] constellationsStars;
    public GameObject ConstellationPrefab;
    public GameObject StarPrefab;

    public GameObject LinePrefab;

    void Start()
    {
        //generate constellation
        constellationCSV = Resources.Load<TextAsset>("constellation_name_utf8");
        constellations = CSVSerializer.Deserialize<Constellation>(constellationCSV.text);
        foreach (Constellation c in constellations)
        {
            GameObject constellation = Instantiate(ConstellationPrefab, Vector3.zero, Quaternion.identity);
            constellation.GetComponent<ConstellationManager>().id = c.id;
            constellation.GetComponent<ConstellationManager>().name_en = c.name_en;
            constellation.GetComponent<ConstellationManager>().name_jp = c.name_jp;
            constellation.name = c.abbreviation;
        }

        //generate star and line
        constellationLineCSV = Resources.Load<TextAsset>("hip_constellation_line");
        constellationLines = CSVSerializer.Deserialize<ConstellationLine>(constellationLineCSV.text);
        constellationsStarCSV = Resources.Load<TextAsset>("hip_constellation_line_star");
        constellationsStars = CSVSerializer.Deserialize<ConstellationStar>(constellationsStarCSV.text);

        foreach (ConstellationLine cl in constellationLines)
        {
            GameObject constellation = GameObject.Find(cl.abbreviation);
            GameObject start = GameObject.Find("HIP_" + cl.start_hip_id);
            GameObject end = GameObject.Find("HIP_" + cl.end_hip_id);

            if (start == null)
            {
                ConstellationStar cs = System.Array.Find(constellationsStars, x => x.hip_id == cl.start_hip_id);
                float alpha = (cs.ra_h + cs.ra_m / 60.0f + cs.ra_s / 3600.0f) * 15.0f * Mathf.Deg2Rad;
                float delta = (cs.dec_deg + cs.dec_m / 60.0f + cs.dec_s / 3600.0f) * Mathf.Deg2Rad;
                float x = Mathf.Cos(delta) * Mathf.Cos(alpha) * 30.0f;
                float y = Mathf.Cos(delta) * Mathf.Sin(alpha) * 30.0f;
                float z = Mathf.Sin(delta) * 30.0f;
                start = Instantiate(StarPrefab, new Vector3(-x, y, z), Quaternion.identity);
                start.name = "HIP_" + cl.start_hip_id;
                start.GetComponent<StarManager>().hip_id = cl.start_hip_id;

                start.transform.parent = constellation.transform;
                constellation.GetComponent<ConstellationManager>().addStar(start);
            }

            if (end == null)
            {
                ConstellationStar cs = System.Array.Find(constellationsStars, x => x.hip_id == cl.end_hip_id);
                float alpha = (cs.ra_h + cs.ra_m / 60.0f + cs.ra_s / 3600.0f) * 15.0f * Mathf.Deg2Rad;
                float delta = (cs.dec_deg + cs.dec_m / 60.0f + cs.dec_s / 3600.0f) * Mathf.Deg2Rad;
                float x = Mathf.Cos(delta) * Mathf.Cos(alpha) * 30.0f;
                float y = Mathf.Cos(delta) * Mathf.Sin(alpha) * 30.0f;
                float z = Mathf.Sin(delta) * 30.0f;
                end = Instantiate(StarPrefab, new Vector3(-x, y, z), Quaternion.identity);
                end.name = "HIP_" + cl.end_hip_id;
                end.GetComponent<StarManager>().hip_id = cl.end_hip_id;

                end.transform.parent = constellation.transform;
                constellation.GetComponent<ConstellationManager>().addStar(end);
            }

            GameObject line = Instantiate(LinePrefab, Vector3.zero, Quaternion.identity);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, start.transform.position);
            lineRenderer.SetPosition(1, end.transform.position);
            line.transform.parent = constellation.transform;
            line.SetActive(false);

            constellation.GetComponent<ConstellationManager>().addLine(start, end, line);
        }
    }
}
