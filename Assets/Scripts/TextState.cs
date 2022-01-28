using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    class TextState
    {

        private readonly string TextColorUntypedClose = "#00000033";
        private readonly string TextColorUntypedFar = "#00000011";
        private readonly string TextColorInvisible = "#00000000";
        private readonly string TextColorNext = "#00008b";
        private readonly string TextColorUnderline = "#00008b";
        private readonly string TextColorCorrect = "#006400";
        private readonly string TextColorCompleteMark = "#fafad244";
        private readonly string TextColorWrong = "#8b0000";
        public struct Character
        {
            public char Value;
            public bool IsCorrect;
        }

        public List<Character> Chars;

        public TextState(string text)
        {
            Chars = text.Select((c) => new Character { Value = c, IsCorrect = false }).ToList();
        }

        public string GetRichText(int currentIndex)
        {
            var completedText = String.Join("", Chars.Take(currentIndex).Select(x => x.IsCorrect ? x.Value.ToString() : $"<{TextColorWrong}>{x.Value}<{TextColorCorrect}>").ToArray());
            var uncompletedTextBase = Chars.Skip(currentIndex + 1);
            var uncompletedTextClose = new String(uncompletedTextBase.Take(5).Select(x => x.Value).ToArray());
            var uncompletedTextFar = new String(uncompletedTextBase.Skip(5).Take(5).Select(x => x.Value).ToArray());
            var uncompletedTextVeryFar = new String(uncompletedTextBase.Skip(10).Select(x => x.Value).ToArray());
            var currentCharWithStyling = $"<{TextColorNext}>{Chars[currentIndex].Value}"; 
            return $"<mark={TextColorCompleteMark}><{TextColorCorrect}>{completedText}</mark>{currentCharWithStyling}<{TextColorUntypedClose}>{uncompletedTextClose}<{TextColorUntypedFar}>{uncompletedTextFar}<{TextColorInvisible}>{uncompletedTextVeryFar}";
        }
    }
}
