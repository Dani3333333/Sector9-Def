using System.Collections.Generic;
using UnityEngine;

public class PuzzleSecuenciaManager : MonoBehaviour
{
    public List<VolanteInteractivo> valvulas;
    public ParticleSystem fugaParticulas;
    public ActivarFugaAgua activarFugaAgua;

    private bool puzzleResuelto = false;

    void Start()
    {
        // Asignar referencia inversa a cada válvula
        foreach (var valvula in valvulas)
        {
            valvula.manager = this;
        }
    }

    void Update()
    {
        if (!puzzleResuelto && TodasLasValvulasGiradas())
        {
            ResolverPuzzle();
        }
    }

    bool TodasLasValvulasGiradas()
    {
        foreach (var valvula in valvulas)
        {
            if (!valvula.fueGirado)
                return false;
        }
        return true;
    }

    void ResolverPuzzle()
    {
        puzzleResuelto = true;

        if (fugaParticulas != null)
            fugaParticulas.Stop();

        if (activarFugaAgua != null)
            activarFugaAgua.DetenerFuga();

        Debug.Log("Fuga reparada: todas las válvulas han sido giradas.");
    }
}
