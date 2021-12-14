using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EnigmaSvgCore
{
    public class Command
    {
        public enum CommandName
        {
            Unknown,
            M,
            m,
            L,
            l,
            H,
            h,
            V,
            v,
            C,
            c,
            Z,
            z
        };

        //public int Order { get; }
        public CommandName Name { get; set; } = CommandName.Unknown;
        public float[] Values { get; set; } = new float[0];

        public Command(CommandName name) //, int order)
        {
            Name = name;
            //Order = order;
        }

        public Command(CommandName name, float[] values) //int order, float[] values)
        {
            Name = name;
            Values = new float[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                Values[i] = values[i];
            }
            //Order = order;
        }

        public static Command[] ParseCommandsFromD(string dValues)
        {
            List<Command> allCommands = new List<Command>();
            var dCommandsAndPoints = Regex.Matches(dValues, @"([A-Za-z])|([-]?\d+[.]?\d*([Ee]-[0-9]+)?)");

            CommandName currentCommandName = CommandName.Unknown;
            List<float> currentCommandValues = new List<float>();
            for (int i = 0; i < dCommandsAndPoints.Count; i++)
            {
                if (Regex.IsMatch(dCommandsAndPoints[i].Value, @"[A-Da-dF-Zf-z]"))
                {
                    if (i > 0)
                    {
                        allCommands.Add(new Command(currentCommandName, currentCommandValues.ToArray()));
                    }
                    currentCommandName = (CommandName)Enum.Parse(typeof(CommandName), dCommandsAndPoints[i].Value, false);
                    currentCommandValues = new List<float>();
                    continue;
                }
                else
                {
                    currentCommandValues.Add(float.Parse(dCommandsAndPoints[i].Value));
                }
            }
            
            allCommands.Add(new Command(currentCommandName, currentCommandValues.ToArray()));

            return allCommands.ToArray();
        }

        public static Command[] IdentifyAllCommandNames(Command[] allCommands)
        {
            // Идентификация имени команды для "одиноких" значений (например, M 1.0 1.0 5.0 5.0 => M 1.0 1.0 L 5.0 5.0;)
            List<Command> resultCommands = new List<Command>();
            foreach (var command in allCommands)
            {
                if (command.Name == CommandName.M)
                {
                    resultCommands.Add(new Command(command.Name, new float[] { command.Values[0], command.Values[1] }));
                    for (int i = 2; i < command.Values.Length; i += 2)
                    {
                        resultCommands.Add(new Command(CommandName.L, new float[] { command.Values[i], command.Values[i + 1] }));
                    }
                }
                else if (command.Name == CommandName.m)
                {
                    resultCommands.Add(new Command(command.Name, new float[] { command.Values[0], command.Values[1] }));
                    for (int i = 2; i < command.Values.Length; i += 2)
                    {
                        resultCommands.Add(new Command(CommandName.l, new float[] { command.Values[i], command.Values[i + 1] }));
                    }
                }
                else if (command.Name == CommandName.L)
                {
                    resultCommands.Add(new Command(command.Name, new float[] { command.Values[0], command.Values[1] }));
                    for (int i = 2; i < command.Values.Length; i += 2)
                    {
                        resultCommands.Add(new Command(command.Name, new float[] { command.Values[i], command.Values[i + 1] }));
                    }
                }
                else if (command.Name == CommandName.l)
                {
                    resultCommands.Add(new Command(command.Name, new float[] { command.Values[0], command.Values[1] }));
                    for (int i = 2; i < command.Values.Length; i += 2)
                    {
                        resultCommands.Add(new Command(command.Name, new float[] { command.Values[i], command.Values[i + 1] }));
                    }
                }
                else if (command.Name == CommandName.C)
                {
                    resultCommands.Add(new Command(command.Name, new float[] { command.Values[0], command.Values[1], command.Values[2], command.Values[3], command.Values[4], command.Values[5] }));
                    for (int i = 6; i < command.Values.Length; i += 6)
                    {
                        resultCommands.Add(new Command(command.Name, new float[] { command.Values[i], command.Values[i + 1], command.Values[i + 2], command.Values[i + 3], command.Values[i + 4], command.Values[i + 5] }));
                    }
                }
                else if (command.Name == CommandName.c)
                {
                    resultCommands.Add(new Command(command.Name, new float[] { command.Values[0], command.Values[1], command.Values[2], command.Values[3], command.Values[4], command.Values[5] }));
                    for (int i = 6; i < command.Values.Length; i += 6)
                    {
                        resultCommands.Add(new Command(command.Name, new float[] { command.Values[i], command.Values[i + 1], command.Values[i + 2], command.Values[i + 3], command.Values[i + 4], command.Values[i + 5] }));
                    }
                }
                else if (command.Name == CommandName.Z || command.Name == CommandName.z)
                {
                    resultCommands.Add(new Command(command.Name, new float[0]));
                }
            }

            return resultCommands.ToArray();
        }

        //public static Command[] DistinctDuplicateCommands(Command[] identifiedCommands)
        //{
        //    List<Command> newCommands = new List<Command>();
        //    for (int c = 1; c < identifiedCommands.Length; c++)
        //    {
        //        if (identifiedCommands[c].Name == CommandName.Z || identifiedCommands[c].Name == CommandName.z)
        //        {
        //            continue;
        //        }
        //        if (identifiedCommands[c].Name == identifiedCommands[c - 1].Name)
        //        {
        //            if (identifiedCommands[c].Values.SequenceEqual(identifiedCommands[c - 1].Values))
        //            {
        //                newCommands.Add(new Command(identifiedCommands[c].Name, identifiedCommands[c].Values));
        //            }
        //            else
        //            {
        //                newCommands.Add(new Command(identifiedCommands[c - 1].Name, identifiedCommands[c - 1].Values));
        //            }
        //        }
        //    }

        //    return newCommands.ToArray();
        //}

        public Point[] GetPoints()
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < Values.Length; i += 2)
            {
                points.Add(new Point(Values[i], Values[i + 1]));
            }
            return points.ToArray();
        }
    }
}
