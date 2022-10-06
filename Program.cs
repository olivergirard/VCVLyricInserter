using utauPlugin;
using UtauVoiceBank;
using Wave;
using System;
using System.Text;

namespace VCVLyricInserter
{

    //TODO fix first note issue
    class Program
    {

        /* hiragana arrays */

        static string[] hiraganaA = { "あ", "か", "さ", "た", "な", "は", "ま", "や", "ら", "わ", "が", "ざ", "だ", "ば", "ぱ", "きゃ", "ぎゃ", "にゃ", "ひゃ", "びゃ", "ぴゃ", "みゃ", "りゃ", "じゃ", "ちゃ", "しゃ" };

        static string[] hiraganaI = {"い", "き", "し", "ち", "に", "ひ", "み", "り", "ぎ", "じ", "び", "ぴ" };

        static string[] hiraganaU = { "う", "く", "す", "つ", "ぬ", "ふ", "む", "ゆ", "る", "ぐ", "ず", "ぶ", "ぷ", "きゅ", "ぎゅ", "にゅ", "ひゅ", "びゅ", "ぴゅ", "みゅ", "りゅ", "じゅ", "ちゅ", "しゅ" };

        static string[] hiraganaE = { "え", "け", "せ", "て", "ね", "へ", "め", "れ", "げ", "ぜ", "で", "べ", "ぺ", "じぇ", "ちぇ", "しぇ" };

        static string[] hiraganaO = { "お", "こ", "そ", "と", "の", "ほ", "も", "よ", "ろ", "を", "ご", "ぞ", "ど", "ぼ", "ぽ", "きょ", "ぎょ", "にょ", "ひょ", "びょ", "ぴょ", "みょ", "りょ", "ちょ", "じょ", "しょ" };

        /* romaji arrays */

        static string[] romajiA = { "a", "ka", "sa", "ta", "na", "ha", "ma", "ya", "ra", "wa", "ga", "za", "da", "ba", "pa", "kya", "gya", "nya", "hya", "bya", "pya", "mya", "rya", "ja", "cha", "sha" };

        static string[] romajiI = { "i", "ki", "shi", "chi", "ni", "hi", "mi", "ri", "gi", "ji", "bi", "pi" };

        static string[] romajiU = { "u", "ku", "su", "tsu", "nu", "fu", "mu", "yu", "ru", "gu", "zu", "bu", "pu", "kyu", "gyu", "nyu", "hyu", "byu", "pyu", "myu", "ryu", "ju", "chu", "shu" };

        static string[] romajiE = { "e", "ke", "se", "te", "ne", "he", "me", "re", "ge", "ze", "de", "be", "pe", "je", "che", "she" };

        static string[] romajiO = { "o", "ko", "so", "to", "no", "ho", "mo", "yo", "ro", "wo", "go", "zo", "do", "bo", "po", "kyo", "gyo", "nyo", "hyo", "byo", "pyo", "myo", "ryo", "cho", "jo", "sho" };

        /* main function necessary for execution */

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

        /* VCV conversion function */

        static void VCV(UtauPlugin utauPlugin)
        {

            Console.WriteLine("Enter your VCV characters in the following format. There should be a space after EVERY phoneme.\n");
            Console.WriteLine("<hiragana/romaji character> <hiragana/romaji character> ");

            string characters = Console.ReadLine();

            /* removes spaces */

           if (characters.Contains(' ') == false)
           {
               Console.WriteLine("It seems there was an error. Please make sure your lyrics are formatted correctly and try again.");
               VCV(utauPlugin);
           }

            char firstSound = char.Parse(characters.Substring(0, 1));

            DetermineType(characters, utauPlugin);
        }

        /* function used if input is in romaji format */

        static void Romaji(string characters, UtauPlugin utauPlugin)
        {
            string newCharacters = "";

            while (characters.Contains(' '))
            {

                if (characters.Substring(0, 2).Contains(' '))
                {
                    /* vowels only */

                    newCharacters += ConvertToHiragana(characters.Substring(0, 1));
                    characters = characters.Substring(2);

                }
                else if (characters.Substring(0, 3).Contains(' '))
                {
                    /* short phonemes */

                    newCharacters += ConvertToHiragana(characters.Substring(0, 2));
                    characters = characters.Substring(3);

                }
                else
                {
                    /* long phonemes */

                    newCharacters += ConvertToHiragana(characters.Substring(0, 3));
                    characters = characters.Substring(4);
                }

                newCharacters += " ";
            }

            Hiragana(newCharacters, utauPlugin);
        }

