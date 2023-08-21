namespace Fsssss.Game.II
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input


module Wall = 

    let color = Color.DarkRed

    let build = Array2D.mapi (fun x y _ -> 
        match x, y with
        | 0, _ | _, 0 | 29, _ | _, 29 -> Wall
        | _ -> Empty)

module Food =

    let color = Color.LightGreen

    let rnd() =     
        let rnd = System.Random()
        rnd.Next(1, 28), rnd.Next(1, 28)

    let drop = 
        let x, y = rnd()
        Array2D.mapi (fun i j t ->
            match i, j with
            | i, j when i = x && j = y -> Food
            | _ -> t)

module Snake = 

    let color = Color.Orange

    let rnd() =     
        let rnd = System.Random()
        rnd.Next(1, 28), rnd.Next(1, 28)

    let spawn =
        let x, y = rnd()
        Array2D.mapi (fun i j t ->
            match i, j with
            | i, j when i = x && j = y -> Snake
            | _ -> t)    
    
    let getMovement() = 
        if isKeyDown Keys.Up then Up
        elif isKeyDown Keys.Down then Down
        elif isKeyDown Keys.Left then Left
        elif isKeyDown Keys.Right then Right
        else None

    let move map movement = 
        match movement with
        | None -> map
        | Up -> map
        | Down -> map
        | Left -> map
        | Right -> map

module State = 

    let tileWidth, tileHeight = 10, 10

    let tileRect x y = rect (x * tileWidth) (y * tileHeight) tileWidth tileHeight

    let create() = { Map = Array2D.create 30 30 Empty }

    let buildWall { Map = map } = 
        { Map = Wall.build map }

    let dropFood { Map = map } = 
        { Map = Food.drop map }

    let spawnSnake { Map = map } = 
        { Map = Snake.spawn map }

    let moveSnake { Map = map } = 
        { Map = Snake.move map (Snake.getMovement()) }

    let init = 
        buildWall
        >> dropFood
        >> spawnSnake

    let update =
        moveSnake        

    let drawTile sb tx x y = 
        function
        | Empty -> ()
        | Wall -> draw sb tx Wall.color 255 (tileRect x y)
        | Food -> draw sb tx Food.color 255 (tileRect x y)
        | Snake -> draw sb tx Snake.color 255 (tileRect x y)

    let draw gd tx state = 
        let sb = spriteBatch gd
        let drawTile = drawTile sb tx
        sb.Begin()
        Array2D.iteri drawTile state.Map
        sb.End()