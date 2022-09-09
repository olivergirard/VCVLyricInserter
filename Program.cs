using utauPlugin;
using UtauVoiceBank;
using Wave;
using System;
using System.Text;

namespace VCVLyricInserter
{
    class Program
    {
        static void Main(string[] args)
        {
            EncodingProvider provider = CodePagesEncodingProvider.Instance;
            provider.GetEncoding(932);
            Encoding.RegisterProvider(provider);

            UtauPlugin utauPlugin = new UtauPlugin(args[0]);
            utauPlugin.Input();

            VCV();

            utauPlugin.Output();
        }

        static void VCV()
        {

            Console.WriteLine("Enter your VCV characters in the following format:\n");
            Console.WriteLine("<romaji vowel><hiragana/romaji character>_<romaji vowel><hiragana/romaji character>");

            string characters = Console.ReadLine();

            string removedUnderscores = "";

            /* removes underscores */

            try
            {
                if (characters.Contains('_') == false)
                {
                    Console.WriteLine("It seems there was an error. Please make sure your lyrics are formatted correctly and try again.");
                    VCV();
                }

                while (characters.Contains('_'))
                {
                    removedUnderscores += characters.Substring(0, 2);
                    characters = characters.Substring(3);
                }

                removedUnderscores += characters;

            } catch (Exception)
            {
                Console.WriteLine("It seems there was an error. Please make sure your lyrics are formatted correctly and try again.");
                VCV();
            }

            char firstSound = char.Parse(removedUnderscores.Substring(2, 3));
            string final = "";

            if (((firstSound >= 65) && (firstSound <= 90)) || ((firstSound <= 97) && (firstSound <= 122)))
            {
                /* usage of ASCII characters */

                final = Romaji(removedUnderscores);
            } else
            {
                /* usage of Shift-JIS characters */

                final = Hiragana(removedUnderscores);
            }

            Console.WriteLine("Your Hiragana VCV output is:\n");
            Console.WriteLine(final);
        }

        static string Romaji(string removedUnderscores)
        {

        }
        static string Hiragana(string removedUnderscores)
        {

        }
    }
}
