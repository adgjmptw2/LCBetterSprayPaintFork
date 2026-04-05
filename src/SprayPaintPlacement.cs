using BetterSprayPaint.Ngo;
using GameNetcodeStuff;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace BetterSprayPaint;

internal static class SprayPaintPlacement {
    public static void PositionDecal(SprayPaintItem instance, GameObject gameObject, RaycastHit hit, bool setColor = true) {
        gameObject.transform.forward = -hit.normal;
        gameObject.transform.position = hit.point;

        if (hit.collider.gameObject.layer == 11 || hit.collider.gameObject.layer == 8 || hit.collider.gameObject.layer == 0) {
            bool parentToShip = hit.collider.transform.IsChildOf(StartOfRound.Instance.elevatorTransform) || RoundManager.Instance.mapPropsContainer == null;
            if (parentToShip) {
                gameObject.transform.SetParent(StartOfRound.Instance.elevatorTransform, true);
            } else {
                gameObject.transform.SetParent(RoundManager.Instance.mapPropsContainer!.transform, true);
            }
        }

        var projector = gameObject.GetComponent<DecalProjector>();
        projector.enabled = true;

        var c = instance.NetExt();

        projector.drawDistance = Plugin.DrawDistance;

        if (setColor) { projector.material = c.DecalMaterialForColor(c.CurrentColor); }

        projector.scaleMode = DecalScaleMode.InheritFromHierarchy;
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        var parentScale = gameObject.transform.lossyScale;
        gameObject.transform.localScale = new Vector3(
            (1f / parentScale.x) * c.PaintSize,
            (1f / parentScale.y) * c.PaintSize,
            1.0f
        );
    }
}
