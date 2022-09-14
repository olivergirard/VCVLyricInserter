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

            VCV(utauPlugin);

            utauPlugin.Output();
        }

        static void VCV(UtauPlugin utauPlugin)
        {

            Console.WriteLine("Enter your VCV characters in the following format:\n");
            Console.WriteLine("<romaji vowel><hiragana/romaji character>_<romaji vowel><hiragana/romaji character>");

            string characters = Console.ReadLine();

            /* removes underscores */

           if (characters.Contains('_') == false)
           {
               Console.WriteLine("It seems there was an error. Please make sure your lyrics are formatted correctly and try again.");
               VCV(utauPlugin);
           }

            char firstSound = char.Parse(characters.Substring(2, 3));

            if (((firstSound >= 65) && (firstSound <= 90)) || ((firstSound <= 97) && (firstSound <= 122)))
            {
                /* usage of ASCII characters */

                Romaji(characters, utauPlugin);
            } else
            {
                /* usage of Shift-JIS characters */

                Hiragana(characters, utauPlugin);
            }
        }

        static void Romaji(string characters, UtauPlugin utauPlugin)
        {

        }
        static void Hiragana(string characters, UtauPlugin utauPlugin)
        {

            //TODO exception handling during debug testing

            string lyric = "";
            int index = 0;

            while (characters.Contains('_'))
            {
                lyric += characters.Substring(0, 1);
                lyric += " ";
                characters = characters.Substring(1);
                
                if ((characters.Substring(0, 2)).Contains('_'))
                {
                    /* short vowels */

                    lyric += characters.Substring(0, 1);
                    characters = characters.Substring(2);

                } else
                {
                    /* long vowels */

                    lyric += characters.Substring(0, 2);
                    characters = characters.Substring(3);
                }
                Note note = utauPlugin.note[index];

                note.SetLyric(lyric);

                lyric = "";
                index++;
            }
        }
            
    }
}
