using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    class TextState
    {


        private readonly string TextColorUntypedClose = "#000000aa";
        private readonly string TextColorUntypedFar = "#00000077";
        private readonly string TextColorInvisible = "#00000000";
        private readonly string TextColorNext = "#00008b";
        private readonly string TextColorCorrect = "#111111";
        private readonly string TextColorCompleteMark = "#fafad244";
        private readonly string TextColorWrong = "#ff0000";
        public struct Character
        {
            public char ActualChar;
            public char EnteredChar;
            public bool IsCorrect
            {
                get
                {
                    return ActualChar == EnteredChar;
                }
            }
        }

        public List<Character> Chars;

        public TextState(string text)
        {
            Chars = text.Select((c) => new Character { ActualChar = c, EnteredChar = c == ' ' ? ' ' : 'a' }).ToList();
        }

        public void EnterChar(int index, char c)
        {
            if (index >= Chars.Count) return;
            Chars[index] = new Character
            {
                ActualChar = Chars[index].ActualChar,
                EnteredChar = c
            };

        }

        public string GetRichText(int currentIndex)
        {
            // Completed text uses the 'entered chars'
            var completedText = String.Join("", Chars.Take(currentIndex).Select(x => x.IsCorrect ? x.EnteredChar.ToString() : $"<{TextColorWrong}>{x.EnteredChar}<{TextColorCorrect}>").ToArray());
            
            var uncompletedTextBase = Chars.Skip(currentIndex + 1);
            var uncompletedTextClose = new String(uncompletedTextBase.Take(1).Select(x => x.ActualChar).ToArray());
            var uncompletedTextFar = new String(uncompletedTextBase.Skip(1).Take(9).Select(x => x.ActualChar).ToArray());
            var uncompletedTextVeryFar = new String(uncompletedTextBase.Skip(10).Select(x => x.ActualChar).ToArray());
            var currentCharWithStyling = (currentIndex < Chars.Count)
                ? $"<{TextColorNext}>{Chars[currentIndex].ActualChar}"
                : "";
            return $"<mark={TextColorCompleteMark}><{TextColorCorrect}>{completedText}</mark>{currentCharWithStyling}<{TextColorUntypedClose}>{uncompletedTextClose}<{TextColorUntypedFar}>{uncompletedTextFar}<{TextColorInvisible}>{uncompletedTextVeryFar}";
        }
    }
}
