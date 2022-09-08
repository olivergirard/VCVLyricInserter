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

            Console.WriteLine("Enter your VCV characters in the following format:\n");
            Console.WriteLine("<vowel> <hiragana character> - <vowel> <hiragana character> - <vowel> <hiragana character>");

            Console.ReadLine();

            utauPlugin.Output();
        }
    }
}
