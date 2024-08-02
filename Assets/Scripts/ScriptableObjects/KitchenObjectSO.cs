using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// [CreateAssetMenu(fileName = "KitchenObjectSO", menuName = "KitchenChaosLearning/KitchenObjectSO", order = 0)]
[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{
  public Transform prefab;
  public Sprite sprite;
  public string objectName;


}