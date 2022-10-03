using utauPlugin;
using UtauVoiceBank;
using Wave;
using System;
using System.Text;

namespace VCVLyricInserter
{
    class Program
    {
        static string[] A = { "あ", "か", "さ", "た", "な", "は", "ま", "や", "ら", "わ", "が", "ざ", "だ", "ば", "ぱ", "きゃ", "ぎゃ", "にゃ", "ひゃ", "びゃ", "ぴゃ", "みゃ", "りゃ", "じゃ", "ちゃ", "しゃ" };

        static string[] I = {"い", "き", "し", "ち", "に", "ひ", "み", "り", "ぎ", "じ", "び", "ぴ" };

        static string[] U = { "う", "く", "す", "つ", "ぬ", "ふ", "む", "ゆ", "る", "ぐ", "ず", "ぶ", "ぷ", "きゅ", "ぎゅ", "にゅ", "ひゅ", "びゅ", "ぴゅ", "みゅ", "りゅ", "じゅ", "ちゅ", "しゅ" };

        static string[] E = { "え", "け", "せ", "て", "ね", "へ", "め", "れ", "げ", "ぜ", "で", "べ", "ぺ", "じぇ", "ちぇ", "しぇ" };

        static string[] O = { "お", "こ", "そ", "と", "の", "ほ", "も", "よ", "ろ", "を", "ご", "ぞ", "ど", "ぼ", "ぽ", "きょ", "ぎょ", "にょ", "ひょ", "びょ", "ぴょ", "みょ", "りょ", "ちょ", "じょ", "しょ" };

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
            Console.WriteLine("<hiragana/romaji character> <hiragana/romaji character>");

            string characters = Console.ReadLine();

            /* removes spaces */

           if (characters.Contains(' ') == false)
           {
               Console.WriteLine("It seems there was an error. Please make sure your lyrics are formatted correctly and try again.");
               VCV(utauPlugin);
           }

            char firstSound = char.Parse(characters.Substring(0, 1));

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

            int index = 0;
            string lyric = "";
            string previousPhoneme = "";

            while (characters.Contains(' '))
            {
                if (characters.Substring(0, 2).Contains(' '))
                {
                    /* short vowels */
                    
                    lyric += DetermineVowel(previousPhoneme);
                    previousPhoneme = characters.Substring(0, 1);

                    lyric += " ";
                    lyric += characters.Substring(0, 1);
                    characters = characters.Substring(2);

                } else
                {
                    /* long vowels */

                    lyric += DetermineVowel(previousPhoneme);
                    previousPhoneme = characters.Substring(0, 2);

                    lyric += " ";
                    lyric += characters.Substring(0, 2);
                    characters = characters.Substring(3);
                }

                Note note = utauPlugin.note[index];

                note.SetLyric(lyric);

                lyric = "";
                index++;
            }
        }

        static string DetermineVowel(string previousPhoneme)
        {
            if (previousPhoneme == "") {
                return "-";
            }

            foreach(string phoneme in A) {
                if (previousPhoneme == phoneme)
                {
                    return "a";
                }
            }

            foreach(string phoneme in I)
            {
                if (previousPhoneme == phoneme)
                {
                    return "i";
                }
            }

            foreach (string phoneme in U)
            {
                if (previousPhoneme == phoneme)
                {
                    return "u";
                }
            }

            foreach (string phoneme in E)
            {
                if (previousPhoneme == phoneme)
                {
                    return "e";
                }
            }
            
            foreach (string phoneme in O)
            {
                if (previousPhoneme == phoneme)
                {
                    return "o";
                }
            }

            return "n";

        }
            
    }
}
