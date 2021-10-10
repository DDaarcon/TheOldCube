using UnityEngine;

using GameInfo;
using GameInfo.GameInfoInternals.SolutionInfoInternals;
using GameInfo.GameInfoInternals.InterfaceInfoInternals.PiecesButtonsInfoInternals;
using GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals;
using GameExtensions.Pieces;
using static Enums;
using GameExtensions.SideData;
using GameInfo.GameInfoInternals.InterfaceInfoInternals;

namespace GameExtensions.Interface
{
    public static class PieceButtonExtensions
    {
        private static PiecesPrefabsInfo piecesPrefabsInfo => GameInfoHolder.Information.EditorEnvironment.PiecesPrefabs;
        private static PiecesButtonsInfo piecesButtonsInfo => GameInfoHolder.Information.Interface.PiecesButtons;

        // TODO: Piece component needs refactor for SideData type using
        public static void ChangePieceSetting(this PieceButtonInfo info, SideData<bool> setting)
        {
            info.SettingForPiece = setting;
            info.PieceComponent.ChangeSetting(setting.Data);
        }

        public static void SetDefaultColorForPiece(this PieceButtonInfo info)
        {
            info.PieceComponent.ChangeColor(piecesButtonsInfo.ColorForPieces);
        }

        public static void ChangeVariant(this PieceButtonInfo info, Variant variant)
        {
            if (info.Variant == variant)
                return;

            info.Variant = variant;

            RecreatePiece(info);
        }

        private static void RecreatePiece(PieceButtonInfo info)
        {
            Vector3 position = info.Piece.transform.position;
            Quaternion rotation = info.Piece.transform.rotation;
            Vector3 scale = info.Piece.transform.localScale;
            Color color = info.Piece.GetComponent<Piece>().color;
            Object.Destroy(info.Piece);

            info.Piece = Object.Instantiate(piecesPrefabsInfo.GetByVariant(info.Variant), position, rotation);
            info.SettingForPiece = new SideData<bool>(info.Variant);
            info.SettingForPiece.Clear(true);
            info.SettingForPiece.DefaultValue = false;

            info.Piece.transform.parent = info.PhysicalObject.transform;
            info.Piece.transform.localScale = scale;
            info.PieceComponent.ChangeLayer(piecesButtonsInfo.CellCameraDiplayLayer);
            info.PieceComponent.ChangeColor(color);
        }

        public static void SetEnableState(this PieceButtonInfo info, bool enable)
        {
            info.IsEnabled = enable;
            if (!enable)
            {
                info.PieceComponent.ChangeTransparency(0.2f);
                info.PhysicalObject.interactable = false;
                Color color = info.BackgroundImage.color;
                color.a = 0.2f;
                info.BackgroundImage.color = color;
            }
            else
            {
                info.PieceComponent.ChangeTransparency(1f);
                info.PhysicalObject.interactable = true;
                Color color = info.BackgroundImage.color;
                color.a = 1f;
                info.BackgroundImage.color = color;
            }
        }
    }
}
