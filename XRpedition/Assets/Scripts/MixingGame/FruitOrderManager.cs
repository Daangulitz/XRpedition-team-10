using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FruitOrderManager : MonoBehaviour
{
    public TextMeshProUGUI orderText;
    public string[] fruitNames = { "Apple", "Banana", "Blueberries", "Druif", "Mandarijn", "Pruim" };
    [SerializeField] private int NumberOfFruits;
    public List<string> CurrentOrder = new List<string>();

    private void Start()
    {
        NewOrder();
    }
    
    private void UpdateGUI()
    {
        string text = "Order: \n";
    
        foreach (string fruit in CurrentOrder) 
        { 
            text += fruit + "\n";
        }
            
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
    
}