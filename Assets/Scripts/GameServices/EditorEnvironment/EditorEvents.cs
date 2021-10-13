using GameExtensions.Editor;
using GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals;

namespace GameServices.Editor
{
    public class EditorEvents : BaseService
    {
        private PlacedPieceInfo placedPieceInfo => editorInfo.PlacedPiece;

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
