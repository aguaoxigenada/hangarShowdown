# Hangar Showdown

A first-person shooter set in an industrial hangar, built with Unity. Fight through waves of AI enemies using a variety of weapons.

## Gameplay
- First-person movement and shooting
- Wave-based enemy encounters
- AI enemies with sight, hearing, NavMesh navigation, and ragdoll death
- Weapons: hitscan rifles, multi-shot, melee, projectile-based, grenade launcher
- Intro cinematic with animated characters

## Project Structure
- `Assets/AI/` — enemy AI (navigation, sight, noise detection)
- `Assets/Player/` — player movement, camera, shooting, animation
- `Assets/WeaponSystem/` — weapon base classes, raycast, melee, projectile, explosion
- `Assets/Encounters/` — wave/encounter management
- `Assets/UI/` — game UI, menus, audio settings
- `Assets/Scenes/` — game levels and intro menu

## Notes
- Binary assets (textures, 3D models) are not tracked in git — reimport from original sources if needed
- Audio clips and all scripts/scenes/prefabs are tracked
- Built with Unity 2021 (URP)
