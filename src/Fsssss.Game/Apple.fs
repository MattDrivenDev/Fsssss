namespace Fsssss.Game
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

/// A type to represent the apple in the garden.
/// Snakes eat apples (well, they do in this game)
type Apple = Apple of Rectangle

module Apple = 

    /// Apples have a width and height of 8 pixels
    let width, height = 8, 8

    /// Drops a new apple at the given coordinates
    let drop x y = new Rectangle(x, y, width, height) |> Apple

    /// Draws the apple on the screen to an existing SpriteBatch
    let draw (spriteBatch : SpriteBatch) (appleTexture : Texture2D) (Apple apple)  =
        spriteBatch.Draw(appleTexture, apple, Color.White)