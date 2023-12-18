using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DKA
{
    public Dictionary<string, List<(char, string)>> rules = new Dictionary<string, List<(char, string)>>();
    string word;
    string currentState = "q0";
    string finalState = "q0";
    TextMeshProUGUI text;
    public DKA(Dictionary<string, List<(char, string)>> rules, string word, string finalState, TextMeshProUGUI text)
    {
        this.rules = rules;
        this.word = word;
        this.finalState = finalState;
        this.text = text;
    }
    public void �heckWord()
    {
        foreach (char symbol in word)
        {
            currentState = TransitionFunction(currentState, symbol);
            if (!Examination())
                break;
            Print($" {symbol}: q{currentState} ->\n");
        }
        if (finalState != currentState)
        {
            Print(" ������� �� ����������� �����!\n ������� ������� ������ �� �� �������� ���������\n");
        }
        else
        {
            Print(" ������� ����������� �����!\n");
        }
    }
    private string TransitionFunction(string currentState, char input)
    {
        foreach (var rul in rules)
        {
            if (currentState == rul.Key)
            {
                foreach (var Val in rul.Value)
                {
                    if (input == Val.Item1)
                    {
                        if (Val.Item2 == "qjopa")
                            return "ERROR1";
                        return Val.Item2;
                    }
                }
                return "ERROR2";
            }
        }
        return "ERROR3";
    }
    private bool Examination()
    {
        if (currentState == "ERROR1")
        {
            Print(" ������� �� ����������� �����!\n ��� �������� �� ���������\n");
            return false;
        }
        else if (currentState == "ERROR2")
        {
            Print(" ������� �� ����������� �����!\n ������������ ����������� �������\n");
            return false;
        }
        else if (currentState == "ERROR3")
        {
            Print(" ������� �� ����������� �����!\n ������� ������� ������ �� �� �������� ���������\n");
            return false;
        }
        else
        {
            return true;
        }
    }
    private void Print(string str)
    {
        text.text += str;
    }
}
