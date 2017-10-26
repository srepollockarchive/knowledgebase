using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace knowledgebase
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser();
            ArrayList individuals = new ArrayList();
            ArrayList rules = new ArrayList();
            ArrayList strings = new ArrayList();
            int counter = 0;
            string line;
            // Read the file and display it line by line.
            System.IO.StreamReader file =
                new System.IO.StreamReader(args[0]);
            while ((line = file.ReadLine()) != null)
            {
                strings.Add(line);
                counter++;
            }
            foreach (string l in strings)
            {
                parser.ParseLine(l, individuals, rules);
            }
            foreach (Individual i in individuals)
            {
                Console.WriteLine(i.ToString());
            }
            foreach (Rule r in rules)
            {
                Console.WriteLine(r.ToString());
            }
            Console.WriteLine("Press 'ESC' to close the window.");
            if (Console.ReadKey(true).Key == ConsoleKey.Escape) return;
        }
    }

    class Individual
    {
        public string name;
        public ArrayList properties;
        public Individual()
        {
            this.name = "";
            this.properties = new ArrayList();
        }
        public Individual(string name, Property property)
        {
            this.name = name;
            this.properties = new ArrayList();
            this.properties.Add(property);
        }
        public Individual(string name, ArrayList properties)
        {
            this.name = name;
            this.properties = properties;
        }
        public override string ToString()
        {
            string properties = "";
            foreach (Property p in this.properties)
            {
                properties += p.ToString() + ", ";
            }
            return "Individual name = " + this.name + " Properties = " + properties;
        }
    }
    class Property
    {
        public string type;
        public Property()
        {
            this.type = "";
        }
        public Property(string type)
        {
            this.type = type;
        }
        public override string ToString()
        {
            return "Property = " + this.type;
        }
    }
    class Rule
    {
        public ArrayList antecedents;
        public Individual consequence;
        public Rule()
        {
            this.antecedents = new ArrayList();
            this.consequence = new Individual();
        }
        public Rule(ArrayList antecedents, Individual individual)
        {
            this.antecedents = antecedents;
            this.consequence = individual;
        }
        public override string ToString()
        {
            return "Antecedents = " + this.antecedents.ToString() + " Consequence = " + this.consequence;
        }
    }
    class Parser
    {
        public Individual GetIndividual(string line, ArrayList individiuals, ArrayList rules)
        {
            String[] split = Regex.Split(line, @"[(|)]"); // TODO: Check if this works
            foreach (Individual i in individiuals)
            {
                if (split.Length <= 0) Console.WriteLine("error");
                if (i.name == split[1])
                {
                    i.properties.Add(new Property(split[0]));
                    return null;
                }
            }
            foreach (Rule r in rules)
            {
                foreach (Individual i in r.antecedents)
                {
                    if (i.name == split[1])
                    {
                        i.properties.Add(new Property(split[0]));
                        return null;
                    }
                }
            }
            return new Individual(split[1], new Property(split[0]));
        }
        public void ParseLine(string line, ArrayList individuals, ArrayList rules)
        {
            string[] split = Regex.Split(line, @"[,|>|' ']");
            int count = 0;
            foreach (string individualLine in split)
            {
                if (individualLine == "") continue;
                Individual i = GetIndividual(individualLine, individuals, rules);
                if (i != null)
                {
                    individuals.Add(i);
                    count++;
                }
            }
            if (count == 1) return;
            ArrayList ruleIndividuals = GetLastIndividual(count, individuals);
            Individual consequence = (Individual)ruleIndividuals[ruleIndividuals.Count - 1];
            ruleIndividuals.RemoveAt(ruleIndividuals.Count - 1);
            rules.Add(new Rule(ruleIndividuals, consequence));
            return;
        }
        private ArrayList GetLastIndividual(int c, ArrayList individuals)
        {
            ArrayList lastIndividuals = new ArrayList();
            while (c != 0)
            {
                lastIndividuals.Add(individuals[individuals.Count - c - 1]);
                c--;
            }
            return lastIndividuals;
        }
    }
}
