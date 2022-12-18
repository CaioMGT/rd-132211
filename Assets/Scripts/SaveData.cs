using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour {
    void Start() {
        Load();
    }

    void Update() {
        if(Input.GetButtonDown("Enter")) {
            Save();

            Debug.Log("Pressionou Enter");
        }
    }

    public void Save() {
        PlayerPrefs.SetFloat("x", transform.position.x);
        PlayerPrefs.SetFloat("y", transform.position.y);
        PlayerPrefs.SetFloat("z", transform.position.z);
    }

    public void Load() {
        Vector3 Pos = new Vector3(
            PlayerPrefs.GetFloat("x"),
            PlayerPrefs.GetFloat("y"),
            PlayerPrefs.GetFloat("z")
        );

        transform.position = Pos;
    }
}
