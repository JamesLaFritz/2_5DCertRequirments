using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IntReference m_coins;

    private void Start()
    {
        m_coins.Value = 0;
    }
}
