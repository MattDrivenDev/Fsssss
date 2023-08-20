namespace Fsssss.Game.II
open Microsoft.Xna.Framework


module Wall = 

    let color = Color.Black

module Food =
    
    let color = Color.Green

module Snake = 

    let color = Color.Orange

module State = 

    let create() = { Map = Array2D.create 30 30 Empty }

    let draw gd tx state = 
        let sb = spriteBatch gd
        sb.Begin()
        Array2D.iteri (fun x y t -> 
            match t with
            | Empty -> ()
            | Wall -> draw sb tx Wall.color 255 (Rectangle(x * 10, y * 10, 10, 10))
            | Food -> draw sb tx Food.color 255 (Rectangle(x * 10, y * 10, 10, 10))
            | Snake -> draw sb tx Snake.color 255 (Rectangle(x * 10, y * 10, 10, 10))
        ) state.Map
        sb.End()