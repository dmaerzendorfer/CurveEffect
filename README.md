# CurveEffect

A Unity prototype of a curved-world ("bending world") shader with a papercraft look. The world bends away from the camera, and flat 2D sprites are turned into thick cardboard cutouts that a small player character runs between.

## Features

- **World bending.** The `WorldBending` and `UnlitWorldBending` Shader Graphs bend vertices away from the camera. `BendingManager` sets the global `_BendingAmount` and toggles the `_ENABLE_BENDING` keyword. It also widens the camera's culling matrix so bent objects outside the normal frustum still render.
- **Cardboarder.** Extrudes a sprite's `PolygonCollider2D` outline into a cardboard edge mesh, adds a front and back face, and can copy the sprite onto the back side.
- **Player.** A tween-animated character (idle and run animations) with eased acceleration and deceleration and a VFX Graph dust trail.
- **Skybox.** A custom skybox Shader Graph.

## Getting started

1. Open the `CurveEffect/` folder in **Unity 6000.3.8f1** (URP).
2. Open `Assets/_Project/Scenes/TestScene.unity`.
3. Press Play. Change the bend strength on the `BendingManager` component.

## Status

Early prototype. The skybox and clouds still need work.

## Third-party

[PrimeTween](https://github.com/KyryloKuzyk/PrimeTween), [EditorAttributes](https://github.com/v0lt13/EditorAttributes).