        /* function used if input is in hiragana format */
        static void Hiragana(string characters, UtauPlugin utauPlugin)
        {

            int index = 0;
            string lyric = "";
            string previousPhoneme = "-";
            Note note = utauPlugin.note[0];

            while ((characters.Contains(' ')) && (note != null))
            {

                if (note.GetLyric().ToUpper() == "R")
                {
                    lyric = "R";
                    previousPhoneme = "-";
                }
                else
                {

                    if (characters.Substring(0, 2).Contains(' '))
                    {
                        /* short phonemes */

                        lyric += DetermineHiraganaVowel(previousPhoneme);
                        previousPhoneme = characters.Substring(0, 1);

                        lyric += " ";
                        lyric += characters.Substring(0, 1);
                        characters = characters.Substring(2);

                    }
                    else
                    {
                        /* long phonemes */

                        lyric += DetermineHiraganaVowel(previousPhoneme);
                        previousPhoneme = characters.Substring(0, 2);

                        lyric += " ";
                        lyric += characters.Substring(0, 2);
                        characters = characters.Substring(3);
                    }
                }

                note.SetLyric(lyric);

                lyric = "";
                index++;
                note = utauPlugin.note[index];
            }
        }

        /* if input is in hiragana format, the romaji vowel is determined */

        static string DetermineHiraganaVowel(string previousPhoneme)
        {

            foreach(string phoneme in hiraganaA) {
                if (previousPhoneme == phoneme)
                {
                    return "a";
                }
            }

            foreach(string phoneme in hiraganaI)
            {
                if (previousPhoneme == phoneme)
                {
                    return "i";
                }
            }

            foreach (string phoneme in hiraganaU)
            {
                if (previousPhoneme == phoneme)
                {
                    return "u";
                }
            }

            foreach (string phoneme in hiraganaE)
            {
                if (previousPhoneme == phoneme)
                {
                    return "e";
                }
            }
            
            foreach (string phoneme in hiraganaO)
            {
                if (previousPhoneme == phoneme)
                {
                    return "o";
                }
            }

            return "n";

        }

        static string ConvertToHiragana(string substring)
        {
            //TODO convert this to hiragana using the romaji and hiragana arrays

            for (int i = 0; i < romajiA.Length; i++)
            {
                if (substring == romajiA[i])
                {
                    return hiraganaA[i];
                }
            }

            for (int i = 0; i < romajiI.Length; i++)
            {
                if (substring == romajiI[i])
                {
                    return hiraganaI[i];
                }
            }

            for (int i = 0; i < romajiU.Length; i++)
            {
                if (substring == romajiU[i])
                {
                    return hiraganaU[i];
                }
            }

            for (int i = 0; i < romajiE.Length; i++)
            {
                if (substring == romajiE[i])
                {
                    return hiraganaE[i];
                }
            }

            for (int i = 0; i < romajiO.Length; i++)
            {
                if (substring == romajiO[i])
                {
                    return hiraganaO[i];
                }
            }

            return "ん";

        }

        static void DetermineType(string characters, UtauPlugin utauPlugin)
        {
            bool hiragana = true;

            foreach (string phoneme in romajiA)
            {
                if (characters.Substring(0, 1) == phoneme.Substring(0, 1))
                {
                    hiragana = false;
                }
            }

            foreach (string phoneme in romajiI)
            {
                if (characters.Substring(0, 1) == phoneme.Substring(0, 1))
                {
                    hiragana = false;
                }
            }

            foreach (string phoneme in romajiU)
            {
                if (characters.Substring(0, 1) == phoneme.Substring(0, 1))
                {
                    hiragana = false;
                }
            }

            foreach (string phoneme in romajiE)
            {
                if (characters.Substring(0, 1) == phoneme.Substring(0, 1))
                {
                    hiragana = false;
                }
            }

            foreach (string phoneme in romajiO)
            {
                if (characters.Substring(0, 1) == phoneme.Substring(0, 1))
                {
                    hiragana = false;
                }
            }

            if (!hiragana)
            {
                Romaji(characters, utauPlugin);
            } else
            {
                Hiragana(characters, utauPlugin);
            }

        }

    }
}
