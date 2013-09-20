namespace Fsssss

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

/// Type to represent whether or not a snake is eating or not, and
/// if it is - how many apples are they eating?
type Eating = | NotEating | Eating of int

/// Represents the direction the snake is slithering.
type Direction = | Up | Right | Down | Left

/// Represents the snake itself - little more than a list of rectangles 
/// a direction it is travelling in, and information about what it is eating (or not).
type Snakey = Snakey of Direction * Rectangle list * Eating

/// A type to pass data about to regulate moving at regular intervals in
/// time, rather than each and every pass around the game loop.
type CanMove = | Yes | No of float

module Snake = 

    /// Standard height and width of a part of a snake in pixels.
    let partWidth, partHeight = 8, 8

    /// The number of milliseconds that must pass in the game loop before we 
    /// move the snake.
    let movetime = 80.0
    
    /// Check to see if we can move the snake, based on the current
    /// game time and how long it was ago since the snake last moved.
    /// If we cannot move, we pass back and updated time since last move.
    let canMove lastmovetime (gametime:GameTime) =
        let total = lastmovetime + gametime.ElapsedGameTime.TotalMilliseconds
        if  total >= movetime 
            then Yes
            else No total           

    /// Creates a new part for the snake's body.
    let newPart (x, y) = new Rectangle(x, y, partWidth, partHeight)

    /// Move the head of the snake 1 position up the game board.
    let headUp (neck:Rectangle) = (neck.Left, neck.Top - partHeight) |> newPart        

    /// Move the head of the snake 1 position down the game board.
    let headDown (neck:Rectangle) = (neck.Left, neck.Bottom) |> newPart

    /// Move the head of the snake 1 position right along the game board.
    let headRight (neck:Rectangle) = (neck.Right, neck.Top) |> newPart

    /// Move the head of the snake 1 position left along the game board.
    let headLeft (neck:Rectangle) =  (neck.Left - partWidth, neck.Top) |> newPart
    
    /// Creates the snake anew.
    let create x y =
        let tail = newPart (x, y)
        let body = headRight tail
        let head = headRight body
        Snakey (Right, [head;body;tail;], NotEating)

    /// Puts a new part (head) onto the body of the snake (at the front),
    /// which emulates it moving forwards.
    let pushHead head body = head :: body

    /// Drops the last part (tail-end) from the body of the snake.
    let popTail body =         
        body
        |> List.mapi(fun n part -> 
            not (n + 1 = body.Length), part
        )
        |> List.filter(fun (keep, part) -> keep)
        |> List.map(fun (keep, part) -> part)

    /// If the snake is eating, we need to grow it instead of dropping
    /// the end of the tail.
    let popOrGrowTail eating body = 
        match eating with
        | NotEating -> popTail body, NotEating
        | Eating n -> 
            if n >= 1 then body, Eating(n - 1)
            else popTail body, NotEating

    /// Moving the snake essentially works like a simple queue of rectangles.
    /// We push a new rectangle to the front of the queue to move the head
    /// forwards, and pop a rectangle on the end of the queue to mvoe the tail
    /// forwards. Yeah I'm using a linked list and not a queue, so what.
    let move (Snakey (direction, body, eating)) = 
        let newhead = 
            match direction with
            | Up ->    headUp body.Head
            | Right -> headRight body.Head
            | Down ->  headDown body.Head
            | Left ->  headLeft body.Head
        let newbody, neweating = body |> pushHead newhead |> popOrGrowTail eating
        Snakey (direction, newbody, neweating)

    /// Turning the snake is an act of altering it's direction. But not all
    /// turns are valid - snakes cannot 180!
    let turn (Snakey (direction, body, eating)) newDirection =  
        let nochange = Snakey(direction, body, eating)       
        match direction, newDirection with
        // No change - already in that direction
        | Up, Up -> nochange
        | Down, Down -> nochange
        | Left, Left -> nochange
        | Right, Right -> nochange
        // You can't do a 180
        | Up, Down -> nochange
        | Down, Up -> nochange
        | Left, Right -> nochange
        | Right, Left -> nochange
        // Okay, you may now turn!
        | _, _ -> Snakey (newDirection, body, eating)

    /// The player doesn't have to be turning the snake all the time, so
    /// we need to handle optional directional input, basically.
    let maybeTurn snakey maybeDirection = 
        match maybeDirection with
        | Some newDirection -> turn snakey newDirection
        | None -> snakey

    /// Collision detection between the snake and an apple.
    let touchingApple (Snakey (direction, body, eating)) (Apple apple) = 
        body.Head.Intersects(apple)

    /// Collision detection between the head and tail of the snake.
    let touchingSelf (Snakey (direction, body, eating)) = 
        body.Tail |> List.map(fun part ->
            body.Head.Intersects(part)
        )
        |> List.reduce (||)  
        
    /// Collision detection between the snake and the garden wall.
    let touchingWall (Snakey (direction, body, eating)) wall = 
        wall |> List.map(fun brick ->
            body.Head.Intersects(brick)
        )
        |> List.reduce (||)

    /// Make the snake eat an apple, *nom-nom*.
    let eat (Snakey (direction, body, eating)) (Apple apple) = 
        match eating with 
        | NotEating -> 
            Snakey(direction, body, Eating(1)), Option<Apple>.None
        | Eating n -> 
            Snakey(direction, body, Eating(n + 1)), Option<Apple>.None

    /// Draw one part of the snake. We do one part at a time because
    /// there is a fade effect along the length of the snake.
    let drawPart 
        (spritebatch:SpriteBatch) 
        (texture:Texture2D) 
        alpha
        (p:Rectangle) =
                
        spritebatch.Draw(
            texture,
            p,
            Nullable(new Rectangle(0, 0, partWidth, partHeight)),
            Color.Magenta * alpha
        )

    /// Draws the whole snake to the spritebatch - takes care of the fade-effect.
    let draw    
        (spritebatch:SpriteBatch) 
        (texture:Texture2D)
        (Snakey (direction, body, eating)) = 
                
        List.iteri(fun i part ->
            let alpha = 
                MathHelper.Clamp(
                    1.0f - float32 i * 0.05f,
                    0.05f, 1.0f
                )
            drawPart spritebatch texture alpha part
        ) body