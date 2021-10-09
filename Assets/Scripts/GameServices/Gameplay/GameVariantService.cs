using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Enums;

using GameInfo.GameInfoInternals;
using GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals;
using GameExtensions.RelativePlacingPiecesPositions;

namespace GameServices.Gameplay
{
    public class GameVariantService : BaseService
    {
        private readonly GameplayInitialization gameplayService = new GameplayInitialization();
        private PiecesPrefabsInfo prefabInfo => editorInfo.PiecesPrefabs;
        private float sideLengthMultipler;

        // TODO: refactor
        public void ChooseVariantAndStartNewGame(Variant variant)
        {
            if (editorInfo.Variant != variant)
            {
                editorInfo.Variant = variant;
                InitVariant(autoStartRandomGame: true);
            }
        }
        
        public void ChooseVariant(Variant variant)
        {
            if (editorInfo.Variant != variant)
            {
                editorInfo.Variant = variant;
                InitVariant(autoStartRandomGame: false);
            }
        }

        public void ChooseVariantAndStartNewGame(int var)
        {
            Variant variant_ = Variant.x4;
            if (var == 0) variant_ = Variant.x3;
            if (var == 1) variant_ = Variant.x4;
            ChooseVariantAndStartNewGame(variant_);
        }

        private void InitVariant(bool autoStartRandomGame = false)
        {
            SelectProperPiecePrefab();

            editorInfo.CurrentSolution = new SolutionInfo<bool>(editorInfo.Variant);
            editorInfo.ShiftedSolution = new SolutionInfo<bool>(editorInfo.Variant);

            editorInfo.Workspace.RelativePiecesPlacingPositions.Calculate(sideLengthMultipler);
            
            for (int i = 0; i < 6; i++)
                interfaceInfo.PiecesButtons.PhysicalData.Buttons[i].GetComponent<ApplySettingToBtn>().ChangeVariant(editorInfo.Variant);

            infoPanel.UpdateInfo();

            if (TutorialScript.firstApplicationLaunch || !levelMenu.IsLevelPassed(0, Variant.x4))
            {
                LevelSettings lS = levelMenu.GetLevelSettings(0, Variant.x4);
                gameplayService.StartNewGame(0, lS.seed, lS.placedSides, lS.finished);
                return;
            }
            if (autoStartRandomGame) gameplayService.StartNewRandomGame(false);
        }

        private void SelectProperPiecePrefab()
        {
            if (editorInfo.IsUsedProceduralGeneratedMesh)
            {
                editorInfo.SelectedPiecePrefab = prefabInfo.PiecePG;
                sideLengthMultipler = (editorInfo.Variant == Variant.x3 ? 1f : 1.5f) * editorInfo.OneSliceOfPieceLength;
            }
            else
            {
                if (editorInfo.Variant == Variant.x3)
                {
                    sideLengthMultipler = 1f * editorInfo.OneSliceOfPieceLength;
                    editorInfo.SelectedPiecePrefab = prefabInfo.Piece3_3;
                }
                if (editorInfo.Variant == Variant.x4)
                {
                    sideLengthMultipler = 1.5f * editorInfo.OneSliceOfPieceLength;
                    editorInfo.SelectedPiecePrefab = prefabInfo.Piece4_4;
                }
            }
        }
    }
}
