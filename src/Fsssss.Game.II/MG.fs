namespace Fsssss.Game.II
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework

[<AutoOpen>]
module MG = 
   
    type Alpha = int

    let clearScreen (gd : GraphicsDevice) color = 
        gd.Clear color

    let spriteBatch (gd : GraphicsDevice) = 
        new SpriteBatch(gd)

    let draw 
        (sb : SpriteBatch) 
        (tx : Texture2D)
        (c : Color)
        (a : Alpha) 
        (r : Rectangle) =
        sb.Draw(tx, r, c)