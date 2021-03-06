/*********************************
 * By: Andrew kitzan
 * last edit: 3/3/2022
 * desc: controlls all the stuff when the player goes in the wall
 * ******************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class WallTransition : MonoBehaviour
{
    //varables
    private KeyCode interact = KeyCode.E;
    public GameObject wall;
    public GameObject layer1;
    private GameObject grid;
    private GameObject enemy;
    private bool inWall = false;
    private Tilemap tileSet;
    private Color defualtGridColor;
    private Color defualtEnemyColor;
    private Color transparentGridColor;
    private Color transparentEnemyColor;
    private float timer = 0;
    public float countdown = 1;
    private bool haveSet = false;
    private GameObject exit;
    private Color defualtExitColor;
    private Color transparentExitColor;
    private GameObject decoration;
    private Color defualtDecorationColor;
    private Color transparentDecorationColor;
    private Color defualtEnemyScanColor;
    private Color transparentEnemyScanColor;


    // Start is called before the first frame update
    void Start()
    {
        wall.SetActive(false);
    }


    private void Update()
    {
        timer += Time.deltaTime;

        if (haveSet == false)
        {
            for (int i = 0; i < layer1.transform.childCount; ++i)
            {

                if (layer1.transform.GetChild(i).gameObject.CompareTag("Grid"))
                {
                    grid = layer1.gameObject.transform.GetChild(i).gameObject;

                    for (int j = 0; j < grid.transform.childCount; ++j)
                    {
                        //sets the loaction of grid, and its colors
                        defualtGridColor = grid.transform.GetChild(j).GetComponent<Tilemap>().color;
                        transparentGridColor = new Color(defualtGridColor.r, defualtGridColor.g, defualtGridColor.b, 0.3f);
                    }
                }

                //set enemy color
                if (layer1.transform.GetChild(i).gameObject.CompareTag("Enemy"))
                {
                    //sets the loaction of enemy and its colors
                    enemy = layer1.gameObject.transform.GetChild(i).gameObject;
                    defualtEnemyColor = enemy.GetComponent<SpriteRenderer>().color;
                    transparentEnemyColor = new Color(defualtEnemyColor.r, defualtEnemyColor.g, defualtEnemyColor.b, 0.3f);
                    for (int j = 0; j < enemy.transform.childCount; j++)
                    {
                        defualtEnemyScanColor = enemy.GetComponent<SpriteRenderer>().color;
                        transparentEnemyScanColor = new Color(defualtEnemyScanColor.r, defualtEnemyScanColor.g, defualtEnemyScanColor.b, 0);
                    }

                }

                //set exit color
                if (layer1.transform.GetChild(i).gameObject.CompareTag("Exit"))
                {
                    exit = layer1.gameObject.transform.GetChild(i).gameObject;
                    defualtExitColor = exit.GetComponent<SpriteRenderer>().color;
                    transparentExitColor = new Color(defualtExitColor.r, defualtExitColor.g, defualtExitColor.b, 0.3f);
                }
                //set decoration color
                if (layer1.transform.GetChild(i).gameObject.CompareTag("Decoration"))
                {
                    decoration = layer1.gameObject.transform.GetChild(i).gameObject;
                    for (int j = 0; j < decoration.transform.childCount; ++j)
                    {
                        defualtDecorationColor = decoration.transform.GetChild(j).GetComponent<Tilemap>().color;
                        transparentDecorationColor = new Color(defualtDecorationColor.r, defualtDecorationColor.g, defualtDecorationColor.b, 0.3f);
                    }

                }

            }
            haveSet = true;
        }
    }

    //check if the player wants to go in the wall
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetKey(interact))
        {
            if (timer > countdown)
            {
                //keeps track of if player is in wall
                wall.SetActive(!wall.activeInHierarchy);
                GameManager.inWall = !GameManager.inWall;

                //loops through all children inside parent(layer1)
                for (int i = 0; i < layer1.transform.childCount; ++i)
                {
                    //checks if child is a Grid
                    if (layer1.transform.GetChild(i).gameObject.CompareTag("Grid"))
                    {

                        //if it is a Grid then change the Alpha value on each tilemap within grid
                        for (int j = 0; j < layer1.transform.GetChild(i).transform.childCount; ++j)
                        {
                            grid = layer1.gameObject.transform.GetChild(i).gameObject;

                            if (!GameManager.inWall)
                            {
                                grid.transform.GetChild(j).GetComponent<Tilemap>().color = defualtGridColor;
                                grid.transform.GetChild(j).GetComponent<TilemapCollider2D>().enabled = true;
                            }
                            else
                            {
                                grid.transform.GetChild(j).GetComponent<Tilemap>().color = transparentGridColor;
                                grid.transform.GetChild(j).GetComponent<TilemapCollider2D>().enabled = false;
                            }
                        }
                    }

                    //if child of parent(layer1) is an enemy
                    if (layer1.transform.GetChild(i).gameObject.CompareTag("Enemy"))
                    {

                        //sets the loaction of enemy and its colors
                        enemy = layer1.gameObject.transform.GetChild(i).gameObject;

                        if (!GameManager.inWall)
                        {
                            enemy.GetComponent<SpriteRenderer>().color = defualtEnemyColor;
                            enemy.GetComponent<BoxCollider2D>().enabled = true;
                            enemy.GetComponent<PolygonCollider2D>().enabled = true;
                            enemy.transform.GetChild(1).gameObject.SetActive(true);
                        }
                        else
                        {
                            enemy.GetComponent<SpriteRenderer>().color = transparentEnemyColor;
                            enemy.GetComponent<BoxCollider2D>().enabled = false;
                            enemy.GetComponent<PolygonCollider2D>().enabled = false;
                            enemy.transform.GetChild(1).gameObject.SetActive(false);

                        }
                    }

                    //if it is the exit
                    if (layer1.transform.GetChild(i).gameObject.CompareTag("Exit"))
                    {
                        exit = layer1.gameObject.transform.GetChild(i).gameObject;
                        if (!GameManager.inWall)
                        {
                            exit.GetComponent<SpriteRenderer>().color = defualtExitColor;
                            exit.GetComponent<BoxCollider2D>().enabled = true;
                        }
                        else
                        {
                            exit.GetComponent<SpriteRenderer>().color = transparentExitColor;
                            exit.GetComponent<BoxCollider2D>().enabled = false;
                        }
                    }

                    //Decorations
                    if (layer1.transform.GetChild(i).gameObject.CompareTag("Decoration"))
                    {
                        for (int j = 0; j < layer1.transform.GetChild(i).transform.childCount; ++j)
                        {
                            decoration = layer1.gameObject.transform.GetChild(i).gameObject;
                            if (!GameManager.inWall)
                            {
                                decoration.transform.GetChild(j).GetComponent<Tilemap>().color = defualtDecorationColor;
                            }
                            else
                            {
                                decoration.transform.GetChild(j).GetComponent<Tilemap>().color = transparentDecorationColor;
                            }
                        }
                    }

                    //collectables
                    Collectible[] collectables = FindObjectsOfType<Collectible>();
                    for (int j = 0; j < collectables.Length; j++)
                    {
                        if (!GameManager.inWall)
                        {
                            Color CC = collectables[j].gameObject.GetComponent<SpriteRenderer>().color;
                            CC = new Color(CC.r, CC.g, CC.b, 1);
                            collectables[j].gameObject.GetComponent<SpriteRenderer>().color = CC;
                            collectables[j].gameObject.GetComponent<BoxCollider2D>().enabled = true;
                        }
                        else
                        {
                            Color CC = collectables[j].gameObject.GetComponent<SpriteRenderer>().color;
                            CC = new Color(CC.r, CC.g, CC.b, 0.3f);
                            collectables[j].gameObject.GetComponent<SpriteRenderer>().color = CC;
                            collectables[j].gameObject.GetComponent<BoxCollider2D>().enabled = false;
                        }
                    }
                }
                timer = 0;
            }
        }
    }
}