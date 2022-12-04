using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    //private List<MovingObject> characters;
    //NPC 수만큼 담기 위한 List
    void Start()
    {

    }
    //public void PreLoadCharacter()
    //{
    //    characters = ToList();
    //}
    //public List<MovingObject> ToList()
    //{
    //    List<MovingObject> tempList = new List<MovingObject>();
    //    MovingObject[] temp = FindObjectsOfType<MovingObject>();

    //    for(int i = 0; i < temp.Length; i++)
    //    {
    //        tempList.Add(temp[i]);
    //    }
    //    return tempList;
    //}
    //public void SetThorough(string _name)
    //{
    //    for (int i = 0; i < characters.Count; i++)
    //    {
    //        if (_name == characters[i].characterName)
    //        {
    //            characters[i].boxCollider.enabled = false;
    //        }
    //    }
    //}
    //public void NotMove()
    //{
    //    thePlayer.notMove = true;
    //}
    //public void Move()
    //{
    //    thePlayer.notMove = false;
    //}
    //public void Move(string _name, string _dir)
    //{
    //    for (int i = 0; i < characters.Count; i++)
    //    {
    //        if (_name == characters[i].characterName)
    //        {
    //            characters[i].Move(_dir);
    //        }
    //    }
    //}
    //public void SetUnThorough(string _name)
    //{
    //    for (int i = 0; i < characters.Count; i++)
    //    {
    //        if (_name == characters[i].characterName)
    //        {
    //            characters[i].boxCollider.enabled = true;
    //        }
    //    }
    //}
    //public void SetTransparent(string _name)
    //{
    //    for (int i = 0; i < characters.Count; i++)
    //    {
    //        if (_name == characters[i].characterName)
    //        {
    //            characters[i].gameObject.SetActive(false);
    //        }
    //    }
    //}
    //public void SetUnTransparent(string _name)
    //{
    //    for (int i = 0; i < characters.Count; i++)
    //    {
    //        if (_name == characters[i].characterName)
    //        {
    //            characters[i].gameObject.SetActive(true);
    //        }
    //    }
    //}
    //public void Turn(string _name, string _dir)
    //{
    //    for (int i = 0; i < characters.Count; i++)
    //    {
    //        if (_name == characters[i].characterName)
    //        {
    //            characters[i].animator.SetFloat("DirX", 0f);
    //            characters[i].animator.SetFloat("DirY", 0f);
    //            switch (_dir)
    //            {
    //                case "UP":
    //                    characters[i].animator.SetFloat("DirY", 1f);
    //                    break;
    //                case "DOWN":
    //                    characters[i].animator.SetFloat("DirY", -1f);
    //                    break;
    //                case "LEFT":
    //                    characters[i].animator.SetFloat("DirX", -1f);
    //                    break;
    //                case "RIGHT":
    //                    characters[i].animator.SetFloat("DirX", 1f);
    //                    break;
    //            }
               
    //        }
    //    }
    //}
    void Update()
    {
        
    }
}
