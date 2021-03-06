﻿using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    public float speed;
    public float minTimeBetweenColorSwitch;
    public float maxTimeBetweenColorSwitch;

    // GAME OBJECTS
    private Player player;

    // COMPONENTS
    private MeshRenderer rendererMonsterPart1;
    private MeshRenderer rendererMonsterPart2;
    private NavMeshAgent agent;

    // VARIABLES
    private float switchTimer;
    private float switchTime;
    private Color targetColor;
    private Color startingColor;
    private float fadingTimer;


	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        rendererMonsterPart1 = GetComponentsInChildren<MeshRenderer>()[0];
        rendererMonsterPart2 = GetComponentsInChildren<MeshRenderer>()[1];

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        switchTimer = 0f;
        switchTime = Random.Range(minTimeBetweenColorSwitch, maxTimeBetweenColorSwitch);
        fadingTimer = 99f;
        startingColor = GameManager.instance.red;
        targetColor = GameManager.instance.red;
        setColor(startingColor);
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Change color periodically
        switchTimer += Time.deltaTime;
        if (switchTimer >= switchTime)
        {
            targetColor = GameManager.instance.red;
            int value = Mathf.FloorToInt(Random.Range(0f, 3f));
            if (value == 1)
            {
                targetColor = GameManager.instance.green;
            } else if (value == 2)
            {
                targetColor = GameManager.instance.blue;
            }
            fadingTimer = 0f;
            startingColor = getColor();
            switchTimer = 0f;
            switchTime = Random.Range(minTimeBetweenColorSwitch, maxTimeBetweenColorSwitch);
        }

        // Cross fade color changes
        if (fadingTimer <= 1f)
        {
            setColor(Color.Lerp(startingColor, targetColor, fadingTimer / 1f));
            fadingTimer += Time.deltaTime;
        }
	}

    void setColor (Color color)
    {
        rendererMonsterPart1.material.SetColor("_Color", color);
        rendererMonsterPart1.material.SetColor("_Color2", color);
        rendererMonsterPart2.material.SetColor("_Color", color);
        rendererMonsterPart2.material.SetColor("_Color2", color);
    }

    Color getColor()
    {
        return rendererMonsterPart1.material.color;
    }

    void OnTriggerEnter (Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            agent.Stop();
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
