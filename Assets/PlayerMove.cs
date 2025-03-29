using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public bool atacando;
    public bool patdada;
    public Animator ani;

    private float Gravedad;
    private float Ypos;
    private float Ypos_Piso;
    private bool inground;
    private bool saltando;
    private int Fases;
    private float AlturaSalto;
    private float PotenciaSalto;
    private float Xpos;
    private float Fallen;

    public SpriteRenderer spr;
    private float delay;
    private int sky_;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }

    public void Mover()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow) && !atacando && !saltando && inground)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            ani.SetBool("run", true);
        }
        else
        {
            ani.SetBool("run", false);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && !atacando && !saltando && inground)
        {
            transform.Translate(Vector3.up * -speed * Time.deltaTime);
            ani.SetBool("run", true);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && !atacando)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            ani.SetBool("run", true);
        }


        if (Input.GetKeyDown(KeyCode.LeftArrow) && !atacando)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            ani.SetBool("run", true);
        }
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.C) && !saltando && inground)
        {
            Ypos_Piso = transform.position.y;
            saltando = true;
            inground = false;
        }

        if (saltando)
        {
            switch (Fases)
            {
                case 0:
                    Gravedad = AlturaSalto;

                    break;

                case 1:

                    if (Gravedad > 0)
                    {
                        Gravedad -= PotenciaSalto * Time.deltaTime;
                    }
                    else
                    {
                        Fases = 2;
                    }

                    break;
            }

        }

    }

    void SetTransformY(float n)
    {
        transform.position = new Vector3(transform.position.x, n, transform.position.z);
    }

    public void Detector_Piso()
    {
        if (!saltando && !atacando)
        {
            sky_ = 0;

            if (Ypos == Ypos_Piso)
            {
                inground = true;
            }
            ani.SetBool("jump", false);
        }
        else
        {
            ani.SetBool("jump", true);
        }

        if (inground)
        {
            Gravedad = 0;
            Fases = 0;
        }
        else
        {
            switch (Fases)
            {
                case 2:
                    Gravedad = 0;
                    Fases = 3;
                    break;

                case 3:
                    if (Ypos >= Ypos_Piso)
                    {
                        if (Gravedad > -10)
                        {
                            Gravedad -= AlturaSalto / Fallen * Time.deltaTime;
                        }
                    }
                    else
                    {
                        saltando = true;
                        inground = true;
                        SetTransformY(Ypos_Piso);
                        Fases = 0;
                    }

                    break;
            }
        }

        if (!inground && !patdada)
        {
            if (transform.position.y > Ypos)
            {
                ani.SetFloat("Gravedad", 1);
            }
            if (transform.position.y < Ypos)
            {
                ani.SetFloat("gravedad", 0);


                switch (sky_)
                {
                    case 0:
                        ani.Play("Base Layer.Jump", 0, 0);
                        sky_++;
                        break;
                }
            }
        }

        Ypos = transform.position.y;

    }

    public void Finish_Ani()
    {
        atacando = false;
        patdada = false;
    }

    public void Golpe()
    {
            if(Input.GetKeyDown(KeyCode.X))
            {
                delay= 1;
                if (!saltando)
                {
                    atacando= true;
                    ani.SetTrigger("hit");
                }
            }
            else
            {
                if (!patdada)
                {
                    patdada = true;
                    ani.SetTrigger("kick");
                }
            
            }

        if (delay > 0)
        {
            spr.sortingOrder = 1;
            delay -= 2 * Time.deltaTime;

        }
        else
        {
            spr.sortingOrder = 0;
            delay = 0;
        }

    }




    // Update is called once per frame
    void Update()
        {
            Detector_Piso();
            Jump();
            Golpe();
        }

    private void FixedUpdate()
    {
        Mover();
        transform.Translate(Vector3.up * Gravedad * Time.deltaTime);
    }



}
