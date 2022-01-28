using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    class TextState
    {

        private readonly string TextColorUntyped = "#000000";
        private readonly string TextColorNext = "#00008b";
        private readonly string TextColorCorrect = "#006400";
        private readonly string TextColorCompleteMark = "#fafad244";
        private readonly string TextColorWrong = "#8b0000";
        public struct Character
        {
            public char Value;
            public bool IsCorrect;
        }

        public List<Character> Chars;
        public int CurrentIndex;

        public TextState(string text)
        {
            Chars = text.Select((c) => new Character { Value = c, IsCorrect = false }).ToList();
        }

        public string GetRichText()
        {
            var completedText = String.Join("", Chars.Take(CurrentIndex).Select(x => x.IsCorrect ? x.Value.ToString() : $"<{TextColorWrong}>{x.Value}<{TextColorCorrect}>").ToArray());
            var uncompletedText = new String(Chars.Skip(CurrentIndex + 1).Select(x => x.Value).ToArray());
            var currentChar = Chars[CurrentIndex].Value;
            return $"<mark={TextColorCompleteMark}><{TextColorCorrect}>{completedText}</mark><{TextColorNext}><u>{currentChar}</u><{TextColorUntyped}>{uncompletedText}";


        }
    }
}
