namespace Fsssss.Game.II
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

[<AutoOpen>]
module MG = 

    let rect x y w h = 
        new Rectangle(x, y, w, h)
   
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

    let getKeyboardState() = 
        Keyboard.GetState()

    let isKeyDown =
        let ks = getKeyboardState()
        ks.IsKeyDown