using UnityEngine;

public class PrisonerManager : MonoBehaviour
{
    [Header("Prisioneros en escena (excepto el primero que ya está activo)")]
    public GameObject[] prisoners; // Prisionero2, Prisionero3, Prisionero4

    private int maxPrisoners = 4; // Incluyendo al Prisionero1

    public void EnablePrisonerForDay(int day)
    {
        // Día 0 ya está activo el Prisionero1, no hacemos nada
        if (day <= 0 || day >= maxPrisoners)
            return;

        int index = day - 1;

        if (index >= prisoners.Length)
        {
            Debug.LogWarning("No hay suficientes prisioneros asignados en el array.");
            return;
        }

        GameObject prisoner = prisoners[index];

        if (prisoner != null && !prisoner.activeSelf)
        {
            prisoner.SetActive(true);
            prisoner.tag = "Prisionero";
        }
    }
}