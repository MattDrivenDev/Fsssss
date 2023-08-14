namespace Fsssss.Game
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Microsoft.Xna.Framework

type FsssssGame() as this = 
    inherit FsGame(
        "Content", 
        (fun gfx ->
            gfx.PreferredBackBufferWidth <- 520
            gfx.PreferredBackBufferHeight <- 520
            gfx.GraphicsProfile <- GraphicsProfile.HiDef
            gfx.IsFullScreen <- false
        )
    )    
        
    let mutable texture = Unchecked.defaultof<_>                       

    let mutable snakey, movetime = 
        let position = Garden.snakeyStartingPosition
        Snake.create position.X position.Y, 0.0

    let mutable apple = None

    /// Grabs the current keyboard state and converts it
    /// over to a snake direction if applicable.
    let playerInput() = 
        let state = Keyboard.GetState()
        if state.IsKeyDown(Keys.Up) then Some Up
        else if state.IsKeyDown(Keys.Down) then Some Down
        else if state.IsKeyDown(Keys.Left) then Some Left
        else if state.IsKeyDown(Keys.Right) then Some Right
        else None
    
    let loadTextures _ =
        texture <- new Texture2D(this.GraphicsDevice, 8, 8, false, SurfaceFormat.Color)
        texture.SetData [| for i in 0 .. 63 -> Color.Magenta |]

    let whiteBackground _ _ = 
        this.Graphics.GraphicsDevice.Clear(Color.White)

    let beginSpritebatch (spritebatch : SpriteBatch) _ = 
        spritebatch.Begin()

    let endSpritebatch (spritebatch : SpriteBatch) _ = 
        spritebatch.End()

    /// Function to track the state of the apple, and drops a new on
    /// in the garden somewhere if there isn't one available.
    let dropApple gametime = 
        let position = Garden.randomPosition this.Random
        match apple with
        | Some a -> ()
        | None -> apple <- Some (Apple.drop position.X position.Y)

    /// Function to track the game time and move the snake if we are allowed.
    let moveSnakey gametime =
        match Snake.canMove movetime gametime with
        | Yes -> 
            snakey <- Snake.move snakey
            movetime <- 0.0
        | No elapsed ->
            movetime <- elapsed

    /// Function to turn the snake per user input.
    let turnSnakey gametime =
        let direction = playerInput()
        snakey <- Snake.maybeTurn snakey direction

    /// Function to see if the snake is able to eat an apple, or not.
    /// If so - he damned well will!
    let snakeyTryEatApple gametime =
        match apple with 
        | Some a ->
            if Snake.touchingApple snakey a 
                then 
                    let update (s, a) = 
                        snakey <- s
                        apple <- a
                    Snake.eat snakey a |> update
                else ()                   
        | None -> ()

    /// Check for lose conditions (collisions with snake-tail or the garden wall).
    /// We just restart the game if the player loses.
    let checkForGameOver _ =
        if Snake.touchingSelf snakey 
            || Snake.touchingWall snakey Garden.wall then 
            let position = Garden.snakeyStartingPosition
            snakey <- Snake.create position.X position.Y
            movetime <- 0.0         

    let drawWall spritebatch _ =
        Garden.draw spritebatch texture

    let drawSnakey spritebatch _ =
        Snake.draw spritebatch texture snakey

    let drawApple spritebatch _ =   
        match apple with
        | Some a -> Apple.draw spritebatch texture a
        | None -> ()

    override this.InitializeSteps = []

    override this.LoadContentSteps = 
        [ loadTextures; ]

    /// List of functions that will be run each Update in the game loop.
    /// We use a list because they are ordered.
    /// Most are closures over mutable state.
    override this.UpdateSteps = 
        [ checkForGameOver;
          dropApple;
          turnSnakey;
          moveSnakey;
          snakeyTryEatApple; ]

    /// List of functions that will be run each Draw in the game loop.
    override this.RenderSteps = 
        [ whiteBackground; 
          beginSpritebatch; 
          drawWall;
          drawApple;
          drawSnakey; 
          endSpritebatch; ]