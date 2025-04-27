using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public enum TipoDeComida { Malo, Regular, Bueno }
    public TipoDeComida tipoDeComida;
    public bool yaUsado = false;
}
