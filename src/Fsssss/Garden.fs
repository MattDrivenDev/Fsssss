namespace Fsssss

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

/// The game world is represented as a Garden, it has a wall around it.
module Garden =

    /// Creates a brick at a set of coordinates.
    let layBrick x y = new Rectangle(x, y, 8, 8)

    /// Creates the garden (essentially a 65*65 board, each cell being 8*8 pixels).
    let garden = [for i in 0..64 -> [for j in 0..64 -> new Point(i * 8, j * 8)]]
                        
    /// The snake starts here.
    let snakeyStartingPosition = garden.[30].[30] 

    /// Gets a random position from the garden game-board.
    let randomPosition (random:Random) = 
        garden.[(random.Next(3, 61))].[(random.Next(3, 61))]
            

    let topWall = [for i in 0..64 -> layBrick (i * 8) 0]
    let bottomWall = [for i in 0..64 -> layBrick (i * 8) (64 * 8)]
    let leftWall = [for i in 0..64 -> layBrick 0 (i * 8)]
    let rightWall = [for i in 0..64 -> layBrick (64 * 8) (i * 8)]

    /// Gets the representation of the 4 walls around the edge of the garden.
    let wall = topWall @ bottomWall @ leftWall @ rightWall

    /// Draws all of the garden wall.
    let drawWall
        (spritebatch:SpriteBatch) 
        (texture:Texture2D) =

        List.iter(fun (brick:Rectangle) ->
            spritebatch.Draw(
                texture,
                brick,
                System.Nullable(new Rectangle(0, 0, 8, 8)),
                Color.Black
            )
        ) wall