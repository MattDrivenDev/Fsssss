namespace Fsssss

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

/// A type to represent apples in the garden.
type Apple = Apple of Rectangle

module Apples = 

    /// Apples have a height and width of 8 pixels.
    let width, height = 8, 8

    /// Drops a new apple in the garden at a specified set of coordinates.
    let drop x y = new Rectangle(x, y, width, height) |> Apple

    /// Draws an apple to a spritebatch.
    let draw
        (spritebatch:SpriteBatch) 
        (texture:Texture2D)         
        (Apple a) =

        spritebatch.Draw(
            texture, 
            a,
            System.Nullable(new Rectangle(0, 0, width, height)),
            Color.GreenYellow
        )