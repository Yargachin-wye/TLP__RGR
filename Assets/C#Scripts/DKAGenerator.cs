using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DKAGenerator
{
    private List<char> alphabet;
    private string finalSubchain;
    private char countSymbol;
    private int count;
    private int countOfCountSymbol;

    public DKAGenerator(List<char> alphabet, string finalSubchain, char countSymbol, int count)
    {
        this.alphabet = alphabet;
        this.finalSubchain = finalSubchain;
        this.countSymbol = countSymbol;
        this.count = count;
        this.countOfCountSymbol = 0;
    }

    public Tuple<Dictionary<string, Dictionary<char, string>>, string> GenerateDKA()
    {
        Console.WriteLine("\n\n### Generate START ###\n");

        List<string> states = new List<string> { "q0" };
        Dictionary<string, Dictionary<char, string>> transitions = new Dictionary<string, Dictionary<char, string>> { { "q0", new Dictionary<char, string>() } };
        List<string> finalStates = new List<string>();
        List<string> dopStates = new List<string>();

        string currentState = "q0";
        Dictionary<string, int> counter = new Dictionary<string, int>();
        int nowCount = this.count;
        this.countOfCountSymbol = this.finalSubchain.Split(this.countSymbol).Length - 1;

        int ifSubLess = 0;
        int neededWeight;
        // <del
        if (this.countOfCountSymbol == this.count)
        {
            neededWeight = this.count;
        }
        else if (this.count - this.countOfCountSymbol < 0)
        {
            if (this.countOfCountSymbol % this.count != 0)
            {
                var difference = this.countOfCountSymbol - this.count;
                neededWeight = this.countOfCountSymbol + difference;
                Console.WriteLine(neededWeight);
                while (neededWeight % this.count != 0)
                {
                    ifSubLess += 1;
                    neededWeight += 1;
                    Console.WriteLine(difference + " " + neededWeight + " " + ifSubLess);
                }
            }
            else
            {
                neededWeight = this.countOfCountSymbol - this.count;
            }
        }
        // del>
        Console.WriteLine("\n- 1st transf -\n");

        if (this.count == 1)
        {
            // pass
        }
        else if (this.countOfCountSymbol == 0)
        {
            for (int i = 0; i < this.count + 1; i++)
            {
                string newState = $"q{states.Count}";
                transitions[currentState] = new Dictionary<char, string>();
                counter[currentState] = i;
                if (i < this.count)
                {
                    transitions[currentState][this.countSymbol] = newState;
                    states.Add(newState);
                    currentState = newState;
                    counter[currentState] = i + 1;
                    nowCount = i + 1;
                }
            }
        }
        else if (this.countOfCountSymbol >= 1)
        {
            if (this.count - this.countOfCountSymbol == 0)
            {
                counter[currentState] = 0;
                string newState = $"q{states.Count}";
                transitions[currentState] = new Dictionary<char, string>();
                transitions[currentState][this.countSymbol] = newState;
                states.Add(newState);
                currentState = newState;
                nowCount = 1;
                counter[currentState] = 1;
            }
            else
            {
                for (int i = 0; i < Math.Abs(this.count - this.countOfCountSymbol) + ifSubLess; i++)
                {
                    counter[currentState] = i;
                    string newState = $"q{states.Count}";
                    transitions[currentState] = new Dictionary<char, string>();
                    transitions[currentState][this.countSymbol] = newState;
                    states.Add(newState);
                    currentState = newState;
                    if (i == Math.Abs(this.count - this.countOfCountSymbol) - 1 + ifSubLess)
                    {
                        nowCount = i + 1;
                        counter[currentState] = i + 1;
                    }
                }
            }
        }

        Console.WriteLine(currentState);
        Console.WriteLine(nowCount);
        Console.WriteLine(string.Join(", ", counter));
        Console.WriteLine(string.Join(", ", transitions));
        Console.WriteLine(string.Join(", ", states));

        Console.WriteLine("\n- Other transf -\n");

        if (this.count == 1)
        {
            // pass
        }
        else if (this.countOfCountSymbol == 0)
        {
            for (int i = 0; i < this.count + 1; i++)
            {
                for (int j = 0; j < this.alphabet.Count; j++)
                {
                    char symbol = this.alphabet[j];
                    if (symbol != this.countSymbol)
                    {
                        transitions[states[i]][symbol] = states[i];
                    }
                }
            }
        }
        else if (this.countOfCountSymbol >= 1)
        {
            if (this.count - this.countOfCountSymbol == 0)
            {
                for (int j = 0; j < this.alphabet.Count; j++)
                {
                    char symbol = this.alphabet[j];
                    if (symbol != this.countSymbol)
                    {
                        transitions[states[0]][symbol] = states[0];
                    }
                }
            }
            else
            {
                for (int i = 0; i < Math.Abs(this.count - this.countOfCountSymbol) + ifSubLess; i++)
                {
                    for (int j = 0; j < this.alphabet.Count; j++)
                    {
                        char symbol = this.alphabet[j];
                        if (symbol != this.countSymbol)
                        {
                            transitions[states[i]][symbol] = states[i];
                        }
                    }
                }
            }
        }

        Console.WriteLine(string.Join(", ", transitions));
        Console.WriteLine(currentState);

        Console.WriteLine("\n- For end sub transf -\n");

        finalStates.Add(currentState);

        if (this.count - this.countOfCountSymbol == 0)
        {
            foreach (char character in this.finalSubchain.Substring(1))
            {
                string newState = $"q{states.Count}";
                states.Add(newState);
                transitions[currentState] = new Dictionary<char, string>();
                transitions[currentState][character] = newState;
                currentState = newState;
                if (character == this.countSymbol)
                {
                    nowCount += 1;
                    counter[currentState] = nowCount;
                }
                else
                {
                    counter[currentState] = nowCount;
                }
                transitions[currentState] = new Dictionary<char, string>();
                finalStates.Add(currentState);
            }
        }
        else
        {
            foreach (char character in this.finalSubchain)
            {
                string newState = $"q{states.Count}";
                states.Add(newState);
                transitions[currentState] = new Dictionary<char, string>();
                transitions[currentState][character] = newState;
                currentState = newState;
                if (character == this.countSymbol)
                {
                    nowCount += 1;
                    counter[currentState] = nowCount;
                }
                else
                {
                    counter[currentState] = nowCount;
                }
                transitions[currentState] = new Dictionary<char, string>();
                finalStates.Add(currentState);
            }
        }

        Console.WriteLine(string.Join(", ", transitions));
        Console.WriteLine(string.Join(", ", finalStates));
        Console.WriteLine(nowCount);
        Console.WriteLine(string.Join(", ", counter));

        int basicStatesCount = states.Count;

        Console.WriteLine("\n- From end sub transf -\n");

        foreach (string state in finalStates)
        {
            foreach (char symbol in this.alphabet)
            {
                if (symbol != this.countSymbol)
                {
                    if (new HashSet<char>(this.finalSubchain).Count == 1 &&
                        symbol == this.finalSubchain[0] &&
                        state == finalStates[finalStates.Count - 1])
                    {
                        transitions[state][symbol] = state;
                    }
                    else if (!transitions[state].ContainsKey(symbol))
                    {
                        if (this.count == 1)
                        {
                            if (symbol == this.finalSubchain[0])
                            {
                                transitions[state][symbol] = states[1];
                            }
                            else
                            {
                                transitions[state][symbol] = states[0];
                            }
                        }
                        else
                        {
                            string nextState = this.SearchState(transitions, state, states, symbol, finalStates, counter, counter[state], dopStates, basicStatesCount);
                            transitions[state][symbol] = nextState;
                        }
                    }
                }
                else
                {
                    if (!transitions[state].ContainsKey(symbol))
                    {
                        if (this.count == 1)
                        {
                            if (symbol == this.finalSubchain[0])
                            {
                                transitions[state][symbol] = states[1];
                            }
                            else
                            {
                                transitions[state][symbol] = states[0];
                            }
                        }
                        else
                        {
                            string nextState = this.SearchState(transitions, state, states, symbol, finalStates, counter, counter[state], dopStates, basicStatesCount);
                            transitions[state][symbol] = nextState;
                        }
                    }
                }
            }
        }

        Console.WriteLine("\n" + string.Join(", ", transitions));

        return new Tuple<Dictionary<string, Dictionary<char, string>>, string>(transitions, finalStates[finalStates.Count - 1]);
    }

    private string SearchState(
        Dictionary<string, Dictionary<char, string>> transitions,
        string state,
        List<string> states,
        char symbol,
        List<string> finalStates,
        Dictionary<string, int> counter,
        int nowCount,
        List<string> dopStates,
        int basicStatesCount,
        int dop = 0
        )
    {
        int newCount;
        if (symbol == this.countSymbol)
        {
            newCount = (nowCount + 1 + dop) % this.count;
        }
        else
        {
            newCount = (nowCount + dop) % this.count;
        }

        string result = counter.FirstOrDefault(x => x.Value == newCount).Key;

        if (finalStates.Contains(result) && result != finalStates[0])
        {
            if (dopStates.Count == 0)
            {
                string newDopState = $"q{basicStatesCount + dopStates.Count}";
                dopStates.Add(newDopState);
                states.Add(newDopState);
                transitions[newDopState] = new Dictionary<char, string>();
                int newDop = this.count - nowCount;
                transitions[newDopState][this.countSymbol] = this.SearchState(transitions, state, states, symbol, finalStates, counter, counter[state], dopStates, basicStatesCount, newDop);

                foreach (char s in this.alphabet)
                {
                    if (s != this.countSymbol)
                    {
                        transitions[newDopState][s] = newDopState;
                    }
                }

                if (!transitions[state].ContainsKey(symbol))
                {
                    transitions[state][symbol] = newDopState;
                }

                return newDopState;
            }
            else
            {
                return dopStates[dopStates.Count - 1];
            }
        }
        else
        {
            return result;
        }
    }
}
