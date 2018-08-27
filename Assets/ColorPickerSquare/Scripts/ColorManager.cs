using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour {


    public static ColorManager instance = null;
    public Color color = Color.blue;
    public string cloudLabel = "";

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        //DontDestroyOnLoad(gameObject);

        //cloudLabel = Instantiate(Resources.Load("CloudLabel", typeof(GameObject)), new Vector3(100.0f, 100.0f, 100.0f), Quaternion.identity) as GameObject;
    }

    public static ColorManager Instance
    {
        get
        {
            return instance;
        }
    }
    
	
	// Update is called once per frame
	void OnColorChange (HSBColor color) {
        this.color = color.ToColor();
	}

   

}
