using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChanger : MonoBehaviour {



    //This object should be called 'Fader' and placed over the camera
    GameObject m_Fader;

    //This ensures that we don't mash to change spheres
    bool changing = false;
    [SerializeField]
    GameObject activeCanvas;
    [SerializeField]
    Renderer Apartment360;

    [SerializeField]
    Material[] rooms;

    [SerializeField]
    GameObject[] roomsCanvas;

    public float time = 7f;
    public int roomIndex = 0;
    public int roomMaxSize;


    void Awake()
    {

        //Find the fader object
        m_Fader = GameObject.Find("Fader");

        //Check if we found something
        if (m_Fader == null)
            Debug.LogWarning("No Fader object found on camera.");

        roomMaxSize = rooms.Length;

    }

    private void Update()
    {
        time -= Time.deltaTime;

        if(time < 0.0f)
        {
            // time = 7f;

           
                roomIndex += 1;

                if (roomIndex < roomMaxSize)
                {

                    activeCanvas.SetActive(false);
                    activeCanvas = roomsCanvas[roomIndex];
                    activeCanvas.SetActive(true);
                    ChangeRoom(rooms[roomIndex]);
                }
                else
                {
                    roomIndex = 0;
                    activeCanvas.SetActive(false);
                    activeCanvas = roomsCanvas[roomIndex];
                    activeCanvas.SetActive(true);
                    ChangeRoom(rooms[roomIndex]);
                }


            time = 100f;
        }
        
    }


    public void ChangeRoom(Material room)
    {
        time = 100f;

        //Start the fading process
        StartCoroutine(FadeCamera(room));

    }

    public void ChangeRoomCanvas(GameObject roomCanvas)
    {

        activeCanvas = roomCanvas;

    }

    public void SetRoomIndex(int index)
    {
        roomIndex = index;
    }

   


    IEnumerator FadeCamera(Material room)
    {

        //Ensure we have a fader object
        if (m_Fader != null)
        {
            
            yield return StartCoroutine(FadeIn(0.75f, m_Fader.GetComponent<Renderer>().material));

            //StartCoroutine(FadeIn(0.75f, m_Fader.GetComponent<Renderer>().material));
            //yield return new WaitForSeconds(0.75f);


            Apartment360.material = room;
            

            
            yield return StartCoroutine(FadeOut(0.75f, m_Fader.GetComponent<Renderer>().material));
            activeCanvas.SetActive(true);
            
            //StartCoroutine(FadeOut(0.75f, m_Fader.GetComponent<Renderer>().material));
            //yield return new WaitForSeconds(0.75f);
        }
        else
        {
            activeCanvas.SetActive(true);
            Apartment360.material = room;
        }


        time = 7f;


    }


    IEnumerator FadeOut(float time, Material mat)
    {
        //While we are still visible, remove some of the alpha colour
        while (mat.color.a > 0.25f)
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a - (Time.deltaTime / time));
            yield return null;
        }
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0);
    }


    IEnumerator FadeIn(float time, Material mat)
    {
        //While we aren't fully visible, add some of the alpha colour
        while (mat.color.a < .75f)
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a + (Time.deltaTime / time));
            yield return null;
        }
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1);
    }


}
