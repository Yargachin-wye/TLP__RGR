using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class InterpreterDKA
{
    private List<char> alphabet = new List<char> { 'a', 'b', 'c' }; // Замените это своим алфавитом
    private string finalSubchain = "abc";
    private char countSymbol = 'c';
    private int count = 3;

    public Dictionary<string, List<(char, string)>> rules = new Dictionary<string, List<(char, string)>>();
    private string finalState = "q0";
    private string startState = "q0";
    TextMeshProUGUI text;
    public InterpreterDKA(List<char> alphabet, string finalSubchain, char countSymbol, int count, TextMeshProUGUI text)
    {
        this.alphabet = alphabet;
        this.finalSubchain = finalSubchain;
        this.countSymbol = countSymbol;
        this.count = count;
        this.text = text;
        GenerateNewDKA();
    }
    private void GenerateNewDKA()
    {
        DKAGenerator dkaGenerator = new DKAGenerator(alphabet, finalSubchain, countSymbol, count);
        var result = dkaGenerator.GenerateDKA();

        Debug.Log("\nTransitions:");
        foreach (var transition in result.Item1)
        {
            Console.Write($"{transition.Key}: ");
            List<(char, string)> transitions = new List<(char, string)>();
            foreach (var pair in transition.Value)
            {
                Console.Write($"({pair.Key}, {pair.Value}) ");
                transitions.Add((pair.Key, pair.Value));
            }
            Debug.Log("");
            rules.Add(transition.Key, transitions);
        }
        Debug.Log($"\nStart State: {result.Item3}\n");
        Debug.Log($"\nFinal State: {result.Item2}\n");
        startState = result.Item3;
        finalState = result.Item2;
    }
    public void CheckDKA(string str)
    {
        DKA dka = new DKA(rules, str, finalState, startState, text);

        dka.СheckWord();
    }
}
