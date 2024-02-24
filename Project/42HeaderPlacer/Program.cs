
using System;
using System.IO;
using System.Linq;

public class Program
{
    static int HeaderHeight = 11;
    static int HeaderLength = 81;

    static void Main(string[] args)
    {
        if (args.Length != 1)
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error, you must specify just your intra nickname");
            return;
        }

        FileSearching(".", IntraUsername: args[0]);

        //TO DO da levare 
        //Console.ReadLine();
    }

    //Recursive method to found and act in all the files
    static void FileSearching(string Path, string IntraUsername)
    {
        //Linq to filter files by extensions
        string[] files = Directory.GetFiles(path: Path).Where(file => file.ToLower().EndsWith(".h") || file.ToLower().EndsWith(".c")).ToArray();

        foreach (string file in files)
            UpdateHeader(file, IntraUsername);
        foreach (string dir in Directory.GetDirectories(Path))
            FileSearching(dir, IntraUsername);
    }

    static void UpdateHeader(string Path, string IntraUsername)
    {
        try
        {
            WriteHeader(Path, IntraUsername);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[UPDATED]   {Path}");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error during updating 42 header in [{Path}], {ex.Message}");
        }
    }

    #region FileManipulation

    static void WriteHeader(string Path, string IntraUsername)
    {
        //Extract file info
        FileInfo info = new FileInfo(Path);

        string[] header = new string[HeaderHeight];

        //Used as temp to fill them back again
        string[] lines = File.ReadAllLines(Path);

        //Removing all lines
        File.WriteAllText(Path, string.Empty);

        //Variable reused to calculate padding of ' ' to write
        int padding = 0;

        #region HeaderBuilding

        header[0] = "/* ************************************************************************** */";
        header[1] = "/*                                                                            */";
        header[2] = "/*                                                        :::      ::::::::   */";

        #region header[3] (FileName)

        //Line start + file name
        header[3] = $"/*   {info.Name}";

        //Padding: total length - length of already placed chars - end line chars - 1 (boh)
        padding = HeaderLength - header[3].Length - ":+:      :+:    :+:   */".Length - 1;
        //Adding padding
        for (int i = 0; i < padding; i++)
            header[3] += ' ';

        //Line end
        header[3] += ":+:      :+:    :+:   */";

        #endregion

        header[4] = "/*                                                    +:+ +:+         +:+     */";

        #region header[5] (Autor)

        //Line start + Username + mail
        header[5] = $"/*   By: {IntraUsername} <{IntraUsername}@student.42.fr>";

        //Padding: total length - length of already placed chars - end line chars - 1 (boh)
        padding = HeaderLength - header[5].Length - "+#+  +:+       +#+        */".Length - 1;
        //Adding padding
        for (int i = 0; i < padding; i++)
            header[5] += ' ';

        //Line end
        header[5] += "+#+  +:+       +#+        */";

        #endregion

        header[6] = "/*                                                +#+#+#+#+#+   +#+           */";

        #region header[7] (Creation)

        //Line start
        header[7] = $"/*   Created: {info.CreationTime} by {IntraUsername}";

        //Padding: total length - length of already placed chars - end line chars - 1 (boh)
        padding = HeaderLength - header[7].Length - "#+#    #+#             */".Length - 1;
        //Adding padding
        for (int i = 0; i < padding; i++)
            header[7] += ' ';

        //Line end
        header[7] += "#+#    #+#             */";

        #endregion

        #region header[8] (Update)

        //Line start
        //Not using file info cause the last update it that one is going on now
        header[8] = $"/*   Updated: {DateTime.Now} by {IntraUsername}";

        //Padding: total length - length of already placed chars - end line chars - 1 (boh)
        padding = HeaderLength - header[8].Length - "###   ########.fr       */".Length - 1;
        //Adding padding
        for (int i = 0; i < padding; i++)
            header[8] += ' ';

        //Line end
        header[8] += "###   ########.fr       */";

        #endregion

        header[9] = "/*                                                                            */";

        header[10] = "/* ************************************************************************** */";

        #endregion


        File.AppendAllLines(Path, header);
        RewriteFile(Path, lines);
    }

    //I hope it doesn't change git history, cause git should check the code it self and it's changing
    static void RewriteFile(string Path, string[] Lines)
    {
        //Check if there's something else to write
        if (Lines.Length - HeaderHeight <= 0)
            return;

        //If the header exist, start to rewrite lastfile from end of header
        int i = (AlreadyExistHeader(Lines)) ? HeaderHeight : 0;

        //If there is an empy line after the header -> skip it cause it will be writte in the loop
        if (Lines[i] == "")
            i++;

        //Rewriting all lines except header
        while(i < Lines.Length)
        {
            File.AppendAllText(Path, '\n' + Lines[i]);
            i++;
        }
    }

    #endregion

    static bool AlreadyExistHeader(string[] Lines)
    {
        if (Lines.Length < HeaderHeight)
            return (false);

        for (int i = 0; i < HeaderHeight; i++)
            if (!(Lines[i].StartsWith("/*") && Lines[i].EndsWith("*/")))
                return (false);

        return (true);
    }
}