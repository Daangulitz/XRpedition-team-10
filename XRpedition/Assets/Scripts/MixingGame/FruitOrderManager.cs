// FruitOrderManager.cs
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class FruitOrderManager : MonoBehaviour
{
    public TextMeshProUGUI orderText;
    public string[] fruitNames = { "Apple", "Banana", "Blauwe bes", "Druif", "Mandarijn", "Pruim" };
    [SerializeField] private int NumberOfFruits;
    public List<string> CurrentOrder = new List<string>();
    private HashSet<string> validFruitSet;

    private void Start()
    {
        validFruitSet = new HashSet<string>(fruitNames);
        NewOrder();
    }

    private void UpdateGUI()
    {
        string text = "Order: \n";
        foreach (string fruit in CurrentOrder)
            text += fruit + "\n";
        orderText.text = text;
    }

    public void NewOrder()
    {
        CurrentOrder.Clear();
        for (int i = 0; i < NumberOfFruits; i++)
        {
            int index = Random.Range(0, fruitNames.Length);
            CurrentOrder.Add(fruitNames[index]);
        }
        UpdateGUI();
    }

    public bool IsValidFruit(string fruitTag)
    {
        return validFruitSet.Contains(fruitTag);
    }

    public bool IsCorrectCombination(List<string> fruits)
    {
        var validFruits = fruits.Where(f => IsValidFruit(f)).ToList();
        if (validFruits.Count != CurrentOrder.Count)
            return false;

        var orderCounts = CurrentOrder.GroupBy(f => f).ToDictionary(g => g.Key, g => g.Count());
        var fruitCounts = validFruits.GroupBy(f => f).ToDictionary(g => g.Key, g => g.Count());

        if (orderCounts.Count != fruitCounts.Count)
            return false;

        foreach (var kv in orderCounts)
        {
            int cnt;
            if (!fruitCounts.TryGetValue(kv.Key, out cnt)) return false;
            if (cnt != kv.Value) return false;
        }
        return true;
    }
}