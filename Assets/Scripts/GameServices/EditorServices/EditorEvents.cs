using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameInfo;
using GameInfo.GameInfoInternals;
using GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals;
using GameExtensions.Editor;

namespace GameServices.Editor
{
    public class EditorEvents : BaseService
    {
        private PlacedPieceInfo PlacedPieceInfo => editorInfo.PlacedPiece;

        public void AbortPlacing()
        {
            // canvasOf2Btns.SetActive(false);
            /*EnabledYesNoButtons(false);

            currentPositionFromAvailable = 0;
            if (GetComponent<ScreenOrientationScript>().screenOrientation == ScreenOrientation.Portrait) levelMenu.ToggleRightPanelHideFeatureOn(true);*/
            editorInfo.Workspace.IsRotating = false;
            editorInfo.AbortPlacing();
        }
    }
}
