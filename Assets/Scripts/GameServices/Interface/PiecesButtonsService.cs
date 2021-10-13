using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameExtensions.Interface;
using GameExtensions.Solution;
using GameInfo.GameInfoInternals.InterfaceInfoInternals;
using GameInfo.GameInfoInternals.InterfaceInfoInternals.PiecesButtonsInfoInternals;
using static Enums;

namespace GameServices.Interface
{
    public class PiecesButtonsService : BaseService
    {
        private PiecesButtonsInfo info => interfaceInfo.PiecesButtons;

        private void SetDefaultColorForPieces()
        {
            for (int i = 0; i < info.Buttons.Length; i++)
                info.Buttons[i].SetDefaultColorForPiece();

        }

        public void RenewButtons()
        {
            SetRandomSideFromGeneratedSolutionForButtons();
            for (int i = 0; i < info.Buttons.Length; i++)
            {
                int i2 = i;
                info.Buttons[i].PhysicalObject.onClick.RemoveAllListeners();
                //info.Buttons[i].PhysicalObject.onClick.AddListener(() => PlacePiece(editorInfo.GeneratedSolution.GetBySide(info.Buttons[i2].SideFromGeneratedSolution.Value), i2));
                info.Buttons[i].ChangePieceSetting(editorInfo.GeneratedSolution.GetBySide(info.Buttons[i2].SideFromGeneratedSolution.Value));
                info.Buttons[i].SetEnableState(true);
            }
        }
        public void RecalculateButtonsCams(int delayBy = 1)
        {
            //foreach (Button button in buttons)
            //{
            //    button.GetComponent<ApplySettingToBtn>().RecalculateUI(delayBy);
            //}
        }

        private void SetRandomSideFromGeneratedSolutionForButtons()
        {
            Side[] order = Enum.GetValues(typeof(Side)) as Side[];

            Random random = new Random();
            for (int i = 0; i < order.Length / 2; i++)
            {
                int index1 = random.Next(order.Length), index2;
                do
                {
                    index2 = random.Next(order.Length);
                } while (index1 == index2);
                Side backup = order[index1];
                order[index1] = order[index2];
                order[index2] = backup;
            }

            for (int i = 0; i < info.Buttons.Length; i++)
            {
                info.Buttons[i].SideFromGeneratedSolution = order[i];
            }
        }

        public void SetEnableStateForAll(bool enable)
        {
            for (int i = 0; i < info.Buttons.Length; i++)
                SetEnableState(i, enable);
        }

        public void SetEnableState(int index, bool enable)
        {
            info.Buttons[index].SetEnableState(enable);
        }

        public PieceButtonInfo GetButtonBySideOnGeneratedSolution(Side side)
        {
            PieceButtonInfo result = null;

            foreach (var button in info.Buttons)
            {
                if (button.SideFromGeneratedSolution == side)
                {
                    if (result != null)
                        throw new MoreThanOneButtonForSideException();
                    else
                        result = button;
                }
            }

            return result;
        }

        public class MoreThanOneButtonForSideException : Exception
        {
            public override string Message => $"In {typeof(PiecesButtonsInfo).Name} there are at least two {typeof(PieceButtonInfo).Name} objects with same value of {nameof(PieceButtonInfo.SideFromGeneratedSolution)}";
        }
    }
}
